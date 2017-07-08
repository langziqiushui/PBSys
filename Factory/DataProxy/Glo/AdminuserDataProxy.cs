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
    /// 数据缓存代理(对应表 Glo.AdminUser(管理员用户表) )。 
    /// </summary> 
    public static class AdminuserDataProxy
    {
        /// <summary> 
        /// 缓存键。 
        /// </summary> 
        private const string KEY = "DataProxy.Glo.AdminUser.All";
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
        public static IList<AdminuserInfo> GetData_All()
        {
            var data = DataCacheFactory.DataCacher[KEY];
            if (null == data)
            {
                lock (__LOCK__)
                {
                    if (null == data)
                    {
                        data = ServicesFactory.CreateGloAdminuserServices().GetData_All();
                        DataCacheFactory.DataCacher.Insert(KEY, data);
                    }
                }
            }

            return (IList<AdminuserInfo>)data;
        }

        /// <summary>
        /// 根据用户名获取用户数据。
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public static AdminuserInfo GetData_UserName(string userName)
        {
            return GetData_All().FirstOrDefault((g) => { return g.UserName.IgnoreCaseEquals(userName); });
        }
    }
}
