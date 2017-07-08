using System;
using System.Web;
using System.Web.SessionState;
using YX.Core;
using YX.Factory;
using YX.Web.Framework;
using YX.IServices.Glo;
using YX.Domain;

namespace YX.WebAPI.API
{
    /// <summary>
    /// 通用异步网关接口。
    /// </summary>
    public class MyHandler : IHttpHandler, IRequiresSessionState
    {
        #region IHttpHandler

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            this.MyProcessRequest(context);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region 变量

        private IDblogServices servicesLog = ServicesFactory.CreateGloDblogServices();

        #endregion

        #region 处理请求

        public void MyProcessRequest(HttpContext context)
        {
            var method = context.Request["method"];

            switch (method)
            {
                case "UploadErrorLog":
                    //记录错误日志信息。
                    this.UploadErrorLog(context);
                    break;
                case "GetPBConfig":
                    this.GetPBConfig(context);
                    //HttpContext.Current.Response.Write(this.GetPBConfig(context));
                    break;
                case "GetPBConfigProxy":
                    this.GetPBConfigProxy(context);
                    //HttpContext.Current.Response.Write(this.GetPBConfigProxy(context));
                    break;
                default:
                    HttpContext.Current.Response.Write("对不起，非法请求或丢失查询参数！");
                    break;
            }
        }

        #endregion


        #region 记录错误日志信息

        /// <summary>
        /// 记录错误日志信息。
        /// </summary>
        private void UploadErrorLog(HttpContext context)
        {
            WFUtil.UIDoTryCatch("记录错误日志信息", () =>
            {
                //验证数据是否符合规范
                #region 获取参数

                //日志类别 
                var logtype = context.Request["logtype"];
                if (string.IsNullOrEmpty(logtype))
                    return;



                //错误信息
                var exMessage = context.Request["exMessage"];
                if (string.IsNullOrEmpty(exMessage))
                    return;


                //访问ip
                var reip = context.Request["reip"];



                //上级来源页面
                var resSource = context.Request["resSource"];

                //来源页面
                var resurl = context.Request["resurl"];

            
                if (context.Request.UrlReferrer != null)
                {
                    resurl = context.Request.UrlReferrer.ToString();
                }

                //根据关键词过滤URL 404页面
                foreach (var item in DBConfigFactory.Log_404filterKeywords.Split('|'))
                {
                    //过滤HTML 标签  
                    if (resurl.IndexOf(item) >= 0 && logtype == "0")
                        return;
                }

                //根据关键词过滤URL 500页面
                foreach (var item in DBConfigFactory.Log_500filterKeywords.Split('|'))
                {
                    //过滤HTML 标签  
                    if (resurl.IndexOf(item) >= 0 && logtype == "1")
                        return;
                }

                //操作系统
                var resPlatform = context.Request["resPlatform"];
                if (string.IsNullOrEmpty(resPlatform))
                    resPlatform = context.Request.Browser.Platform.ToString();

                //浏览器 +浏览器版本
                var resbrowser = context.Request["resbrowser"];
                if (string.IsNullOrEmpty(resbrowser))
                    resbrowser = context.Request.Browser.Browser.ToString() + context.Request.Browser.MajorVersion.ToString();

                //应用程序 
                var apptype = context.Request["apptype"];
                var restype = context.Request["restype"];

                if (apptype == "-1" || restype == "-1")
                {
                    if (resurl.IgnoreCaseIndexOf("yiwan.com") >= 0)
                        apptype = ((int)ApplicationTypes.myiwan).ToString();

                    if (resurl.IgnoreCaseIndexOf("shouji56.com") >= 0)
                        apptype = ((int)ApplicationTypes.mshouji56).ToString();

                    if (resurl.IgnoreCaseIndexOf("497.com") >= 0)
                        apptype = ((int)ApplicationTypes.m497).ToString();

                    if (resurl.IgnoreCaseIndexOf("391k.com") >= 0)
                        apptype = ((int)ApplicationTypes.m391k).ToString();

                    if (resurl.IgnoreCaseIndexOf("kuaila.com") >= 0)
                        apptype = ((int)ApplicationTypes.mkuaila).ToString();

                    if (resurl.IgnoreCaseIndexOf("qqxzb.com") >= 0)
                        apptype = ((int)ApplicationTypes.mqqxzb).ToString();

                    if (resurl.IgnoreCaseIndexOf("yxdown.com") >= 0)
                        apptype = ((int)ApplicationTypes.myxdown).ToString();

                    if (resurl.IgnoreCaseIndexOf("caoxie.com") >= 0)
                        apptype = ((int)ApplicationTypes.mcaoxie).ToString();

                    if (resurl.IgnoreCaseIndexOf("pp8.com") >= 0)
                        apptype = ((int)ApplicationTypes.mpp8).ToString();

                    if (resurl.IgnoreCaseIndexOf("499.cn") >= 0)
                        apptype = ((int)ApplicationTypes.m499).ToString();

                    if (resurl.IgnoreCaseIndexOf("anxiu.com") >= 0)
                        apptype = ((int)ApplicationTypes.manxiu).ToString();

                    if (resurl.IgnoreCaseIndexOf("lanqie.com") >= 0)
                        apptype = ((int)ApplicationTypes.mlanqie).ToString();

                    if (resurl.IgnoreCaseIndexOf("yx85.com") >= 0)
                        apptype = ((int)ApplicationTypes.myx85).ToString();

                    if (resurl.IgnoreCaseIndexOf("1j1j.com") >= 0)
                    {
                        apptype = ((int)ApplicationTypes.m1j1j).ToString();
                    }

                    //类别
                    //默认API  接口 DPS  TONGJI
                    restype = ((int)Restypes.API).ToString();

                    if (resurl.IgnoreCaseIndexOf("bianji.") >= 0)
                        restype = ((int)Restypes.ADMINsite).ToString();

                    if (resurl.IgnoreCaseIndexOf("http://m.") >= 0)
                        restype = ((int)Restypes.Msite).ToString();

                    if (resurl.IgnoreCaseIndexOf("www.") >= 0)
                        restype = ((int)Restypes.PCsite).ToString();
                }

                if (string.IsNullOrEmpty(apptype) || string.IsNullOrEmpty(restype))
                    return;
                //站点属性 




                int dbLogID = 0;
                servicesLog.Add(new YX.Domain.Glo.DblogInfo()
                {
                    ApplicationType = Convert.ToByte(apptype),
                    LogType = Convert.ToByte(logtype),
                    Restype = Convert.ToByte(restype),
                    ResIP = reip,
                    ResSource = resSource,
                    Resurl = resurl,
                    LogContent = exMessage,
                    ResPlatform = resPlatform,
                    ResBrowser = resbrowser,
                    CreateTime = DateTime.Now,
                    Version = ConfigAppSettings.WebVersion
                }, ref dbLogID);

                #endregion


            }, false, (Exception ex) =>
            {
                HttpContext.Current.Response.Write("记录错误日志信息失败：" + ex.Message);
            });
        }

