using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YX.Core
{
    /// <summary>
    /// 应用程序AppSettings配置节点。
    /// </summary>
    public static class ConfigAppSettings
    {
        #region 全局

        /// <summary>
        /// 获取 对应当前系统的数据库配置版本号。
        /// </summary>
        public static string DBConfigVersion
        {
            get { return ConfigUtil.GetAppSetting("DBConfigVersion", "T1"); }
        }

        /// <summary>
        /// 获取 网站版本号。
        /// </summary>
        public static string WebVersion
        {
            get { return ConfigUtil.GetAppSetting("WebVersion", "LogWebT1"); }
        }   

        /// <summary>
        /// 获取 本站加密密钥。
        /// </summary>
        public static string Cryptography_Key
        {
            get { return ConfigUtil.GetAppSetting("Cryptography_Key"); }
        }

        /// <summary>
        /// 获取 本站加密向量。
        /// </summary>
        public static string Cryptography_IV
        {
            get { return ConfigUtil.GetAppSetting("Cryptography_IV"); }
        }

        /// <summary>
        /// 获取 Javascript版本号。
        /// </summary>
        public static string JSVersion
        {
            get { return ConfigUtil.GetAppSetting("JSVersion"); }
        }

        /// <summary>
        /// 获取 是否合并压缩JS。
        /// </summary>
        public static bool IsCompressionJS
        {
            get { return bool.Parse(ConfigUtil.GetAppSetting("IsCompressionJS")); }
        }

        /// <summary>
        /// 获取 Css版本号。
        /// </summary>
        public static string CssVersion
        {
            get { return ConfigUtil.GetAppSetting("CssVersion"); }
        }

        /// <summary>
        /// 获取 是否合并压缩CSS。
        /// </summary>
        public static bool IsCompressionCSS
        {
            get { return bool.Parse(ConfigUtil.GetAppSetting("IsCompressionCSS")); }
        }
       
        /// <summary>
        /// 获取 网站名称。
        /// </summary>
        public static string WebSiteName
        {
            get { return ConfigUtil.GetAppSetting("WebSiteName", "综合信息管理系统"); }
        }

        /// <summary>
        /// 获取 是否为测试站点。
        /// </summary>
        public static bool IsTestWeb
        {
            get { return bool.Parse(ConfigUtil.GetAppSetting("IsTestWeb", "false")); }
        }

        #endregion        
    }
}
