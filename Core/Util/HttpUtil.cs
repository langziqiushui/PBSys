using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace YX.Core
{
    /// <summary>
    /// Http请求处理。
    /// </summary>
    public static class HttpUtil
    {
        #region 变量

        #endregion

        #region cookie容器

        /// <summary>
        /// 创建Cookie容器。
        /// </summary>
        /// <returns></returns>
        public static CookieContainer CreateCookie()
        {
            return new CookieContainer();
        }

        /// <summary>
        /// 获取 cookie容器。
        /// </summary>
        public static CookieContainer MyCookie
        {
            get
            {
                if (null == HttpContext.Current)
                    return null;

                var cookie = HttpContext.Current.Session["CookieContainer98562"] as CookieContainer;
                if (null == cookie)
                {
                    cookie = CreateCookie();
                    HttpContext.Current.Session["CookieContainer98562"] = cookie;
                }

                return cookie;
            }
        }

        /// <summary>
        /// 获取指定URI下的Cookie键和值。
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string GetCookeValues(Uri uri)
        {
            var t = string.Empty;
            foreach (Cookie _cookie in HttpUtil.MyCookie.GetCookies(new Uri("http://b2b.cits.com.cn")))
            {
                t += _cookie.Name + ":" + _cookie.Value + "\n";
            }
            return t;
        }

        #endregion

        #region 模拟浏览器请求数据

        /// <summary>
        /// 模拟浏览器GET数据。
        /// </summary>
        /// <param name="url">请求网址</param>
        /// <returns></returns>
        public static string GetData(string url)
        {
            return GetData(url, Encoding.UTF8);
        }

        /// <summary>
        /// 模拟浏览器GET数据。
        /// </summary>
        /// <param name="url">请求网址</param>
        /// <param name="en">字符编码</param>
        /// <returns></returns>
        public static string GetData(string url, Encoding en)
        {
            var req = (HttpWebRequest)WebRequest.Create(url);
            req.CookieContainer = MyCookie;
            req.KeepAlive = true;
            req.Method = "GET";
            req.Host = "flights.ctrip.com";
            req.ContentType = "application/x-www-form-urlencoded";
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:29.0) Gecko/20100101 Firefox/29.0";
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            req.AllowAutoRedirect = true;
            req.Timeout = 50000;
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";

            return GetData(req, en);
        }

        /// <summary>
        /// 模拟浏览器GET数据。
        /// </summary>
        /// <param name="url">请求网址</param>
        /// <param name="en">编码</param>
        /// <returns></returns>
        public static string GetData(HttpWebRequest req, Encoding en)
        {
            if (null == req.CookieContainer)
                req.CookieContainer = MyCookie;

            if (req.RequestUri.ToString().StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                req.ProtocolVersion = HttpVersion.Version10;
            }           

            var res = (HttpWebResponse)req.GetResponse();
            //CookieCollection cook = res.Cookies;            
            using (var resStream = res.GetResponseStream())
            {
                using (var sr = new StreamReader(resStream, en))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// 模拟浏览器下载数据。
        /// </summary>
        /// <param name="url">请求网址</param>
        /// <returns></returns>
        public static Stream DownloadData(HttpWebRequest req)
        {
            if (null == req.CookieContainer)
                req.CookieContainer = MyCookie;

            System.Net.ServicePointManager.ServerCertificateValidationCallback += (se, cert, chain, sslerror) =>
            {
                return true;
            };

            return ((HttpWebResponse)req.GetResponse()).GetResponseStream();
        }

        public static string PrintCookie(CookieCollection cookies)
        {
            var s = "";
            foreach (System.Net.Cookie _c in cookies)
                s += _c.Name + ":" + _c.Value + "(" + _c.Domain + ")" + "，";

            return s;
        }

        /// <summary>
        /// 模拟浏览器GET数据。
        /// </summary>
        /// <param name="url">请求网址</param>
        /// <param name="en">编码</param>
        /// <returns></returns>
        public static void GetDataAsync(HttpWebRequest req, AsyncCallback callback)
        {
            if (null == req.CookieContainer)
                req.CookieContainer = MyCookie;

            System.Net.ServicePointManager.ServerCertificateValidationCallback += (se, cert, chain, sslerror) =>
            {
                return true;
            };

            req.BeginGetResponse(callback, req);
        }


        /// <summary>
        /// 模拟浏览器请求Post数据。
        /// </summary>
        /// <param name="url">对方API网址</param>
        /// <param name="postData">请求数据</param>
        /// <returns></returns>
        public static string PostData(string url, string postData)
        {
            var req = (HttpWebRequest)WebRequest.Create(url);
            req.KeepAlive = true;
            req.Method = "POST";
            req.AllowAutoRedirect = true;
            req.CookieContainer = MyCookie;
            req.Host = "kyfw.12306.cn";
            req.ContentType = "application/x-www-form-urlencoded";
            req.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; InfoPath.2; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; .NET4.0C; .NET4.0E)";
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            req.AllowAutoRedirect = true;
            req.Timeout = 50000;

            return PostData(req, postData, Encoding.UTF8);
        }

        /// <summary>
        /// 模拟浏览器请求Post数据。
        /// </summary>
        /// <param name="req">http请求</param>
        /// <param name="postData">请求数据</param>
        /// <param name="en">编码</param>
        /// <returns></returns>
        public static string PostData(HttpWebRequest req, string postData, Encoding en)
        {
            CookieCollection cookies = null;
            return PostData(req, postData, en, ref cookies);
        }

        /// <summary>
        /// 模拟浏览器请求Post数据。
        /// </summary>
        /// <param name="req">http请求</param>
        /// <param name="postData">请求数据</param>
        /// <param name="en">编码</param>
        /// <param name="cookies">从服务器端获取到的关联cookie</param>
        /// <returns></returns>
        public static string PostData(HttpWebRequest req, string postData, Encoding en, ref CookieCollection cookies)
        {
            if (null == req.CookieContainer)
                req.CookieContainer = MyCookie;

            req.MaximumAutomaticRedirections = 4;
            req.MaximumResponseHeadersLength = 4;
            req.Credentials = CredentialCache.DefaultCredentials;

            var buffer = en.GetBytes(postData);
            req.ContentLength = buffer.Length;
            using (var reqStream = req.GetRequestStream())
            {
                reqStream.Write(buffer, 0, buffer.Length);
            }

            if (req.RequestUri.ToString().StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                req.ProtocolVersion = HttpVersion.Version10;
            }

            var res = (HttpWebResponse)req.GetResponse();
            res.Cookies = req.CookieContainer.GetCookies(req.RequestUri);
            cookies = res.Cookies;
            //CookieCollection cook = res.Cookies;            
            using (var resStream = res.GetResponseStream())
            {
                using (var sr = new StreamReader(resStream, en))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Htpp Post请求数据。
        /// </summary>
        /// <param name="url">对方API网址</param>
        /// <param name="postData">请求数据</param>
        /// <param name="enCoding">字符编码</param>
        /// <returns></returns>
        public static string PostData(string url, byte[] buffer, Encoding enCoding)
        {
            var req = (HttpWebRequest)WebRequest.Create(url);
            req.KeepAlive = true;
            req.Method = "POST";
            req.AllowAutoRedirect = true;
            //req.CookieContainer = MyCookie;
            req.ContentType = "application/x-www-form-urlencoded";
            req.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; InfoPath.2; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; .NET4.0C; .NET4.0E)";
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            req.AllowAutoRedirect = true;
            req.Timeout = 50000;

            req.ContentLength = buffer.Length;
            using (var reqStream = req.GetRequestStream())
            {
                reqStream.Write(buffer, 0, buffer.Length);
            }

            System.Net.ServicePointManager.ServerCertificateValidationCallback += (se, cert, chain, sslerror) =>
            {
                return true;
            };

            var res = (HttpWebResponse)req.GetResponse();
            using (var resStream = res.GetResponseStream())
            {
                using (var sr = new StreamReader(resStream, enCoding))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        #endregion

        #region 填充基本的HttpWebRequest

        /// <summary>
        /// 填充基本的HttpWebRequest。
        /// </summary>
        /// <param name="req">HttpWebRequest对象</param>
        /// <param name="method">请求方法(取值：GET OR POST)</param>
        /// <param name="host">Host标头值</param>
        /// <param name="referer">当前引用页面网址</param>
        public static void FillWebRequest(HttpWebRequest req, string method, string host, string referer)
        {
            if (null == req)
                return;

            if (null == req.CookieContainer)
                req.CookieContainer = HttpUtil.MyCookie;

            req.KeepAlive = true;
            req.Method = method;
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            req.AllowAutoRedirect = true;
            req.ContentType = "application/x-www-form-urlencoded";
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:36.0) Gecko/20100101 Firefox/36.0";
            req.AllowAutoRedirect = true;
            req.Timeout = 50000;
            req.Host = host;
            req.Referer = referer;
        }

        /// <summary>
        /// 创建HTTP请求对象。
        /// </summary>
        /// <param name="cookie">Cookie对象</param>
        /// <param name="url">请求网址</param>
        /// <param name="method">请求方法(取值：GET OR POST)</param>
        /// <param name="host">Host标头值</param>
        /// <param name="referer">当前引用页面网址</param>
        /// <returns></returns>
        public static HttpWebRequest CreateWebRequest(CookieContainer cookie, string url, string method, string host, string referer)
        {
            var req = (HttpWebRequest)WebRequest.Create(url);
            req.CookieContainer = cookie;
            FillWebRequest(req, method, host, referer);
            return req;
        }

        #endregion        

        #region 获取所有参数

        /// <summary>
        /// 获取所有请求参数(GET OR POST)。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetRequestString(HttpContext context)
        {
            return context.Request.RequestType.IgnoreCaseEquals("POST") ? context.Request.Form.ToString() : context.Request.QueryString.ToString();
        }

        #endregion

        #region 其他方法

        /// <summary>
        /// 验证证书。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        #endregion
    }
}
