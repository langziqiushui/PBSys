using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text.RegularExpressions;
using System.Web;
using System.IO;
using System.Net;


/// <summary>
/// CountHelper 的摘要说明
/// </summary>
public class PbHelper
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="refresh"></param>
    /// <returns></returns>
    public static List<Hashtable> GetCachePBs(bool refresh)
    {
        string cacheKey = "cache_pbs";
        var cacheObject = HttpRuntime.Cache.Get(cacheKey);
        List<Hashtable> list = null;
        if (cacheObject != null && cacheObject != DBNull.Value && !refresh)
        {
            list = (List<Hashtable>)cacheObject;
        }
        else
        {
            Common.DB.IDBHelper dbh = Common.DB.Factory.CreateDBHelper();

            list = dbh.GetDataList("select ID,ApplicationType,PbType,KeyData from Glo.[PBConfig] with(nolock) ");

            HttpRuntime.Cache.Insert(cacheKey, list, null, DateTime.Now.AddMinutes(15), TimeSpan.Zero); //缓存15分钟
        }

        return list;
    }

    public static bool GetCacheCHPB(string applicationtype, string pbtype, string keydata)
    {
        var list = GetCachePBs(false);

       
        var ob = list.SingleOrDefault(g => { return g["ApplicationType"].ToString() == applicationtype && g["PbType"].ToString() == pbtype && g["KeyData"].ToString() == keydata; });

        if (ob == null)
        {
            list = GetCachePBs(true);

            ob = list.SingleOrDefault(g => { return g["ApplicationType"].ToString() == applicationtype && g["PbType"].ToString() == pbtype && g["KeyData"].ToString() == keydata; });
        }

        if (ob != null)
        {
            return true;
        }
        return false;
    }
}