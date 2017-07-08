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
    /// 数据缓存代理(对应表 Glo.DBConfig(配置参数表) )。 
    /// </summary> 
    public static class DbconfigDataProxy
    {
        /// <summary> 
        /// 缓存键。 
        /// </summary> 
        public const string KEY = "DataProxy.Glo.DBConfig.All";
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
                //清除缓存。
                DataCacheFactory.DataCacher.Remove(KEY);
                //清除变量缓存。
                //DBConfigFactory.ClearCache();
            }
        }

        /// <summary> 
        /// 获取所有数据。 
        /// </summary> 
        public static IList<DbconfigInfo> GetData_All()
        {
            var data = DataCacheFactory.DataCacher[KEY];
            if (null == data)
            {
                lock (__LOCK__)
                {
                    data = ServicesFactory.CreateGloDbconfigServices().GetData_All().Where(g => { return g.Version.IgnoreCaseEquals(ConfigAppSettings.DBConfigVersion); }).ToList();
                    DataCacheFactory.DataCacher.Insert(KEY, data, DateTime.Now.AddMinutes(3));
                }
            }

            return (IList<DbconfigInfo>)data;
        }

        /// <summary>
        /// 根据配置参数键获取值。
        /// </summary>
        /// <param name="dbConfigKey">配置参数键</param>
        /// <returns></returns>
        public static string GetValue(DBConfigKeys dbConfigKey)
        {
            var entity = GetData_All().FirstOrDefault((g) => { return g.ConfigKey == dbConfigKey.ToString(); });
            if (null != entity)
                return entity.Value;

            return null;
        }        
    }
}
