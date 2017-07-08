using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

using YX.Core;
using YX.IServices;

namespace YX.Factory
{
    /// <summary>
    /// 数据缓存对象工厂。
    /// </summary>
    public class DataCacheFactory : BaseFactory
    {
        private DataCacheFactory()
        {
            this._dataCache = this.CreateObject("DataCacher", true) as IDataCache;
        }

        /// <summary>
        /// 数据缓存对象。
        /// </summary>
        private IDataCache _dataCache = null;

        /// <summary>
        /// 获取 数据缓存对象。
        /// </summary>        
        public static IDataCache DataCacher
        {
            get { return Nested.instance._dataCache; }
        }

        /// <summary>
        /// 内嵌类为创建单例对象。
        /// </summary>
        class Nested
        {
            static Nested() { }
            internal static readonly DataCacheFactory instance = new DataCacheFactory();
        }
    }
}
