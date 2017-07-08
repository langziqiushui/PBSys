using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using System.Web.Caching;
using System.Net;
using System.Drawing;
using System.Security.Cryptography;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Data;
using System.Net.Security;

namespace YX.Core
{
    /// <summary>
    /// 项目通用工具类。
    /// </summary>
    public static class CoreUtil
    {
        #region 静态构造器

        static CoreUtil()
        {
            lock (typeof(CoreUtil))
            {
                //初始化盘古分词。
                //PanGu.Segment.Init();
            }
        }

        #endregion

        #region 获取当前用户的用户名

        /// <summary>
        /// 获取当前用户的用户名。
        /// </summary>
        public static string CurrentUserName
        {
            get
            {
                if (null == HttpContext.Current || null == HttpContext.Current.User || null == HttpContext.Current.User.Identity || !HttpContext.Current.User.Identity.IsAuthenticated)
                    return null;

                return HttpContext.Current.User.Identity.Name;
            }
        }

        #endregion

        #region 获取当前网站的域名

        /// <summary>
        /// 获取当前网站的域名。
        /// </summary>
        public static string WebSiteHOST
        {
            get
            {
                if (null == HttpContext.Current)
                    return string.Empty;
                return "http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"];
            }
        }

        #endregion

        #region 利用反射创建对象

        /// <summary>
        /// 利用反射创建对象。
        /// </summary>
        /// <param name="assemblyFullName">程序集完整名称</param>
        /// <param name="classFullName">类完整名称</param>
        /// <param name="args">对象构造初始参数</param>
        /// <returns></returns>
        public static object CreateObject(string assemblyFullName, string classFullName, object[] args)
        {
            Assembly _assembly = Assembly.Load(assemblyFullName);
            Type _type = _assembly.GetType(classFullName, false);
            if (null != _type)
                return Activator.CreateInstance(_type, args);

            return null;
        }

        #endregion

        #region 字符串XML转义和反转

        /// <summary>
        /// 替换单引号'。
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ReplaceAPOS(string text)
        {
            return text.Replace("'", "&apos;");
        }

        /// <summary>
        /// 替换双引号"。
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ReplaceQUOT(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            return text.Replace("\"", "&quot;");
        }

        /// <summary>
        /// xml标签转义 。
        /// </summary>
        /// <param name="text">要进行xml转义的字符串</param>
        /// <returns></returns>
        public static string XMLTagTransferred(string text)
        {
            //转义对象。
            //	&lt;	<	小于号 
            //	&gt;	>	大于号 
            //	&amp;	&	和 
            //	&apos;	'	单引号 
            //	&quot;	"	双引号 

            if (string.IsNullOrEmpty(text))
                return string.Empty;

            //以实体名称转义。
            return text
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("'", "&apos;")
                .Replace("\"", "&quot;");

            //以实体编号转义。
            //return text
            //    .Replace("&", "&#38;")
            //    .Replace("<", "&#60;")
            //    .Replace(">", "&#62;")
            //    .Replace("'", "&#39;")
            //    .Replace("\"", "&#34;");
        }

        /// <summary>
        /// 引号转义(单引号和双引号)。
        /// </summary>
        /// <param name="text">要进行转义的字符串</param>
        /// <returns></returns>
        public static string QuotationTransferred(string text)
        {
            //转义对象。           
            //	&apos;	'	单引号 
            //	&quot;	"	双引号 

            if (string.IsNullOrEmpty(text))
                return string.Empty;

            //以实体名称转义。
            return text
                .Replace("'", "&apos;")
                .Replace("\"", "&quot;");
        }

        /// <summary>
        /// javascript转义。
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string EscapeForJavascript(string text)
        {
            //小于号(<)。
            text = text.Replace("<", @"\&lt;");
            text = Regex.Replace(text, @"((\\)*&lt;)|((\\)*&#60;)", @"\&lt;");

            //大于号(>)。
            text = text.Replace(">", @"\&gt;");
            text = Regex.Replace(text, @"((\\)*&gt;)|((\\)*&#62;)", @"\&gt;");

            //双引号(")。
            text = text.Replace("\"", @"\&quot;");
            text = Regex.Replace(text, @"((\\)*&quot;)|((\\)*&#34;)", @"\&quot;");

            //单引号(')。
            text = text.Replace("'", @"\&apos;");
            text = Regex.Replace(text, @"((\\)*&apos;)|((\\)*&#39;)", @"\&apos;");

            return text;
        }

        /// <summary>
        /// 反转为xml标签。
        /// </summary>
        /// <param name="text">将要进行反转的字符串</param>
        /// <returns></returns>
        public static string XMLTagReverse(string text)
        {
            //转义对象。
            //	&lt;	<	小于号 
            //	&gt;	>	大于号 
            //	&amp;	&	和 
            //	&apos;	'	单引号 
            //	&quot;	"	双引号 

            if (string.IsNullOrEmpty(text))
                return string.Empty;

            return text
                .Replace("&amp;", "&")
                .Replace("&lt;", "<")
                .Replace("&gt;", ">")
                .Replace("&apos;", "'")
                .Replace("&quot;", "\"");
        }

        /// <summary>
        /// xml标签转义。
        /// </summary>
        /// <param name="text">要进行xml转义的字符串</param>
        /// <returns></returns>
        public static string RemoveXMLTag(string text)
        {
            //转义对象。
            //	&lt;	<	小于号 
            //	&gt;	>	大于号 
            //	&amp;	&	和 
            //	&apos;	'	单引号 
            //	&quot;	"	双引号 

            if (string.IsNullOrEmpty(text))
                return string.Empty;

            //以实体名称转义。
            return text
                .Replace("&", string.Empty)
                .Replace("<", string.Empty)
                .Replace(">", string.Empty)
                .Replace("'", string.Empty)
                .Replace("\"", string.Empty);
        }

