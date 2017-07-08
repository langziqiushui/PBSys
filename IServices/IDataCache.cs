using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YX.IServices
{
    /// <summary>
    /// 数据缓存接口。
    /// </summary>
    public interface IDataCache
    {
        /// <summary>
        /// 获取指定缓存键的缓存对象。
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns></returns>
        object this[string key] { get; }

        /// <summary>
        /// 移除指定缓存键的缓存对象。
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>从 Cache 移除的项。如果未找到键参数中的值，则返回 null</returns>
        object Remove(string key);

        /// <summary>
        /// 添加缓存。
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <returns></returns>
        void Insert(string key, object value);

        /// <summary>
        /// 添加缓存。
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="absoluteExpiration">缓存过期时间</param>
        /// <returns></returns>
        void Insert(string key, object value, DateTime? absoluteExpiration);
    }
}
