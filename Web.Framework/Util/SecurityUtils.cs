using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

using YX.Core;
using YX.Factory;

namespace YX.Web.Framework
{
    /// <summary>
    /// 系统安全相关封装。
    /// </summary>
    public static class SecurityUtils
    {
        #region 变量

        #endregion        

        #region 单点登录

        /// <summary>
        /// 创建登录票据。
        /// </summary>
        /// <param name="userID">用户名</param>
        /// <param name="isAutoLogin">是否创建持久性用户自动登录Cookie(一周内自动登录)</param>
        /// <param name="isSaveUserID">是否保存用户名(一周内记住用户名)</param>
        /// <param name="userData">用户数据</param>
        public static void SSOSignIn(string userID, bool isAutoLogin, bool isSaveUserID, string userData)
        {
            //先退出。
            SSOSignOut();

            //设置Cookie过期日期。            
            DateTime expirationTime = isAutoLogin ? DateTime.Now.AddDays(7) : DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes);
            //设置身份状态。
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, userID, DateTime.Now, expirationTime, isAutoLogin, userData);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            cookie.Expires = expirationTime;

            if (isAutoLogin)
            {
                //如果要求创建持久自动登录Cookie，则创建持久性自动登录的Cookie。
                //指定 Cookie 为 Web.config 中 <forms path="/" … /> path 属性，不指定则默认为“/” 。
                //此句非常重要，少了的话，就算此 Cookie 在身份验票中指定为持久性 Cookie ，也只是即时型的 Cookie 关闭浏览器后就失效。
                cookie.Path = FormsAuthentication.FormsCookiePath;
                //保存用户名。
                CookieManager.WriteCookie(WebKeys.COOKIE_LOGIN_PERSISTENT, userID, expirationTime);
            }
            else
            {
                //如果不创建持久自动登录Cookie，则删除持久性自动登录的Cookie。
                CookieManager.RemoveCookie(WebKeys.COOKIE_LOGIN_PERSISTENT);
            }

            if (isSaveUserID)
            {
                //如果要求记住用户名，则创建持久性保存用户名的Cookie。
                CookieManager.WriteCookie(WebKeys.COOKIE_LOGIN_USERID, userID, DateTime.Now.AddDays(7));
            }
            else
            {
                //如果不保存用户名，则删除持久性保存用户名的Cookie。
                CookieManager.RemoveCookie(WebKeys.COOKIE_LOGIN_USERID);
            }

            //如果非本地运行则设置cookie域以便实现多站单点登录。
            if (!CoreUtil.IsLocalHost)
                cookie.Domain = FormsAuthentication.FormsCookieName;

            //保存cookie。
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 单点登录退出系统。
        /// </summary>
        public static void SSOSignOut()
        {
            //移除验证票。
            FormsAuthentication.SignOut();
            //删除自定义cookie。
            CookieManager.RemoveCookie(WebKeys.COOKIE_LOGIN_PERSISTENT);
            CookieManager.RemoveCookie(WebKeys.COOKIE_LOGIN_USERID);
            //删除票据cookie。
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName);
            if (null != cookie)
            {
                if (!CoreUtil.IsLocalHost)
                    cookie.Domain = FormsAuthentication.FormsCookieName;
                cookie.Expires = System.DateTime.Now.AddDays(-10);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }

