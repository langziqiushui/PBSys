using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Routing;
using System.IO;

using YX.Core;
using YX.Web.Framework;
using YX.Factory;

namespace YX.Web
{
    public class Global : System.Web.HttpApplication
    {
        #region 应用程序启动

        protected void Application_Start(object sender, EventArgs e)
        {
            //注册路由。
            RegisterRoutes(RouteTable.Routes);
           
            //开启定时任务管理器。
            TaskManager.Ins.Start();
        }

        protected void Application_End(object sender, EventArgs e)
        {           
            //停止定时任务管理器。
            TaskManager.Ins.Stop();
        }

        #endregion

        #region 应用程序相关事件


        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        #endregion

        #region 注册URL路由

        void RegisterRoutes(RouteCollection routes)
        {
            /*
            Page.RouteData.Values。
            routes.MapPageRoute("rp", "rp_{id}", "~/default.aspx", false,
                null, //此处设置默认值
                new RouteValueDictionary { { "id", @"\d+" } } //设置路由参数规则
                );
            */
            //---------------------------- 全局 ---------------------------------//
            #region 基本

            routes.MapPageRoute("error", "error", "~/Repair/error.aspx");
            routes.MapPageRoute("error-404", "error-404", "~/Repair/error.aspx");
            routes.MapPageRoute("login", "login", "~/Passport/login.aspx");
            routes.MapPageRoute("signout", "signout", "~/Passport/signout.aspx");

            #endregion           

            #region 系统管理

            routes.MapPageRoute("LogList", "Admin/Glo/LogList", "~/Admin/Glo/LogList.aspx");
            routes.MapPageRoute("DBConfig", "Admin/Glo/DBConfig", "~/Admin/Glo/DBConfig.aspx");
            routes.MapPageRoute("PBConfigList", "Admin/Glo/PBConfigList", "~/Admin/Glo/PBConfigList.aspx");
            routes.MapPageRoute("OfficialAccount", "OfficialAccount/OfficialAccountList", "~/OfficialAccount/OfficialAccountList.aspx");

            //后台主页
            routes.MapPageRoute("AdminIndex", "Admin/Index", "~/Admin/Index.aspx");
            #endregion

        }

        #endregion
    }
}