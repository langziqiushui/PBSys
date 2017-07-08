using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

using YX.Core;
using YX.Domain;
using YX.Domain.Glo;

namespace YX.Factory.Glo
{
    /// <summary> 
    /// 数据缓存代理(对应表 Glo.PBConfig(屏蔽表) )。 
    /// </summary> 
    public static class PbconfigDataProxy
    {
        /// <summary> 
        /// 缓存键。 
        /// </summary> 
        private const string KEY = "DataProxy.Glo.PBConfig.All";
        /// <summary> 
        /// 并发锁定对象。 
        /// </summary> 
        private readonly static object __LOCK__ = new object();

        /// <summary> 
        /// 清除缓存。 
        /// </summary> 
        public static void ClearCache()
        {
            lock (__LOCK__)
            {
                DataCacheFactory.DataCacher.Remove(KEY);
            }
        }

        /// <summary> 
        /// 获取所有数据。 
        /// </summary> 
        public static IList<PbconfigInfo> GetData_All()
        {
            var data = DataCacheFactory.DataCacher[KEY];
            if (null == data)
            {
                lock (__LOCK__)
                {
                    if (null == data)
                    {
                        data = ServicesFactory.CreateGloPbconfigServices().GetData_All();
                        DataCacheFactory.DataCacher.Insert(KEY, data, DateTime.Now.AddMinutes(10)); //缓存10分钟
                    }
                }
            }

            return (IList<PbconfigInfo>)data;
        }



        /// <summary>
        /// 判断指定条件户是否是屏蔽数据
        /// </summary>
        /// <param name="applicationtype">应用程序类别[ApplicationTypes] 1= yx 2=anxiu 3=pp8 4=...</param> 
        /// <param name="pbtype">屏蔽类别[PbTypes]软件 = 0,新闻 = 1, 专题=2</param> 
        /// <param name="keydata">主键</param> 
        /// <param name="isnormal">是否正常站(正式=1, 镜像 = 0)</param> 
        /// <returns></returns>
        public static bool GetData_Keywords(byte applicationtype, byte pbtype, int keydata, bool isnormal)
        {
            //var dataPB = GetData_All().Where(g => { return g.ApplicationType == applicationtype && g.PbType == pbtype && g.KeyData == keydata && g.IsNormal == isnormal; }).ToList();
            //if (dataPB.Count > 0)
            //    return true;

            var dataPB2 = GetData_All().FirstOrDefault(g => { return g.ApplicationType == applicationtype && g.PbType == pbtype && g.KeyData == keydata ; });
            if (dataPB2 != null)
                return true;


            return false;
        }

    }
}
