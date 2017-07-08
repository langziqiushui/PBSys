using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.SessionState;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections;

using YX.Domain;
using YX.Core;
using YX.Factory;
using YX.Web.Framework;
using YX.Component;
using YX.Domain.Glo;
using System.Drawing;
using System.Data;

namespace YX.Web.API
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

        #endregion

        #region 处理请求

        public void MyProcessRequest(HttpContext context)
        {
            var method = context.Request["method"];          

            switch (method)
            {
                case "RenderDataSource":
                    //生成DataSelector数据源。
                    this.RenderDataSource(context);
                    break;
                default:
                    HttpContext.Current.Response.Write("对不起，非法请求或丢失查询参数！");
                    break;
            }
        }


        #endregion


        #region 生成DataSelector数据源

        /// <summary>
        /// 生成DataSelector数据源。
        /// </summary>
        private void RenderDataSource(HttpContext context)
        {
            var json = WFUtil.DoTryCatch("获取信息", () =>
            {
                var cJson = "\"\"";
                var dsType = context.Request["dsType"];



                #region 站点数据

                if (dsType.IgnoreCaseEquals("siteCategory"))
                {
                    //生成站点数据。
                    cJson = this.RendersiteCategory();
                }

                #endregion



                return WFUtil.MakeSuccessJsonData(cJson);
            });

            context.Response.Write(json);
        }

        #endregion

        /// <summary>
        /// 生成站点数据。 
        /// </summary>
        /// <returns></returns>
        private string RendersiteCategory()
        {            
            var tabs = new string[] { "站点" };

            var sbJson = new StringBuilder();

            //加入tab。
            sbJson.Append("{\"tab\":[");
            foreach (var _tab in tabs)
                sbJson.AppendFormat("\"{0}\",", _tab);
            if (sbJson.Length > 0)
                sbJson = sbJson.Remove(sbJson.Length - 1, 1);
            sbJson.Append("],\"contents\":[");

            //获取所有章节分类。
            foreach (var _name in Enum.GetNames(typeof(ApplicationTypes)))
            {
                //var _em = (LogTypes)Enum.Parse(typeof(LogTypes), _name, true);
                ApplicationTypes _enItem = (ApplicationTypes)Enum.Parse(typeof(ApplicationTypes), _name);
                //sb.Append("{\"n\":\"" + _item.Name.Trim() + "\",\"v\":\"" + _item.ChapterID + "\",\"v2\":""},");
                sbJson.Append("{");
                sbJson.AppendFormat(
                    "\"id\": \"{0}\",\"pid\": \"\",\"n\":\"{1}\",\"c\":\"\"",
                    (int)_enItem,
                    EnumDescription.GetFieldText((ApplicationTypes)_enItem)
                );
                sbJson.Append("},");
            }

            if (sbJson.Length > 0)
                sbJson = sbJson.Remove(sbJson.Length - 1, 1);
            sbJson.Append("]}");

            return sbJson.ToString();
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