        /// <summary>
        /// 将winform标识符转换为web标签。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ConvertWinTagToWeb(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            return str
                .Replace("\r\n", "<br/>")
                .Replace("\n", "<br/>")
                .Replace("\r", "<br/>")
                .Replace(" ", "&nbsp;")
                .Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;")
                ;
        }

        /// <summary>
        /// 将web标签符转换为win标识。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ConvertWebTagToWin(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            return str
                .IgnoreCaseReplace("<br/>", "\n")
                .IgnoreCaseReplace("<br>", "\n")
                .Replace("    ", "\t")
                .IgnoreCaseReplace("&nbsp;&nbsp;&nbsp;&nbsp;", "\t")
                .IgnoreCaseReplace("&nbsp;", " ")
                ;
        }

        /// <summary>
        /// 删除Win标记。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveWinTag(string str)
        {
            return str
                .Replace("\r\n", "")
                .Replace("\n", "")
                .Replace("\r", "")
                .Replace("\t", "")
                .Replace(" ", "")
                .Trim()
                ;
        }

        /// <summary>
        /// 将win相关符号(换行符，tab符)替换为指定符号。
        /// </summary>
        /// <param name="str">原文本</param>
        /// <param name="newValue">替换的新值</param>
        /// <returns></returns>
        public static string Replacebreaks(string str, string newValue)
        {
            return str
                .Replace("\r\n", newValue)
                .Replace("\n", newValue)
                .Replace("\r", newValue)
                .Replace("\t", newValue)
                .Replace(" ", string.Empty)
                .Trim()
                ;
        }

        /// <summary>
        /// 将HTML转换为纯文本。
        /// </summary>
        /// <param name="html">html标签</param>
        /// <returns></returns>
        public static string ConvertHTML2String(string html)
        {
            return Regex.Replace(html, @"(\<[^\>]*\>)|(\n+)|(\s+)|(&nbsp;)", string.Empty);
        }

        /// <summary>
        /// json转义 。
        /// </summary>
        /// <param name="json">要进行xml转义的字符串</param>
        /// <returns></returns>
        public static string JsonTransferred(string json)
        {
            return json
                .Replace("\"", "&quot;")
                .Replace("\r\n", "<br/>")
                .Replace("\n", "<br/>")
            ;
        }

        #endregion

        #region 封装一个重试机制

        /// <summary>
        /// 封装一个重试机制。
        /// </summary>
        /// <param name="retryTimes">重试次数</param>
        /// <param name="sleepTime">下一次重试时暂停时间(毫秒)</param>
        /// <param name="fun">主处理方法</param>
        public static void DoRetry(int retryTimes, int sleepTime, Action fun)
        {
            DoRetry(retryTimes, sleepTime, fun, null, null);
        }

        /// <summary>
        /// 封装一个重试机制。
        /// </summary>
        /// <param name="retryTimes">重试次数</param>
        /// <param name="sleepTime">下一次重试时暂停时间(毫秒)</param>
        /// <param name="fun">主处理方法</param>
        /// <param name="funError">当发生异常时的处理方法</param>
        public static void DoRetry(int retryTimes, int sleepTime, Action fun, Action<Exception> funError)
        {
            DoRetry(retryTimes, sleepTime, fun, funError, null);
        }

        /// <summary>
        /// 封装一个重试机制。
        /// </summary>
        /// <param name="retryTimes">重试次数</param>
        /// <param name="sleepTime">下一次重试时暂停时间(毫秒)</param>
        /// <param name="fun">主处理方法</param>
        /// <param name="funError">当发生异常时的处理方法</param>
        /// <param name="funErrorMaxRetry">重试达到最大值时仍然失败时的处理方法</param>
        public static void DoRetry(int retryTimes, int sleepTime, Action fun, Action<Exception> funError, Action<Exception> funErrorMaxRetry)
        {
            if (null == fun)
                return;

            for (int i = 0; i < retryTimes; ++i)
            {
                try
                {
                    fun();
                    break;
                }
                catch (Exception ex)
                {
                    if (i == retryTimes - 1)
                    {
                        if (null != funErrorMaxRetry)
                            funErrorMaxRetry(ex);
                        else
                            break;
                    }
                    else
                    {
                        if (null != funError)
                            funError(ex);

                        if (sleepTime > 0)
                            System.Threading.Thread.Sleep(sleepTime);
                    }
                }
            }
        }

        #endregion

        #region 封装一个Try、Catch过程

        /// <summary>
        /// 封装一个Try、Catch过程。
        /// </summary>
        /// <param name="fun">在Try块中执行的方法</param>
        public static void DoTryCatch(Action fun)
        {
            DoTryCatch(fun, null, null);
        }


        /// <summary>
        /// 封装一个Try、Catch过程。
        /// </summary>
        /// <param name="fun">在Try块中执行的方法</param>
        /// <param name="funError">在Catch块中执行的方法</param>
        public static void DoTryCatch(Action fun, Action<Exception> funError)
        {
            DoTryCatch(fun, funError, null);
        }

        /// <summary>
        /// 封装一个Try、Catch过程。
        /// </summary>
        /// <param name="fun">在Try块中执行的方法</param>
        /// <param name="funError">在Catch块中执行的方法</param>
        /// <param name="funFinally">在finally块中执行的方法</param>
        public static void DoTryCatch(Action fun, Action<Exception> funError, Action funFinally)
        {
            if (null == fun)
                return;

            try
            {
                fun();
            }
            catch (Exception ex)
            {
                if (null != funError)
                    funError(ex);
            }
            finally
            {
                if (null != funFinally)
                    funFinally();
            }
        }

        #endregion

        #region xml相关操作

        /// <summary>
        /// 从原xml节点克隆一个副本对象。
        /// </summary>
        /// <param name="sourceNode">原XmlNode</param>
        /// <param name="xDoc">新克隆对象所属的XmlDocument</param>
        /// <returns></returns>
        public static XmlNode CloneXmlElement(XmlNode sourceNode, XmlDocument xDoc)
        {
            XmlNode newNode = xDoc.CreateElement(sourceNode.Name);
            _CloneXmlElement(sourceNode, newNode, xDoc);

            return newNode;
        }

        /// <summary>
        /// 根据原xml节点创建副本对象。
        /// </summary>
        /// <param name="sourceNode">原XmlNode</param>
        /// <param name="targetNode">目标XmlNode</param>
        /// <param name="xDoc">目标XmlNode所属的XmlDocument</param>
        /// <returns></returns>
        private static void _CloneXmlElement(XmlNode sourceNode, XmlNode targetNode, XmlDocument xDoc)
        {
            //复制属性。
            foreach (XmlAttribute att in sourceNode.Attributes)
            {
                XmlNode newAtt = xDoc.CreateAttribute(att.Name);
                newAtt.Value = att.Value;
                targetNode.Attributes.SetNamedItem(newAtt);
            }

            foreach (XmlNode childNode in sourceNode.ChildNodes)
            {
                XmlNode newNode = null;

                if (childNode.Name.Trim().Length == 0)
                    continue;

                //如果是注释。
                if (childNode.Name.Trim().IndexOf("#") >= 0)
                {
                    newNode = xDoc.CreateComment(childNode.Value);
                    targetNode.AppendChild(newNode);
                }
                else
                {
                    newNode = xDoc.CreateElement(childNode.Name);
                    targetNode.AppendChild(newNode);

                    _CloneXmlElement(childNode, newNode, xDoc);
                }
            }
        }

        #region 读取XML节点

        /// <summary>
        /// 读取XML节点
        /// </summary>
        /// <param name="xn"></param>
        /// <returns></returns>
        public static string GetNodeText(XmlNode xn, string defaultValue)
        {
            if (null == xn)
                return defaultValue;
            return xn.InnerText.Trim();
        }

        #endregion

        #endregion

        #region 日期时间相关操作

        /// <summary>
        /// 判断是否为今天。
        /// </summary>
        /// <param name="d">指定日期</param>
        /// <returns></returns>
        public static bool IsToday(DateTime d)
        {
            return DateTimeCompare(d, DateTime.Now);
        }

        /// <summary>
        /// 判断是否为昨天。
        /// </summary>
        /// <param name="d">指定日期</param>
        /// <returns></returns>
        public static bool IsYesterday(DateTime d)
        {
            return DateTimeCompare(d, DateTime.Now.AddDays(-1));
        }

        /// <summary>
        /// 判断是否为前天。
        /// </summary>
        /// <param name="d">指定日期</param>
        /// <returns></returns>
        public static bool IsBeforeYesterday(DateTime d)
        {
            return DateTimeCompare(d, DateTime.Now.AddDays(-2));
        }

        /// <summary>
        /// 判断是否为最近三天以内的日子。
        /// </summary>
        /// <param name="d">指定日期</param>
        /// <returns></returns>
        public static bool IsLastThreeDays(DateTime d)
        {
            return d > DateTime.Parse(DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString()).AddDays(-2);
        }

        /// <summary>
        /// 格式化日期格式(返回今天、昨天、前天及其它具体的日期)。
        /// </summary>
        /// <param name="d">日期</param>
        /// <param name="highLight">是否高亮显示</param>
        /// <returns></returns>
        public static string FormatDateTime1(DateTime d, bool highLight)
        {
            if (IsToday(d))
                return "<span style='color:red;'>今天</span>";
            else if (IsYesterday(d))
                return "<span style='color:blue;'>昨天</span>";
            else if (IsBeforeYesterday(d))
                return "<span style='color:green;'>前天</span>";

            return d.ToString("d");
        }

        /// <summary>
        /// 判断两个时间是否为同一天。
        /// </summary>
        /// <param name="d1">时间一</param>
        /// <param name="d2">时间二</param>
        /// <returns></returns>
        public static bool DateTimeCompare(DateTime d1, DateTime d2)
        {
            return d1.Year == d2.Year && d1.Month == d2.Month && d1.Day == d2.Day;
        }

        /// <summary>
        /// 将时间转换为yyyyMMdd格式的字符串。
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToDateString(DateTime date)
        {
            return date.ToString("yyyy-MM-dd").Replace("-", string.Empty);
        }

        /// <summary>
        /// 将日期字符串转换为DateTime。
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime StringToDate(string date)
        {
            Match m = Regex.Match(date, @"(\d{4})(\d{2})(\d{2})");
            if (m.Success)
                return DateTime.Parse(m.Groups[1] + "-" + m.Groups[2] + "-" + m.Groups[3]);

            throw new AppException(date + " 未能识别为有效的日期格式，正确格式为yyyyMMdd。", ExceptionLevels.Warning);
        }

        /// <summary>
        /// 从日期时间字符串创建日期对象。
        /// </summary>
        /// <param name="dt">日期时间字符串</param>
        /// <returns></returns>
        public static DateTime? DateTimeParse(string dt)
        {
            try
            {
                return DateTime.Parse(dt);
            }
            catch { return null; }
        }
        #endregion

        #region 获取用户的真实IP地址

        /// <summary>
        /// 获取用户的真实IP地址(如果使用代理，获取真实IP)。
        /// </summary>
        public static string UserIPAddress
        {
            get
            {
                string ip = null;
                CoreUtil.DoTryCatch(() =>
                {
                    if (null == HttpContext.Current || null == HttpContext.Current.Request)
                        return;

                    HttpRequest Request = HttpContext.Current.Request;
                    // 如果使用代理，获取真实IP。
                    ip = !string.IsNullOrEmpty(Request.ServerVariables["HTTP_X_FORWARDED_FOR"]) ? Request.ServerVariables["REMOTE_ADDR"] : Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (string.IsNullOrEmpty(ip))
                        ip = Request.UserHostAddress;
                    if (ip == "::1")
                        ip = "127.0.0.1";
                });

                return ip;
            }
        }


        /// 获取 当前网站的主域名(带http://)。
        /// </summary>
        public static string SiteHost
        {
            get
            {
                if (null == HttpContext.Current || null == HttpContext.Current.Request)
                    return null;

                return "http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"];
            }
        }


        /// <summary>
        /// 获取本机外网IP。
        /// </summary>
        /// <returns></returns>
        public static string ServerExternalIP
        {
            get
            {
                var str = new WebClient().DownloadString("http://iframe.ip138.com/ic.asp");
                return Regex.Match(str, @"您的IP是\s*：\s*\[(.*?)\]", RegexOptions.IgnoreCase).Groups[1].Value;
            }
        }

        /// <summary>
        /// 获取本机所有IP地址。
        /// </summary>
        /// <returns></returns>
        public static IList<string> GetHostIP()
        {
            var ips = new List<string>();
            foreach (var ip in Dns.GetHostAddresses(Dns.GetHostName()))
                ips.Add(ip.ToString());

            return ips;
        }

        #endregion

        #region 封装公用方法

        /// <summary>
        /// 根据枚举值获取枚举项。
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="v">枚举值</param>
        /// <returns></returns>
        public static T GetItemByValue<T>(int v)
        {
            foreach (string _name in Enum.GetNames(typeof(T)))
            {
                T _em = (T)Enum.Parse(typeof(T), _name);
                if (Convert.ToInt32(_em) == v)
                    return _em;
            }

            return default(T);
        }

        /// <summary>
        /// 判断指定的文件扩展名是否为图片文件。
        /// </summary>
        /// <param name="ex">文件扩展名(已去掉空格和自动转小写)</param>
        /// <returns></returns>
        public static bool IsImageFile(string ex)
        {
            ex = ex.Trim().ToLower();
            return ex == ".gif" || ex == ".jpg" || ex == ".png" || ex == ".bmp";
        }

        /// <summary>
        /// 格式化网址。
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string FormatURL(string url)
        {
            url = url.Trim().ToLower();
            if (url.IndexOf("http://") != 0)
                url = "http://" + url;

            return url;
        }

        /// <summary>
        /// 随机生成指定位数的随机数字。
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string RandomNumeric(int len)
        {
            var ns = string.Empty;
            while (true)
            {
                ns += Math.Abs(Guid.NewGuid().GetHashCode()).ToString();
                if (ns.Length >= len)
                    break;
            }

            return ns.Substring(0, len);
        }

        /// <summary>
        /// 判断当前访问者的浏览器是否为ie6。
        /// </summary>
        /// <returns></returns>
        public static bool IsIE6
        {
            get
            {
                if (HttpContext.Current.Request.Browser.Browser.Trim().ToLower() == "ie" &&
                    HttpContext.Current.Request.Browser.Version.Trim().IndexOf("6") == 0)
                    return true;

                return false;
            }
        }

        /// <summary>
        /// 转义对象属性值的html标签。
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="_this"></param>
        public static void EscapePropertyHTMLTag<T>(T _this)
        {
            //	&lt;	<	小于号 
            //	&gt;	>	大于号 

            try
            {
                foreach (var _p in typeof(T).GetProperties())
                {
                    if (_p.CanWrite)
                    {
                        var v = _p.GetValue(_this, null);
                        if (null != v && v is string)
                        {
                            v = v.ToString().Replace("<", "&lt;").Replace(">", "&gt;");
                            _p.SetValue(_this, v, null);
                        }
                    }
                }
            }
            catch { }
        }

        #endregion

        #region 对字符串进行编码及反转

        /// <summary>
        /// 对字符串进行编码。
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string CharEncode(string text)
        {
            var sb = new StringBuilder();
            foreach (byte num in Encoding.UTF8.GetBytes(text))
                sb.AppendFormat("{0:X2}", num);

            return sb.ToString();
        }

        /// <summary>
        /// 对字符串进行编码反转。
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string CharDecode(string text)
        {
            byte[] buffer = new byte[text.Length / 2];
            for (int i = 0; i < (text.Length / 2); i++)
            {
                var num = Convert.ToInt32(text.Substring(i * 2, 2), 0x10);
                buffer[i] = (byte)num;
            }

            return Encoding.UTF8.GetString(buffer);
        }

        #endregion

        #region 计算文件大写40位SHA1值，用于文件唯一性校验

        /// <summary>  
        /// 计算文件大写40位SHA1值，用于文件唯一性校验。
        /// </summary>  
        /// <param name="fileName">文件绝对路径</param>  
        /// <returns>文件大写SHA1值</returns> 
        public static string GetFileSHA1(string fileName)
        {
            if (!File.Exists(fileName))
                return null;

            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                SHA1CryptoServiceProvider sp = new SHA1CryptoServiceProvider();
                byte[] result = sp.ComputeHash(fs);
                StringBuilder sb = new StringBuilder();
                foreach (byte item in result)
                    sb.AppendFormat("{0:X2}", item);
                return sb.ToString();
            }
        }

        #endregion

        #region 删除文件当失败后重试

        /// <summary>
        /// 删除文件当失败后重试。
        /// </summary>
        /// <param name="filePath">将要删除的文件完整路径</param>
        /// <param name="retryTimes">当删除失败重试次数</param>
        /// <param name="sleepTime">下一次重试时暂停时间(单位：毫秒)</param>
        /// <param name="fun">主处理方法</param>
        public static void DeleteFileWithRetry(string filePath, int retryTimes, int sleepTime)
        {
            DoRetry(retryTimes, sleepTime, () =>
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }, null, null);
        }

        #endregion

        #region 判断是否为本地网络

        /// <summary>
        /// 判断是否为本地网络。
        /// </summary>
        public static bool IsLocalHost
        {
            get
            {
                if (null == HttpContext.Current)
                    return false;

                return HttpContext.Current.Request.Url.ToString().IgnoreCaseIndexOf("localhost") >= 0;
            }
        }

        #endregion

        #region 从0索引处截取指定字符长度(区分全角半角)的字符串

        /// <summary>
        /// 从0索引处截取指定字符长度(区分全角半角)的字符串。
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="charLength">指定的字符半角长度</param>
        /// <returns></returns>
        public static string SubString2(string _this, int charLength)
        {
            if (string.IsNullOrEmpty(_this))
                return _this;

            int charIndex = 0;
            int totCharLength = 0;
            foreach (char _c in _this.ToCharArray())
            {
                totCharLength += _c > 255 ? 2 : 1;
                if (totCharLength >= charLength)
                    break;
                charIndex += 1;
            }
            return _this.Substring(0, charIndex);
        }

        #endregion

        #region 远程请求数据

        #region Http Post请求数据

        /// <summary>
        /// 获取 同一进程共享的Cookie对象。
        /// </summary>
        public static CookieContainer ProcessCookie
        {
            get
            {
                if (null == HttpContext.Current)
                    return new CookieContainer();

                var cookie = HttpContext.Current.Session["ProcessCookie985659855"] as CookieContainer;
                if (null == cookie)
                {
                    cookie = new CookieContainer();
                    HttpContext.Current.Session["ProcessCookie985659855"] = cookie;
                }

                return cookie;
            }
        }

        /// <summary>
        /// Http Post请求数据(默认为UTF-8编码)。
        /// </summary>
        /// <param name="url">对方API网址</param>
        /// <param name="postData">请求数据</param>
        /// <returns></returns>
        public static string HttpPostData(string url, string postData)
        {
            return HttpPostData(url, postData, Encoding.UTF8);
        }

        /// <summary>
        /// Http Post请求数据。
        /// </summary>
        /// <param name="url">对方API网址</param>
        /// <param name="postData">请求数据</param>
        /// <param name="enCoding">字符编码</param>
        /// <returns></returns>
        public static string HttpPostData(string url, string postData, Encoding enCoding)
        {
            return HttpPostData(url, enCoding.GetBytes(postData), enCoding);
        }

        /// <summary>
        /// Http Post请求数据。
        /// </summary>
        /// <param name="url">对方API网址</param>
        /// <param name="postData">请求数据</param>
        /// <param name="enCoding">字符编码</param>
        /// <returns></returns>
        public static string HttpPostData(string url, byte[] buffer, Encoding enCoding)
        {
            var req = (HttpWebRequest)WebRequest.Create(url);
            req.KeepAlive = true;
            req.Method = "POST";
            req.AllowAutoRedirect = true;
            //req.CookieContainer = ProcessCookie;
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

        /// <summary>
        /// 通过POST方式发送数据
        /// </summary>
        /// <param name="Url">url</param>
        /// <param name="postDataStr">Post数据</param>
        /// <param name="cookie">Cookie容器</param>
        /// <returns></returns>
        public static string SendDataByPost(string Url, string postDataStr)
        {
            byte[] data = System.Text.Encoding.GetEncoding("utf-8").GetBytes(postDataStr);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            Stream myRequestStream = request.GetRequestStream();
            myRequestStream.Write(data, 0, data.Length);
            myRequestStream.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader myStreamReader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            return retString;
        }

        #endregion

        #region Http Get请求数据

        /// <summary>
        /// Http Get请求数据(默认为UTF-8编码)。
        /// </summary>
        /// <param name="url">请求网址</param>
        /// <param name="queryString">请求参数(不要带?号)</param>
        public static string HttpGetData(string url, string queryString)
        {
            return HttpGetData(url, queryString, Encoding.UTF8);
        }

        /// <summary>
        /// Http Get请求数据。
        /// </summary>
        /// <param name="url">请求网址</param>
        /// <param name="queryString">请求参数(不要带?号)</param>
        /// <param name="enCoding">字符编码</param>
        public static string HttpGetData(string url, string queryString, Encoding enCoding)
        {
            if (!string.IsNullOrEmpty(queryString))
                url += "?" + queryString;

            var req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "GET";
            req.ContentType = "text/html;charset=UTF-8";
            req.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; InfoPath.2; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; .NET4.0C; .NET4.0E)";
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";

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

        #endregion

        #region 生成带掩码的字符串

        /// <summary>
        /// 生成带掩码的电子邮箱字符串(比如将13823698955@qq.com生成138****8955@qq.com)。
        /// </summary>
        /// <param name="email">电子邮箱原字符串</param>
        /// <param name="a">前面留的位数</param>
        /// <param name="b">后面留的位数</param>
        /// <returns></returns>
        public static string GetMaskText_Email(string email, int a, int b)
        {
            var i = email.LastIndexOf("@");
            return GetMaskText(email.Substring(0, i), a, b) + "@" + email.Substring(i + 1);
        }

        /// <summary>
        /// 生成带掩码的字符串(比如将13823698955生成138****8955)。
        /// </summary>
        /// <param name="text">原字符串</param>
        /// <param name="a">前面留的位数</param>
        /// <param name="b">后面留的位数</param>
        /// <returns></returns>
        public static string GetMaskText(string text, int a, int b)
        {
            if (text.Length < a + b + 1)
                return text;

            var maskText = text.Substring(0, a);
            maskText += new string('*', text.Length - a - b);
            maskText += text.Substring(text.Length - b);

            return maskText;
        }

        #endregion

        #region 生成MD5码

        /// <summary>
        /// 生成MD5码(gb2312)
        /// </summary>
        /// <param name="original">待生成字符串</param>
        /// <returns></returns>
        public static string MakeMD5_GB2312(string original)
        {
            return MakeMD5(original, Encoding.GetEncoding("gb2312"));
        }

        /// <summary>
        /// 生成MD5码(UTF-8)
        /// </summary>
        /// <param name="original">待生成字符串</param>
        /// <returns></returns>
        public static string MakeMD5_UTF8(string original)
        {
            return MakeMD5(original, Encoding.UTF8);
        }

        /// <summary>
        /// 生成MD5码(使用系统默认编码)。
        /// </summary>
        /// <param name="original">待生成字符串</param>
        /// <returns></returns>
        public static string MakeMD5(string original)
        {
            return MakeMD5(original, Encoding.Default);
        }

        /// <summary>
        /// 生成MD5码。
        /// </summary>
        /// <param name="original">待生成字符串</param>
        /// <param name="encoding">编码格式</param>
        /// <returns></returns>
        public static string MakeMD5(string original, Encoding encoding)
        {
            var hashmd5 = new MD5CryptoServiceProvider();
            byte[] byteOriginal = hashmd5.ComputeHash(encoding.GetBytes(original));
            StringBuilder ciphertext = new StringBuilder(32);
            for (int i = 0; i < byteOriginal.Length; i++)
            {
                ciphertext.Append(byteOriginal[i].ToString("x").PadLeft(2, '0'));
            }

            return ciphertext.ToString();
        }

        #endregion

        #region 生成订单号

        /// <summary>
        /// 生成订单号(前缀+yyyyMMddHHmmss+N位随机数字)。
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="numRandomNumeric"></param>
        /// <returns></returns>
        public static string GenOrderNo(string prefix, int numRandomNumeric)
        {
            return prefix.ToUpper() + DateTime.Now.ToString("yyMMddHHmmssfff") + RandomNumeric(numRandomNumeric);
        }

        #endregion

        #region 获取用户的真实IP地址

        /// <summary>
        /// 获取用户的真实IP地址(如果使用代理，获取真实IP)。
        /// </summary>
        public static string IPAddress
        {
            get
            {
                if (null == HttpContext.Current || null == HttpContext.Current.Request)
                    return string.Empty;

                HttpRequest Request = HttpContext.Current.Request;
                // 如果使用代理，获取真实IP。
                string userIP = string.IsNullOrEmpty(Request.ServerVariables["HTTP_X_FORWARDED_FOR"]) ? Request.ServerVariables["REMOTE_ADDR"] : Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(userIP))
                    userIP = Request.UserHostAddress;
                if (userIP == "::1")
                    userIP = "127.0.0.1";
                return userIP;
            }
        }

        public static string HostIPAddress
        {
            get
            {

                return HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"] == "::1" ? "127.0.0.1" : HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"];
                //eturn HttpContext.Current.Request.UserHostAddress;
            }
        }

        /// <summary>
        /// 获取 当前网站的主域名(带http://，不包含端口号)
        /// </summary>
        public static string HttpHost
        {
            get
            {
                if (null == HttpContext.Current || null == HttpContext.Current.Request)
                    AppException.ThrowWaringException("无法在异步线程中获取HttpContext信息！");

                return "http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"];

            }
        }

        /// <summary>
        /// 获取 当前网站的主域名及端口(不带http://，如localhost:345 或 www.abc.com:3456)
        /// </summary>
        public static string HostPort
        {
            get
            {
                if (null == HttpContext.Current || null == HttpContext.Current.Request)
                    AppException.ThrowWaringException("无法在异步线程中获取HttpContext信息！");

                var domain = HttpContext.Current.Request.Url.Host;
                var port = HttpContext.Current.Request.Url.Port;
                if (port != 80)
                {
                    domain += ":" + port.ToString();
                }

                return domain;
            }
        }


        #endregion

        #region 将json格式字符串转换为字典集合

        /// <summary>
        /// 将json格式字符串转换为字典集合。
        /// </summary>
        /// <param name="json">json格式数据</param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> SeralizeListDictionary(string jsonText)
        {
            return new JsonSeralize.Seralize().GetList(jsonText);
        }

        /// <summary>
        /// 将json格式字符串转换为字典集合。
        /// </summary>
        /// <param name="json">json格式数据</param>
        /// <returns></returns>
        public static Dictionary<string, object> SeralizeDictionary(string jsonText)
        {
            return new JsonSeralize.Seralize().GetDic(jsonText);
        }
        /// <summary>
        /// 将json格式字符串转换为字典集合数组。
        /// </summary>
        /// <param name="json">json格式数据</param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> SeralizeList(string jsonText)
        {
            return new JsonSeralize.Seralize().GetList(jsonText);
        }
        /// <summary>
        /// 将对象转换JSON
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string SeralizeJson(object obj)
        {
            return new JsonSeralize.Seralize().JsSeralize(obj);
        }

        #endregion

        #region 将指定秒数转换为分钟秒数

        /// <summary>
        /// 将指定秒数转换为分钟秒数。
        /// </summary>
        /// <param name="totalSecond">总秒数</param>
        /// <returns></returns>
        public static string ChangeToMinute(double totalSecond)
        {
            var n = (int)(totalSecond / 60);
            return n.ToString("00") + "分" + (totalSecond - n * 60).ToString("00") + "秒";
        }

        #endregion

        #region 数字月转英文简码

        /// <summary>
        /// 12月英文简码转换函数，数字月转英文简码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FormatMonthEn(string value)
        {
            string strMonth = "";
            switch (value)
            {
                case "01":
                    strMonth = "JAN";
                    break;
                case "02":
                    strMonth = "FEB";
                    break;
                case "03":
                    strMonth = "MAR";
                    break;
                case "04":
                    strMonth = "APR";
                    break;
                case "05":
                    strMonth = "MAY";
                    break;
                case "06":
                    strMonth = "JUN";
                    break;
                case "07":
                    strMonth = "JUL";
                    break;
                case "08":
                    strMonth = "AUG";
                    break;
                case "09":
                    strMonth = "SEP";
                    break;
                case "10":
                    strMonth = "OCT";
                    break;
                case "11":
                    strMonth = "NOV";
                    break;
                case "12":
                    strMonth = "DEC";
                    break;
                default:
                    break;
            }
            return strMonth;
        }

        #endregion

        #region 删除按本系统指定规则的图片

        /// <summary>
        /// 删除按本系统指定规则的图片。
        /// </summary>
        /// <param name="pictureFolder">图片所在目录</param>
        /// <param name="pictureName">图片名称</param>
        public static void DeleteTripSystemPicture(string pictureFolder, string pictureName)
        {
            //删除原图。
            CoreUtil.DeleteFileWithRetry(pictureFolder + "/" + pictureName, 3, 300);

            //删除相关缩略图。
            if (Directory.Exists(pictureFolder + "/thumbs"))
            {
                foreach (var _fi in new DirectoryInfo(pictureFolder + "/thumbs").GetFiles())
                {
                    //删除名称匹配 或 去除1位前缀后匹配。
                    if (_fi.Name.IgnoreCaseEquals(pictureName) || _fi.Name.Substring(1).IgnoreCaseEquals(pictureName))
                        CoreUtil.DeleteFileWithRetry(_fi.FullName, 3, 300);
                }
            }
        }

        #endregion

        #region 传入一个长地址生成短地址

        /// <summary>
        /// 传入一个长地址生成短地址
        /// </summary>
        /// <param name="longUrl">长地址</param>
        /// <returns></returns>
        public static string CreateShortUrl(string longUrl)
        {
            try
            {
                var url = "http://api.t.sina.com.cn/short_url/shorten.json";
                var par = "source=1681459862&url_long=" + longUrl;
                var json = SendDataByPost(url, par);
                var listSeralize = SeralizeListDictionary(json);
                if (listSeralize.Count < 1)
                    return "";
                var dicSeralize = (Dictionary<string, object>)listSeralize[0];
                return dicSeralize["url_short"].ToString();
            }
            catch
            {
                return longUrl;
            }
        }

        #endregion        

        #region 将Html代码转为纯文本

        /// <summary>
        /// 将Html代码转为纯文本。
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string StripHTML(string html)
        {
            if (string.IsNullOrEmpty(html))
                return html;

            try
            {
                string result;

                // Remove HTML Development formatting
                // Replace line breaks with space
                // because browsers inserts space
                result = html.Replace("\r", " ");
                // Replace line breaks with space
                // because browsers inserts space
                result = result.Replace("\n", " ");
                // Remove step-formatting
                result = result.Replace("\t", string.Empty);
                // Remove repeating spaces because browsers ignore them
                result = System.Text.RegularExpressions.Regex.Replace(result,
                                                                      @"( )+", " ");

                // Remove the header (prepare first by clearing attributes)
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*head([^>])*>", "<head>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"(<( )*(/)( )*head( )*>)", "</head>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(<head>).*(</head>)", string.Empty,
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // remove all scripts (prepare first by clearing attributes)
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*script([^>])*>", "<script>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"(<( )*(/)( )*script( )*>)", "</script>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                //result = System.Text.RegularExpressions.Regex.Replace(result,
                //         @"(<script>)([^(<script>\.</script>)])*(</script>)",
                //         string.Empty,
                //         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"(<script>).*(</script>)", string.Empty,
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // remove all styles (prepare first by clearing attributes)
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*style([^>])*>", "<style>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"(<( )*(/)( )*style( )*>)", "</style>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(<style>).*(</style>)", string.Empty,
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // insert tabs in spaces of <td> tags
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*td([^>])*>", "\t",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // insert line breaks in places of <BR> and <LI> tags
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*br( )*>", "\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*li( )*>", "\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // insert line paragraphs (double line breaks) in place
                // if <P>, <DIV> and <TR> tags
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*div([^>])*>", "\r\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*tr([^>])*>", "\r\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*p([^>])*>", "\r\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // Remove remaining tags like <a>, links, images,
                // comments etc - anything that's enclosed inside < >
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<[^>]*>", string.Empty,
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // replace special characters:
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @" ", " ",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"•", " * ",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"‹", "<",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"›", ">",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"™", "(tm)",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"⁄", "/",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<", "<",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @">", ">",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"©", "(c)",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"®", "(r)",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                // Remove all others. More can be added, see
                // http://hotwired.lycos.com/webmonkey/reference/special_characters/
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&(.{2,6});", string.Empty,
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // for testing
                //System.Text.RegularExpressions.Regex.Replace(result,
                //       this.txtRegex.Text,string.Empty,
                //       System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // make line breaking consistent
                result = result.Replace("\n", "\r");

                // Remove extra line breaks and tabs:
                // replace over 2 breaks with 2 and over 4 tabs with 4.
                // Prepare first to remove any whitespaces in between
                // the escaped characters and remove redundant tabs in between line breaks
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\r)( )+(\r)", "\r\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\t)( )+(\t)", "\t\t",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\t)( )+(\r)", "\t\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\r)( )+(\t)", "\r\t",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                // Remove redundant tabs
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\r)(\t)+(\r)", "\r\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                // Remove multiple tabs following a line break with just one tab
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\r)(\t)+", "\r\t",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                // Initial replacement target string for line breaks
                string breaks = "\r\r\r";
                // Initial replacement target string for tabs
                string tabs = "\t\t\t\t\t";
                for (int index = 0; index < result.Length; index++)
                {
                    result = result.Replace(breaks, "\r\r");
                    result = result.Replace(tabs, "\t\t\t\t");
                    breaks = breaks + "\r";
                    tabs = tabs + "\t";
                }

                // That's it.
                return result.Trim();
            }
            catch
            {
                return html;
            }
        }

        #endregion

        #region 生成重复字符串

        /// <summary>
        /// 生成重复字符串。
        /// </summary>
        /// <param name="text">需要重复生成的文本</param>
        /// <param name="count">生成次数</param>
        /// <returns></returns>
        public static string RenderRepeatText(string text, int count)
        {
            var t = string.Empty;
            for (int i = 0; i < count; i++)
                t += text;

            return t;
        }

        #endregion

        #region 生成错误返回JSON

        /// <summary>
        /// 生成错误返回JSON。
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string MakeErrorJsonData(string code, string message)
        {
            return "{\"r\":\"" + code + "\",\"m\":\"" + message + "\"}";
        }

        #endregion

        #region 格式化成为一个json键值串

        /// <summary>
        /// 格式化成为一个json键值串
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="isDecimal">是否是数字</param>
        /// <param name="trimLast">是否去除最后一个,</param>
        /// <returns></returns>
        public static string JsonFormat(string key, string value, bool isDecimal = false, bool trimLast = false)
        {
            var ret = string.Empty;
            if (isDecimal)
            {
                ret = "\"" + key + "\":" + value + ",";
            }
            else
            {
                ret = "\"" + key + "\":\"" + value + "\",";
            }
            if (trimLast)
                ret = ret.TrimEnd(',');
            return ret;
        }

        #endregion

        #region 汉字转换为Unicode编码

        /// <summary>
        /// 汉字转换为Unicode编码
        /// </summary>
        /// <param name="str">要编码的汉字字符串</param>
        /// <returns>Unicode编码的的字符串</returns>
        public static string ToUnicode(string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            byte[] bts = Encoding.Unicode.GetBytes(str);
            string r = "";
            for (int i = 0; i < bts.Length; i += 2) r += "\\u" + bts[i + 1].ToString("x").PadLeft(2, '0') + bts[i].ToString("x").PadLeft(2, '0');
            return r;
        }

        #endregion

        #region GET POST 请求的参数

        public static string Request(string key)
        {
            if (HttpContext.Current.Request[key] != null)
            {
                return HttpContext.Current.Request[key].ToString().Trim();
            }
            else
            {
                return "";
            }
        }


        /// <summary>
        /// 用于一般处理文件中
        /// </summary>
        /// <param name="context"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public static string Request(HttpContext context, string para)
        {
            if (context.Request[para] != null)
            {
                return context.Request[para].ToString().Trim();
            }
            else
                return "";
        }

        #endregion

        #region 格式化银行卡号

        /// <summary>
        /// 格式化银行卡号。
        /// </summary>
        /// <param name="cardNo">银行卡号</param>
        /// <returns></returns>
        public static string FormatBankCardNo(string cardNo)
        {
            return Regex.Replace(cardNo, @"(\d{4})", "$0 ");
        }

        #endregion

        #region 判断是否为手机浏览器

        /// <summary>
        /// 判断是否为手机浏览器。
        /// </summary>
        public static bool IsPhoneBrowser
        {
            get
            {
                var ua = System.Web.HttpContext.Current.Request.UserAgent;
                if (
                    ua.IgnoreCaseIndexOf("iPhone") >= 0 ||
                    ua.IgnoreCaseIndexOf("Android") >= 0 ||
                    ua.IgnoreCaseIndexOf("Windows Phone") >= 0
                )
                    return true;

                return false;
            }
        }

        #endregion

        #region 获取排序后的查询参数

        /// <summary>
        /// 获取排序后的查询参数。
        /// </summary>
        /// <param name="url">网址</param>
        /// <returns></returns>
        public static List<string> QueryString(string url)
        {
            //获取查询参数部分。
            url = url.Trim().ToLower();
            var index = url.IndexOf("?");
            if (index < 0)
                return new List<string>();

            //将查询参数转为集合。
            var qsList = url.Substring(index + 1).Split("&").ToList();

            //移除分页参数。
            for (int i = qsList.Count - 1; i >= 0; i--)
            {
                if (qsList[i].IndexOf("page=") == 0)
                {
                    qsList.RemoveAt(i);
                    break;
                }
            }

            //返回查询参数。
            return qsList;
        }

        #endregion

        #region 判断字符串是否存在指定枚举

        /// <summary>
        /// 判断字符串是否存在指定枚举。
        /// </summary>
        /// <param name="type">枚举类型</param>
        /// <param name="str">需要判断的字符串</param>
        public static void TestEnum(Type type, string str)
        {
            var f = false;
            foreach (var _name in Enum.GetNames(type))
            {
                if (_name.IgnoreCaseEquals(str))
                {
                    f = true;
                    break;
                }
            }

            if (!f)
                AppException.ThrowWaringException("未能在枚举类型“" + type + "”中找到子顶“" + str + "”！");
        }

        #endregion

        #region 判断是否是邮箱

        /// <summary>
        /// 判断是否是邮箱。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsEmail(string str)
        {
            string expression = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            return Regex.IsMatch(str, expression, RegexOptions.IgnoreCase);
        }

        #endregion

        #region 判断是否是手机号码

        public static bool IsMobilePhone(string input)
        {
            Regex regex = new Regex("^1\\d{10}$");
            return regex.IsMatch(input);
        }

        #endregion
    }
}