        #endregion

        /// <summary>
        /// 验证是否屏蔽。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private void GetPBConfig(HttpContext context)
        {
            var callback = CoreUtil.Request(context, "callback") ?? string.Empty;
            if (string.IsNullOrEmpty(callback))
            {
                context.Response.Write("callback 参数 未 指定");
                return;
            }
            try
            {
                //获取参数
                var ApplicationType = CoreUtil.Request(context, "at");
                //if (string.IsNullOrEmpty(ApplicationType))
                //{
                //    var resurl = string.Empty;
                //    if (context.Request.UrlReferrer != null)
                //    {                       
                //        resurl = context.Request.UrlReferrer.ToString();
                //    }                  
                //    if (string.IsNullOrEmpty(resurl))
                //    {
                //        context.Response.Write("未指定来源");
                //        return;
                //    }

                //    //如果用户未传来源则根据 UrlReferrer 来判断站点
                //    resurl = resurl.ToString();
                //    if (resurl.IgnoreCaseIndexOf("yiwan.com") >= 0)
                //        ApplicationType = ApplicationTypes.myiwan.ToString();

                //    if (resurl.IgnoreCaseIndexOf("shouji56.com") >= 0)
                //        ApplicationType = ApplicationTypes.mshouji56.ToString();

                //    if (resurl.IgnoreCaseIndexOf("497.com") >= 0)
                //        ApplicationType = ApplicationTypes.m497.ToString();

                //    if (resurl.IgnoreCaseIndexOf("391k.com") >= 0)
                //        ApplicationType = ApplicationTypes.m391k.ToString();

                //    if (resurl.IgnoreCaseIndexOf("kuaila.com") >= 0)
                //        ApplicationType = ApplicationTypes.mkuaila.ToString();

                //    if (resurl.IgnoreCaseIndexOf("qqxzb.com") >= 0)
                //        ApplicationType = ApplicationTypes.mqqxzb.ToString();

                //    if (resurl.IgnoreCaseIndexOf("yxdown.com") >= 0)
                //        ApplicationType = ApplicationTypes.myxdown.ToString();

                //    if (resurl.IgnoreCaseIndexOf("caoxie.com") >= 0)
                //        ApplicationType = ApplicationTypes.mcaoxie.ToString();

                //    if (resurl.IgnoreCaseIndexOf("pp8.com") >= 0)
                //        ApplicationType = ApplicationTypes.mpp8.ToString();

                //    if (resurl.IgnoreCaseIndexOf("499.cn") >= 0)
                //        ApplicationType = ApplicationTypes.m499.ToString();

                //    if (resurl.IgnoreCaseIndexOf("anxiu.com") >= 0)
                //        ApplicationType = ApplicationTypes.manxiu.ToString();

                //    if (resurl.IgnoreCaseIndexOf("lanqie.com") >= 0)
                //        ApplicationType = ApplicationTypes.mlanqie.ToString();

                //    if (resurl.IgnoreCaseIndexOf("yx85.com") >= 0)
                //        ApplicationType = ApplicationTypes.myx85.ToString();

                //    if (resurl.IgnoreCaseIndexOf("1j1j.com") >= 0)
                //    {
                //        ApplicationType = ApplicationTypes.m1j1j.ToString();
                //    }
                //}
                var apptype = (ApplicationTypes)Enum.Parse(typeof(ApplicationTypes), ApplicationType);

                //获取参数
                var PbType = CoreUtil.Request(context, "pt");
                if(!string.IsNullOrEmpty(PbType))
                {
                    if (PbType.IgnoreCaseIndexOf("news") >= 0)
                        PbType = ((int)PbTypes.news).ToString();

                    if (PbType.IgnoreCaseIndexOf("soft") >= 0)
                        PbType = ((int)PbTypes.soft).ToString();

                    if (PbType.IgnoreCaseIndexOf("zt") >= 0)
                        PbType = ((int)PbTypes.zt).ToString();
                }

                //获取参数
                var KeyData = CoreUtil.Request(context, "id");
                if (string.IsNullOrEmpty(KeyData))
                {
                    context.Response.Write("异常");
                    return ;
                }
                                  

                //获取参数
                var IsNormal = CoreUtil.Request(context, "isp");
                //默认正式站
                if (string.IsNullOrEmpty(IsNormal))
                {
                    IsNormal = "true";
                }
              
                

                var PBConfigList = ServicesFactory.CreateGloPbconfigServices().GetData_Bywhere(Convert.ToByte(apptype), byte.Parse(PbType), int.Parse(KeyData));
              
                if (PBConfigList.ToString() == "1")
                {
                    context.Response.Write(callback);
                    context.Response.Write("(true)");
                  
                }
                else
                {
                    context.Response.Write(callback);
                    context.Response.Write("(false)");
                   
                }

            }
            catch (Exception ex)
            {
                context.Response.Write(callback);
                context.Response.Write("{\"r\":\"F\",\"m\":\"" + ex.Message + "\"}");
            }
        }


