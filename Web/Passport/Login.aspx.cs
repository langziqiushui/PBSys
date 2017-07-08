using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using YX.Web.Framework;
using YX.Core;
using YX.Factory;
using YX.Domain;

namespace YX.Web.Passport
{
    /// <summary>
    /// 用户登录。
    /// </summary>
    public partial class Login : BaseLogin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                
                //浏览器判断。
                CoreUtil.DoTryCatch(() =>
                {
                    if (Request.Browser.Type.IgnoreCaseIndexOf("IE") >= 0)
                    {
                        var version = int.Parse(System.Text.RegularExpressions.Regex.Match(Request.Browser.Type, @"\d+").Value);
                        if (version < 11)
                            Response.Redirect("/Repair/BrowserUnSupport.htm");
                        return;
                    }
                });                              

                //载入以前的登录名。
                var loginUser = CookieManager.GetCookie(WebKeys.COOKIE_LOGIN_USERID);
                if (!string.IsNullOrEmpty(loginUser))
                    this.txtUserName.Text = loginUser;

                this.btLogin.Attributes.Add(WebKeys.RequestText, "登录中..");
            }
            //引用css，JS。
            this.RefJavascript(false, "myTable.js");
            this.RefStyle(false, "myTable.css");

            this.btLogin.Click += new EventHandler(btLogin_Click);
            WFUtil.RegisterStartupScript("login.init();");
        }

        #region 用户登录

        /// <summary>
        /// 用户登录。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btLogin_Click(object sender, EventArgs e)
        {
            this.divErrorMsg.InnerHtml = null;

            WFUtil.UIDoTryCatch("会员登录", () =>
            {
                if (!Page.IsValid)
                    return; 
               
                //准备登录。
                this.BeginLogin(this.txtUserName.Text.Trim(),this.txtPassword.Text);

            }, false, (Exception ex) =>
            {
                this.txtPassword.Text = null;
                WFUtil.RegisterStartupScript("$2('txtPassword').focus();");
                this.ShowMessage(ex.Message);
            }, () =>
            {
                WFUtil.RegisterStartupScript("$2('txtPassword').value='" + this.txtPassword.Text + "';");
            });
        }

        /// <summary>
        /// 用户登录。
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        private void BeginLogin(string userName, string password)
        {
            var entityUser = ServicesFactory.CreateGloAdminuserServices().Login(userName, password);
            if (null != entityUser)
            {
                //设置登录状态。
                SecurityUtils.SimpleSignIn(entityUser.UserName, false, true);
                //保存主账户和子账户用户名。
                CookieManager.WriteCookie(WebKeys.COOKIE_LOGIN_USERID, this.txtUserName.Text.Trim(), DateTime.Now.AddDays(7));
                //登录成功后自动跳转。
                this.Redirect();
            } 
        }

        /// <summary>
        /// 显示提示消息。
        /// </summary>
        /// <param name="message"></param>
        private void ShowMessage(string message)
        {
            this.divErrorMsg.InnerHtml = message;
            this.divErrorMsg.Visible = true;
        }

        /// <summary>
        /// 登录成功后自动跳转。
        /// </summary>
        private void Redirect()
        {
            var url = "";
            if (!string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
            {
                url = Request.QueryString["ReturnUrl"];
            }
            else if (null != Request.UrlReferrer && Request.UrlReferrer.ToString().IgnoreCaseIndexOf("login") < 0)
            {
                url = Request.UrlReferrer.ToString();
            }
            else
            {
                url = "~/Admin/index";
            }

            Response.Redirect(url);
        }

        #endregion

    }
}