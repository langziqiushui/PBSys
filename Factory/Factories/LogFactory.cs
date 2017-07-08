using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using YX.Core;
using YX.Domain;

namespace YX.Factory
{
    /// <summary>
    /// 日志工厂。
    /// </summary>
    public static class LogFactory
    {
        /// <summary>
        /// 日志写入服务对象。
        /// </summary>
        private static YX.IServices.Glo.IDblogServices servicesLog = ServicesFactory.CreateGloDblogServices();

        ////resurl  website logtype exMessage resPlatform reip resbrowser restype
        /// <summary>
        /// 写入日志。
        /// </summary>
        /// <param name="apptype">站点类别</param>       
        /// <param name="logtype">日志类别</param>
        /// <param name="restype">站点类别 M  PC </param>
        /// <param name="reip">IP记录</param>
        /// <param name="resurl">来源页面</param>
        /// <param name="exMessage">日志内容-错误信息</param>
        /// <param name="resPlatform">操作系统</param>
        /// <param name="resbrowser">浏览器 +浏览器版本</param>
        public static void Write(string apptype, string logtype, string restype, string reip, string resurl, string exMessage, string resPlatform, string resbrowser)
        {
            var ip = string.Empty;
            CoreUtil.DoTryCatch(() =>
            {
                ip = CoreUtil.UserIPAddress;
            });

            Action invoke = () =>
            {
                int dbLogID = 0;
                servicesLog.Add(new Domain.Glo.DblogInfo()
                {
                    ApplicationType = Convert.ToByte(apptype),
                    LogType = Convert.ToByte(logtype) ,
                    Restype = Convert.ToByte(restype),
                    ResIP = reip,
                    Resurl = resurl,
                    LogContent = exMessage,
                    ResPlatform = resPlatform,
                    ResBrowser = resbrowser,
                    CreateTime = DateTime.Now,
                    Version = ConfigAppSettings.WebVersion
                }, ref dbLogID);
            };

            //异步方式写入日志。
            invoke.BeginInvoke(null, null);
        }
    }
}

