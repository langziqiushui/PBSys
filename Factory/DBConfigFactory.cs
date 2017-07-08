using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using YX.Domain;
using YX.Core;
using YX.IServices;


namespace YX.Factory
{
    /// <summary>
    /// 依赖于数据库配置项工厂。
    /// </summary>
    public static class DBConfigFactory
    {
        #region 变量

        /// <summary>
        /// 数据库配置参数服务对象。
        /// </summary>
        public static IServices.Glo.IDbconfigServices server = ServicesFactory.CreateGloDbconfigServices();
        /// <summary>
        /// 并发控制锁对象。
        /// </summary>
        public static readonly object __LOCK__ = new object();
       
       

        #endregion

        #region 通用方法

        /// <summary>
        /// 根据配置参数键获取配置值。
        /// </summary>
        /// <param name="dbConfigKey">配置参数键</param>
        /// <returns></returns>
        public static string GetConfig(DBConfigKeys dbConfigKey)
        {
            return GetConfig(dbConfigKey, string.Empty);
        }

        /// <summary>
        /// 根据配置参数键获取配置值。
        /// </summary>
        /// <param name="dbConfigKey">配置参数键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string GetConfig(DBConfigKeys dbConfigKey, string defaultValue)
        {
            var v = Factory.Glo.DbconfigDataProxy.GetValue(dbConfigKey);
            if (string.IsNullOrEmpty(v))
                return defaultValue;

            return v;
        }

        /// <summary>
        /// 设置配置参数值。
        /// </summary>
        /// <param name="dbConfigKey">配置参数键</param>
        /// <param name="value">配置值</param>
        public static void SetConfig(DBConfigKeys dbConfigKey, string value)
        {
            server.Modify(new Domain.Glo.DbconfigInfo()
            {
                ConfigKey = dbConfigKey.ToString(),
                Version = ConfigAppSettings.DBConfigVersion,
                Value = value
            });
        }


        #endregion

        #region 日志相关

        /// <summary>
        /// 404日志的URL 过滤的关键词。
        /// </summary>
        public static string Log_404filterKeywords
        {
            get { return GetConfig(DBConfigKeys.Log_404filterKeywords); }
        }

        /// <summary>
        /// 500日志的URL 过滤的关键词。
        /// </summary>
        public static string Log_500filterKeywords
        {
            get { return GetConfig(DBConfigKeys.Log_500filterKeywords); }
        }

        #endregion

        #region 不要但不能删除

        /// <summary>
        /// 获取 图片资源URL。
        /// </summary>
        public static string Global_ImageResourceUrl
        {
            get { return string.Empty; }
          
        }


        /// <summary>
        /// 获取 文件服务器文件上传网关。
        /// </summary>
        public static string Global_ImageUploadGateway
        {
            get { return string.Empty; }
            //get { return GetConfig(DBConfigKeys.Global_ImageUploadGateway, "http://i-1.bbtiku.com/API/FileManager.ashx"); }
        }

        #endregion
    }
}
