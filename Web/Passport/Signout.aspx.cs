using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace YX.Web.Passport
{
    /// <summary>
    /// 退出登录。
    /// </summary>
    public partial class Signout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //退出登录。
            YX.Web.Framework.SecurityUtils.SimpleSignOut();
            //返回登录页面。
            Response.Redirect("/Login");
        }
    }
}