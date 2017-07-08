// ===============================================================================
// Powered by：.net项目开发工具(V3.3.00.00)  
// Author：有容乃大，Hxw (mrhgw@sohu.com;Http://mrhgw.cnblogs.com) 
// Date：2017/3/2 15:14:15 
// Description：业务实体对象。 
// ===============================================================================

using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;

using YX.Core;

namespace YX.Domain.Glo
{
    /// <summary> 
    /// 业务实体对象，映射数据表 (Glo.DBLog )。 
    /// </summary> 
    [Serializable]
    public partial class DblogInfo : BaseDomain
    {
        #region 构造器

        /// <summary> 
        /// 默认构造器。 
        /// </summary> 
        public DblogInfo() : base() { }

        #endregion

        #region 变量

        /// <summary>
        /// 当前实体对应的数据表完整名称。
        /// </summary>
        public const string TableName = "Glo.DBLog";
        /// <summary>
        /// 当前实体对应的数据库完整名称。
        /// </summary>
        public const string DBName = "er_common";

        #endregion

        #region 属性

        /// <summary> 
        /// 获取或设置 DBLogID。 
        /// </summary> 
        public int DBLogID { get; set; }

        /// <summary> 
        /// 获取或设置 应用程序类别[ApplicationTypes]  1= yx  2=anxiu 3=pp8 4=...。 
        /// </summary> 
        public byte ApplicationType { get; set; }

        /// <summary> 
        /// 获取或设置 日志类别[LogTypes]400 = 0,500 = 1, 自定义=2。 
        /// </summary> 
        public byte LogType { get; set; }

        /// <summary> 
        /// 获取或设置 主键数据 。 
        /// </summary> 
        public string PrimaryKeyData { get; set; }

        /// <summary> 
        /// 获取或设置 标题 。 
        /// </summary> 
        public string Subject { get; set; }

        /// <summary> 
        /// 获取或设置 日志内容 。 
        /// </summary> 
        public string LogContent { get; set; }

        /// <summary> 
        /// 获取或设置 类别类别[Restypes]M站 = 0,WEB站 = 1, 后台=2, APP = 3, 接口=4 。 
        /// </summary> 
        public byte? Restype { get; set; }

        /// <summary> 
        /// 获取或设置 错误源URL。 
        /// </summary> 
        public string Resurl { get; set; }

        /// <summary> 
        /// 获取或设置 访问浏览器。 
        /// </summary> 
        public string ResBrowser { get; set; }

        /// <summary> 
        /// 获取或设置 访问平台。 
        /// </summary> 
        public string ResPlatform { get; set; }

        /// <summary> 
        /// 获取或设置 访问源。 
        /// </summary> 
        public string ResSource { get; set; }

        /// <summary> 
        /// 获取或设置 访问IP。 
        /// </summary> 
        public string ResIP { get; set; }

        /// <summary> 
        /// 获取或设置 日志生成时间 。 
        /// </summary> 
        public DateTime CreateTime { get; set; }

        /// <summary> 
        /// 获取或设置 日志版本号 。 
        /// </summary> 
        public string Version { get; set; }

        #endregion
    }
}
