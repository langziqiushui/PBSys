using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using YX.Web.Framework;
using YX.Core;

namespace YX.Web.Repair
{
    /// <summary>
    /// 错误显示页面。
    /// </summary>
    public partial class Error : BaseRepair
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.div404.Visible = false;
            this.divOtherError.Visible = false;

            if (Request.RawUrl.IgnoreCaseIndexOf("404") >= 0)
            {
                this.div404.Visible = true;
                this.Title = "抱歉，我们找不到您请求的页面！";
            }
            else
            {
                this.divOtherError.Visible = true;

                //如果是其它异常。
                string key = WebKeys.APPLICATION_LAST_ERROR + HttpContext.Current.Session.SessionID;
                HttpContext.Current.Application.Lock();
                var ex = HttpContext.Current.Application[key];
                HttpContext.Current.Application.Remove(key);
                HttpContext.Current.Application.UnLock();
                if (null == ex)
                {
                    //Response.Redirect("~/");
                    return;
                }

                var ex2 = ex as Exception;
                this.Title = "对不起，您访问的页面发生异常-" + ConfigAppSettings.WebSiteName;
                var message = Request.Headers["Host"].IgnoreCaseIndexOf("localhost") >= 0 ? ex2.ToString() : ex2.Message;
                this.divErrorMsg.InnerHtml = CoreUtil.ConvertWinTagToWeb(message);
            }
        }

    }
}