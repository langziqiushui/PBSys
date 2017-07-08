using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

using YX.IServices;

namespace YX.Services
{
    /// <summary>
    /// MemcachedCache分布式缓存方案。
    /// </summary>
    public class MemcachedCache : IDataCache
    {
        public object this[string key]
        {
            get
            {
                return HttpRuntime.Cache[key];
            }
        }

        public object Remove(string key)
        {
            return HttpRuntime.Cache.Remove(key);
        }       

        public void Insert(string key, object value)
        {
            Insert(key, value, null);
        }

        public void Insert(string key, object value, DateTime? absoluteExpiration)
        {
            HttpRuntime.Cache.Insert(
                key, 
                value, 
                null, 
                null == absoluteExpiration ? DateTime.MaxValue : absoluteExpiration.Value, 
                Cache.NoSlidingExpiration, 
                CacheItemPriority.High, 
                null
            );
        }
    }
}
