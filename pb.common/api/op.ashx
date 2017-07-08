<%@ WebHandler Language="C#" Class="op" %>
using System;
using System.Web;
using YX.Core;
using YX.Domain;
public class op : IHttpHandler
{
    HttpRequest Request;
    HttpResponse Response;
    public void ProcessRequest(HttpContext context)
    {
        Request = context.Request;
        Response = context.Response;
        switch (Request.PathInfo)
        {
            case "/getpb.do":
                getpb();
                break;
            default:
                break;
        }
    }

    void getpb()
    {
        string callback = Request.QueryString["callback"] ?? string.Empty;
        if (string.IsNullOrEmpty(callback))
        {
            Response.Write("callback参数 未指定!");
            return;
        }
        try
        {
            //站点程序
            string ApplicationType = Request.QueryString["at"] ?? string.Empty;
            if (string.IsNullOrEmpty(ApplicationType))
            {
                Response.Write("站点 未指定!");
                return;
            }

            //屏蔽类别
            string PbType = Request.QueryString["pt"] ?? string.Empty;
            if (!string.IsNullOrEmpty(PbType))
            {
                if (PbType.IgnoreCaseIndexOf("news") >= 0)
                    PbType = ((int)PbTypes.news).ToString();

                if (PbType.IgnoreCaseIndexOf("soft") >= 0)
                    PbType = ((int)PbTypes.soft).ToString();

                if (PbType.IgnoreCaseIndexOf("zt") >= 0)
                    PbType = ((int)PbTypes.zt).ToString();

            }
            //转枚举ID 值
            var apptype = (ApplicationTypes)Enum.Parse(typeof(ApplicationTypes), ApplicationType);

            //获取参数
            string KeyData = Request.QueryString["id"] ?? string.Empty;
            if (string.IsNullOrEmpty(KeyData))
            {
                Response.Write("id 未指定!");
                return;
            }

            //获取屏蔽名称 标题
            string strTitle = Request.QueryString["title"] ?? string.Empty;

            
            //Common.DB.IDBHelper dbh = Common.DB.Factory.CreateDBHelper();
            //var objrec = dbh.ExecuteScalar<object>("select top 1 id from [Glo].[PBConfig] with(nolock) where KeyData=@KeyData and ApplicationType=@apptype and PbType=@PbType", dbh.CreateParameter("@KeyData", KeyData), dbh.CreateParameter("@apptype", Convert.ToByte(apptype)), dbh.CreateParameter("@PbType", PbType));

            var dbh = Common.DBX.Factory.CreateDBHelper("default");
            var objrec = dbh.ExecuteScalar<object>("select top 1 id from [Glo].[PBConfig] with(nolock) where KeyData=@0 and ApplicationType=@1 and PbType=@2", KeyData, Convert.ToByte(apptype), PbType);

            if (objrec != null && objrec != DBNull.Value)
            {
                //V2  接收标题数据执行UPDATE
                if (!string.IsNullOrEmpty(strTitle))
                {
                    var istitleobj = dbh.ExecuteScalar<string>("select top 1 title from [Glo].[PBConfig] with(nolock) where KeyData=@0 and ApplicationType=@1 and PbType=@2", KeyData, Convert.ToByte(apptype), PbType);
                    if (string.IsNullOrEmpty(istitleobj))
                        dbh.ExecuteNoneQuery("update [Glo].[PBConfig] set Title=@0 where KeyData=@1" , strTitle,KeyData);
                }
                
                Response.Write(callback);
                Response.Write("(" + ((int)objrec > 0 ? "true" : "false") + ")");
            }
            else
            {
                Response.Write(callback);
                Response.Write("(false)");
            }

        }
        catch (Exception ex)
        {
            Response.Write(callback);
            Response.Write("{\"r\":\"F\",\"m\":\"" + ex.Message + "\"}");
        }
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}