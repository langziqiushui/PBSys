using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using YX.Core;
using YX.Domain;
using System.Web;

namespace YX.Web.Framework
{
    /// <summary>
    /// 系统管理基类。
    /// </summary>
    public class BaseAdmin : BaseWeb
    {
        /// <summary>
        /// 页面载入处理。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //强制用户登录。
            this.AutoLogin();

        }

        /// <summary>
        /// 注册页面基础对象。
        /// </summary>
        protected override void RegPageObjects()
        {
            base.RegPageObjects();

            this.RefJavascript(false, "Modules/Admin.js");
        }

        /// <summary>
        /// 获取或设置 当前页面对应的菜单标识(Admin.xml中的MenuTag)。
        /// </summary>
        public string MenuTag { get; set; }

        #region 强制用户登陆

        /// <summary>
        /// 判断登陆。
        /// </summary>
        protected virtual void AutoLogin()
        {
            //从Sesion中获取。
            var loginUserObj = System.Web.HttpContext.Current.Session[Factory.GlobalKeys.SESSION_LOGIN];
            if (loginUserObj != null)
            {
                //根据帐户获取用户信息。
                var entityUser = YX.Factory.ServicesFactory.CreateGloAdminuserServices().GetData(loginUserObj.ToString());
                if (null == entityUser)
                    AppException.ThrowWaringException("对不起，用户名或密码不正确！");
                 
                //验证用户能否登录。
                if (!entityUser.IsActived)
                    AppException.ThrowWaringException("对不起，此用户已被禁止登录！");
            }
            else
            {
                Response.Redirect("~/login?ReturnUrl=" + HttpUtility.UrlEncode(Request.RawUrl));
            }
        }

        #endregion

    }
}