        /// <summary>
        /// 根据缓存 验证是否屏蔽。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private void GetPBConfigProxy(HttpContext context)
        {
            var callback = CoreUtil.Request(context, "callback") ?? string.Empty;
            if (string.IsNullOrEmpty(callback))
            {
                context.Response.Write("callback参数未 指定");
                return;
            }

            try
            {
                ////获取参数
                //var ApplicationType = byte.Parse(CoreUtil.Request(context, "at"));
                ////获取参数
                //var PbType = byte.Parse(CoreUtil.Request(context, "pt"));
                ////获取参数
                //var KeyData = int.Parse(CoreUtil.Request(context, "id"));
                ////获取参数
                //var IsNormal = Convert.ToBoolean(CoreUtil.Request(context, "isp"));

                //获取参数
                var ApplicationType = CoreUtil.Request(context, "at");
                if (string.IsNullOrEmpty(ApplicationType))
                {
                    var resurl = string.Empty;
                    if (context.Request.UrlReferrer != null)
                    {
                        resurl = context.Request.UrlReferrer.ToString();
                    }
                    if (string.IsNullOrEmpty(resurl))
                    {
                        context.Response.Write("未指定来源");
                        return;
                    }

                    //如果用户未传来源则根据 UrlReferrer 来判断站点
                    resurl = resurl.ToString();
                    if (resurl.IgnoreCaseIndexOf("yiwan.com") >= 0)
                        ApplicationType = ApplicationTypes.myiwan.ToString();

                    if (resurl.IgnoreCaseIndexOf("shouji56.com") >= 0)
                        ApplicationType = ApplicationTypes.mshouji56.ToString();

                    if (resurl.IgnoreCaseIndexOf("497.com") >= 0)
                        ApplicationType = ApplicationTypes.m497.ToString();

                    if (resurl.IgnoreCaseIndexOf("391k.com") >= 0)
                        ApplicationType = ApplicationTypes.m391k.ToString();

                    if (resurl.IgnoreCaseIndexOf("kuaila.com") >= 0)
                        ApplicationType = ApplicationTypes.mkuaila.ToString();

                    if (resurl.IgnoreCaseIndexOf("qqxzb.com") >= 0)
                        ApplicationType = ApplicationTypes.mqqxzb.ToString();

                    if (resurl.IgnoreCaseIndexOf("yxdown.com") >= 0)
                        ApplicationType = ApplicationTypes.myxdown.ToString();

                    if (resurl.IgnoreCaseIndexOf("caoxie.com") >= 0)
                        ApplicationType = ApplicationTypes.mcaoxie.ToString();

                    if (resurl.IgnoreCaseIndexOf("pp8.com") >= 0)
                        ApplicationType = ApplicationTypes.mpp8.ToString();

                    if (resurl.IgnoreCaseIndexOf("499.cn") >= 0)
                        ApplicationType = ApplicationTypes.m499.ToString();

                    if (resurl.IgnoreCaseIndexOf("anxiu.com") >= 0)
                        ApplicationType = ApplicationTypes.manxiu.ToString();

                    if (resurl.IgnoreCaseIndexOf("lanqie.com") >= 0)
                        ApplicationType = ApplicationTypes.mlanqie.ToString();

                    if (resurl.IgnoreCaseIndexOf("yx85.com") >= 0)
                        ApplicationType = ApplicationTypes.myx85.ToString();

                    if (resurl.IgnoreCaseIndexOf("1j1j.com") >= 0)
                    {
                        ApplicationType = ApplicationTypes.m1j1j.ToString();
                    }
                }
                var apptype = (ApplicationTypes)Enum.Parse(typeof(ApplicationTypes), ApplicationType);

                //获取参数
                var PbType = CoreUtil.Request(context, "pt");
                if (!string.IsNullOrEmpty(PbType))
                {
                    if (PbType.IgnoreCaseIndexOf("news") >= 0)
                        PbType = ((int)PbTypes.news).ToString();

                    if (PbType.IgnoreCaseIndexOf("soft") >= 0)
                        PbType = ((int)PbTypes.soft).ToString();

                    if (PbType.IgnoreCaseIndexOf("zt") >= 0)
                        PbType = ((int)PbTypes.zt).ToString();
                }

                //获取参数
                var KeyData = CoreUtil.Request(context, "id");
                if (string.IsNullOrEmpty(KeyData))
                {
                    context.Response.Write("异常");
                    return;
                }


                //获取参数
                var IsNormal = CoreUtil.Request(context, "isp");
                //默认正式站
                if (string.IsNullOrEmpty(IsNormal))
                {
                    IsNormal = "true";
                }

                if (Factory.Glo.PbconfigDataProxy.GetData_Keywords(Convert.ToByte(apptype), byte.Parse(PbType), int.Parse(KeyData), Convert.ToBoolean(IsNormal)))
                {
                    context.Response.Write(callback);
                    context.Response.Write("(true)");
                }
                else
                {
                    context.Response.Write(callback);
                    context.Response.Write("(false)");
                   
                }
            }
            catch (Exception ex)
            {
                context.Response.Write(callback);
                context.Response.Write("{\"r\":\"F\",\"m\":\"" + ex.Message + "\"}");
            }
        }

        #region 通用方法

        /// <summary>
        /// 获取请求参数的值
        /// </summary>
        private string GetParaValue(HttpContext context, string para)
        {
            return context.Request[para].Trim();
        }

        #endregion 通用方法

    }
}