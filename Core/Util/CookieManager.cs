using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Collections.Specialized;

namespace YX.Core
{
    /// <summary>
    /// HttpCookie管理器。
    /// </summary>
    public static class CookieManager
    {
        #region 获取cookie的值

        /// <summary>
        /// 根据某cookie名称获取cookie值。
        /// </summary>
        /// <param name="cookieName">cookie名称</param>
        /// <returns></returns>
        public static string GetCookie(string cookieName)
        {
            return GetCookie(cookieName, null);
        }

        /// <summary>
        /// 根据某cookie名称获取cookie值或指定键的值。
        /// </summary>
        /// <param name="cookieName">cookie名称</param>
        /// <param name="cookieKey">cookie键</param>
        /// <returns></returns>
        public static string GetCookie(string cookieName, string cookieKey)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (null == cookie)
                return null;

            if (string.IsNullOrEmpty(cookieKey))
                return HttpUtility.UrlDecode(cookie.Value);

            foreach (string _key in cookie.Values.AllKeys)
            {
                if (_key.Trim().ToLower() == cookieKey.Trim().ToLower())
                    return HttpUtility.UrlDecode(cookie.Values[_key]);
            }

            return null;
        }

        /// <summary>
        /// 根据某cookie名称获取cookie值或指定键的值。
        /// </summary>
        /// <param name="cookieName">cookie名称</param>
        /// <param name="cookieKey">cookie键</param>
        /// <returns></returns>
        public static NameValueCollection GetCookieValues(string cookieName)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (null == cookie)
                return null;

            return cookie.Values;
        }

        /// <summary>
        /// 获取所有cookie名称。
        /// </summary>
        /// <returns></returns>
        public static string GetAllCookie()
        {
            string t = string.Empty;
            HttpCookieCollection cookies = HttpContext.Current.Request.Cookies;
            for (int i = 0; i < cookies.Count; i++)
            {
                t += cookies[i].Name + "<br/>";
            }

            return t;
        }

        /// <summary>
        /// 移除所有cookie。
        /// </summary>
        /// <returns></returns>
        public static void RemoveAllCookie()
        {
            HttpCookieCollection cookies = HttpContext.Current.Request.Cookies;
            int len = cookies.Count;
            for (int i = 0; i < len; i++)
            {
                cookies[i].Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(cookies[i]);
            }

            cookies = HttpContext.Current.Request.Cookies;
            len = cookies.Count;
            for (int i = 0; i < len; i++)
            {
                cookies[i].Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(cookies[i]);
            }

            HttpContext.Current.Response.Cookies.Clear();
        }

        #endregion

        #region 写入cookie

        /// <summary>
        /// 写入cookie。
        /// </summary>
        /// <param name="cookieName">cookie名称</param>
        /// <param name="cookieValue">cookie值</param>
        public static void WriteCookie(string cookieName, string cookieValue)
        {
            WriteCookie(cookieName, cookieValue, null);
        }


        /// <summary>
        /// 写入cookie，如果不存在创建，否则打开原有的cookie。
        /// </summary>
        /// <param name="cookieName">cookie名称</param>
        /// <param name="cookieKey">cookie键</param>
        /// <param name="cookieValue">cookie值</param>
        /// <param name="cookieExpires">cookie过期日期(若参数为空，默认为30分钟)</param>
        public static void WriteCookie(string cookieName, string cookieValue, DateTime? cookieExpires)
        {
            if (null == cookieExpires)
                cookieExpires = DateTime.Now.AddMinutes(30);

            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (null == cookie)
                cookie = new HttpCookie(cookieName);
            cookie.Value = HttpUtility.UrlEncode(cookieValue);             
            cookie.Expires = cookieExpires.Value;
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 将cookie键值集合写入或更新到cookie中。
        /// </summary>
        /// <param name="cookieName">cookie名称</param>
        /// <param name="cookieKeyValues">cookie键值集合</param>
        /// <param name="cookieExpires">cookie过期日期(若参数为空，默认为30分钟)</param>
        public static void WriteCookie(string cookieName, NameValueCollection cookieKeyValues, DateTime? cookieExpires)
        {
            if (null == cookieExpires)
                cookieExpires = DateTime.Now.AddMinutes(30);

            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (null == cookie)
                cookie = new HttpCookie(cookieName);

            cookie.Values.Clear();
            for (int i = 0; i < cookieKeyValues.Count; i++)
                cookie.Values.Add(cookieKeyValues.GetKey(i), HttpUtility.UrlEncode(cookieKeyValues.Get(i)));
            cookie.Expires = cookieExpires.Value;
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        #endregion

        #region 删除cookie

        /// <summary>
        /// 删除cookie。
        /// </summary>
        /// <param name="cookieName">cookie名称</param>
        public static void RemoveCookie(string cookieName)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (null != cookie)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        /// <summary>
        /// 删除cookie某一键。
        /// </summary>
        /// <param name="cookieName">cookie名称</param>        
        /// <param name="cookieKey">cookie键</param>
        public static void RemoveCookieItem(string cookieName, string cookieKey)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (null != cookie)
            {
                string targetKey = null;
                foreach (string _key in cookie.Values.Keys)
                {
                    if (_key.Trim().ToLower() == cookieKey.Trim().ToLower())
                    {
                        targetKey = _key;
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(targetKey))
                    cookie.Values.Remove(targetKey);
            }
        }

        #endregion
    }
}
