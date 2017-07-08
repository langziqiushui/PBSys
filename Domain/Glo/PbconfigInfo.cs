// ===============================================================================
// Powered by：.net项目开发工具(V3.3.00.00)  
// Author：有容乃大，Hxw (mrhgw@sohu.com;Http://mrhgw.cnblogs.com) 
// Date：2017/3/9 12:35:57 
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
    /// 业务实体对象，映射数据表 (Glo.PBConfig(屏蔽表) )。 
    /// </summary> 
    [Serializable]
    public partial class PbconfigInfo : BaseDomain
    {
        #region 构造器

        /// <summary> 
        /// 默认构造器。 
        /// </summary> 
        public PbconfigInfo() : base() { }

        #endregion

        #region 变量

        /// <summary>
        /// 当前实体对应的数据表完整名称。
        /// </summary>
        public const string TableName = "Glo.PBConfig";
        /// <summary>
        /// 当前实体对应的数据库完整名称。
        /// </summary>
        public const string DBName = "er_common";

        #endregion

        #region 属性

        /// <summary> 
        /// 获取或设置 屏蔽ID主键。 
        /// </summary> 
        public int ID { get; set; }

        /// <summary> 
        /// 获取或设置 应用程序类别[ApplicationTypes] 1= yx 2=anxiu 3=pp8 4=... 。 
        /// </summary> 
        public byte ApplicationType { get; set; }

        /// <summary> 
        /// 获取或设置 屏蔽类别[PbTypes]软件 = 0,新闻 = 1, 专题=2 。 
        /// </summary> 
        public byte PbType { get; set; }

        /// <summary> 
        /// 获取或设置 主键。 
        /// </summary> 
        public int KeyData { get; set; }

        /// <summary> 
		/// 获取或设置 Title。 
		/// </summary> 
		public string Title { get; set; }


        #endregion
    }
}
