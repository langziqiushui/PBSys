using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using YX.Web.Framework;

namespace YX.Web
{
    /// <summary>
    /// 默认首页。
    /// </summary>
    public partial class Default : BaseWeb
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
                Response.Redirect("/Admin/Index");
            else
                Response.Redirect("/Login");
        }
    }
}