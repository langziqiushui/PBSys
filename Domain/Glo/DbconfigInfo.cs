// ===============================================================================


// Date：2015/5/29 16:40:33 
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
    /// 业务实体对象，映射数据表 (Glo.DBConfig(配置参数表) )。 
    /// </summary> 
    [Serializable]
    public partial class DbconfigInfo : BaseDomain
    {
        #region 构造器

        /// <summary> 
        /// 默认构造器。 
        /// </summary> 
        public DbconfigInfo() : base() { }

        #endregion

        #region 变量

        /// <summary>
        /// 当前实体对应的数据表完整名称。
        /// </summary>
        public const string TableName = "Glo.DBConfig";
        /// <summary>
        /// 当前实体对应的数据库完整名称。
        /// </summary>
        public const string DBName = "JikePay2015";

        #endregion

        #region 属性

        /// <summary> 
        /// 获取或设置 配置参数名(枚举：DBConfigKeys)。 
        /// </summary> 
        public string ConfigKey { get; set; }

        /// <summary> 
        /// 获取或设置 系统程序版本号。 
        /// </summary> 
        public string Version { get; set; }

        /// <summary> 
        /// 获取或设置 参数值。 
        /// </summary> 
        public string Value { get; set; }

        #endregion
    }
}