            HttpContext.Current.Session.Abandon();
        }

        #endregion

        #region 普通登录

        /// <summary>
        /// 普通登录。
        /// </summary>
        /// <param name="userID">用户名</param>
        /// <param name="isAutoLogin">是否创建持久性用户自动登录Cookie(一周内自动登录)</param>
        /// <param name="isSaveUserID">是否保存用户名(一周内记住用户名)</param>
        /// <param name="userData">用户数据</param>
        public static void SignIn(string userID, bool isAutoLogin, bool isSaveUserID, string userData)
        {
            //先退出。
            SignOut();

            //设置Cookie过期日期。            
            DateTime expirationTime = isAutoLogin ? DateTime.Now.AddDays(7) : DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes);
            //设置身份状态。
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, userID, DateTime.Now, expirationTime, isAutoLogin, userData);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            cookie.Expires = expirationTime;

            if (isAutoLogin)
            {
                //如果要求创建持久自动登录Cookie，则创建持久性自动登录的Cookie。
                //指定 Cookie 为 Web.config 中 <forms path="/" … /> path 属性，不指定则默认为“/” 。
                //此句非常重要，少了的话，就算此 Cookie 在身份验票中指定为持久性 Cookie ，也只是即时型的 Cookie 关闭浏览器后就失效。
                cookie.Path = FormsAuthentication.FormsCookiePath;
                //保存用户名。
                CookieManager.WriteCookie(WebKeys.COOKIE_LOGIN_PERSISTENT, userID, expirationTime);
            }
            else
            {
                //如果不创建持久自动登录Cookie，则删除持久性自动登录的Cookie。
                CookieManager.RemoveCookie(WebKeys.COOKIE_LOGIN_PERSISTENT);
            }

            if (isSaveUserID)
            {
                //如果要求记住用户名，则创建持久性保存用户名的Cookie。
                CookieManager.WriteCookie(WebKeys.COOKIE_LOGIN_USERID, userID, DateTime.Now.AddDays(7));
            }
            else
            {
                //如果不保存用户名，则删除持久性保存用户名的Cookie。
                CookieManager.RemoveCookie(WebKeys.COOKIE_LOGIN_USERID);
            }

            //保存cookie。
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 普通登录退出系统。
        /// </summary>
        public static void SignOut()
        {
            //移除验证票。
            FormsAuthentication.SignOut();

            //删除自定义cookie。
            CookieManager.RemoveCookie(WebKeys.COOKIE_LOGIN_PERSISTENT);
            //CookieManager.RemoveCookie(COOKIE_LOGIN_USERID);

            //删除票据cookie。
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName);
            if (null != cookie)
            {
                cookie.Expires = System.DateTime.Now.AddDays(-10);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }

            HttpContext.Current.Session.Abandon();
        }

        #endregion

        #region 简单登录

        /// <summary>
        /// 简单登录。
        /// </summary>
        /// <param name="userID">用户名</param>
        /// <param name="isAutoLogin">是否自动登录(一周）</param>
        /// <param name="isSaveUserID">是否保存用户名(一周)</param>
        public static void SimpleSignIn(string userID, bool isAutoLogin, bool isSaveUserID)
        {
            //先退出。
            SimpleSignOut();
           
            if (isAutoLogin)
            {
                CookieManager.WriteCookie(WebKeys.COOKIE_LOGIN_PERSISTENT, userID, DateTime.Now.AddDays(7));
            }
            else
            {
                //如果不创建持久自动登录Cookie，则删除持久性自动登录的Cookie。
                CookieManager.RemoveCookie(WebKeys.COOKIE_LOGIN_PERSISTENT);
            }

            if (isSaveUserID)
            {
                //如果要求记住用户名，则创建持久性保存用户名的Cookie。
                CookieManager.WriteCookie(WebKeys.COOKIE_LOGIN_USERID, userID, DateTime.Now.AddDays(7));
            }
            else
            {
                //如果不保存用户名，则删除持久性保存用户名的Cookie。
                CookieManager.RemoveCookie(WebKeys.COOKIE_LOGIN_USERID);
            }

            SimpleSignIn(userID);
        } 
    
        /// <summary>
        /// 简单登录。
        /// </summary>
        /// <param name="userID">用户名</param>
        public static void SimpleSignIn(string userID)
        {
            System.Web.HttpContext.Current.Session[Factory.GlobalKeys.SESSION_LOGIN] = userID;
            FormsAuthentication.SetAuthCookie(userID, false);
        }

        /// <summary>
        /// 退出系统。
        /// </summary>
        public static void SimpleSignOut()
        {
            //移除验证票。
            System.Web.HttpContext.Current.Session[Factory.GlobalKeys.SESSION_LOGIN] = null;
            FormsAuthentication.SignOut();         
            //HttpContext.Current.Session.Abandon();
        }

        #endregion

        #region 获取用户验证票据

        /// <summary>
        /// 获取用户验证票据。
        /// </summary>
        /// <returns></returns>
        public static FormsAuthenticationTicket GetAuthTicket()
        {
            var cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (null == cookie || string.IsNullOrEmpty(cookie.Value))
                return null;

            return FormsAuthentication.Decrypt(cookie.Value);
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 创建Session键。
        /// </summary>
        /// <param name="userID">用户名</param>
        /// <returns></returns>
        private static string CreateSessionKey(string userID)
        {
            return WebKeys.SESSION_PRINCIPAL + userID.ToUpper();
        }

        #endregion
    }
}
