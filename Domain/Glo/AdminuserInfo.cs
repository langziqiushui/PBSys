// ===============================================================================
// Powered by：.net项目开发工具(V3.3.00.00)  
// Author：有容乃大，Hxw (mrhgw@sohu.com;Http://mrhgw.cnblogs.com) 
// Date：2013-8-12 11:37:09 
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
    /// 业务实体对象，映射数据表 (Glo.AdminUser(管理员用户表) )。 
    /// </summary> 
    [Serializable]
    public partial class AdminuserInfo : BaseDomain
    {
        #region 构造器

        /// <summary> 
        /// 默认构造器。 
        /// </summary> 
        public AdminuserInfo() : base() { }

        #endregion

        #region 变量

        /// <summary>
        /// 当前实体对应的数据表完整名称。
        /// </summary>
        public const string TableName = "Glo.AdminUser";
        /// <summary>
        /// 当前实体对应的数据库完整名称。
        /// </summary>
        public const string DBName = "JKServices";

        #endregion

         #region 属性

        /// <summary> 
        /// 获取或设置 用户名。 
        /// </summary> 
        public string UserName { get; set; }

        /// <summary> 
        /// 获取或设置 密码。 
        /// </summary> 
        public string Password { get; set; }

        /// <summary> 
        /// 获取或设置 加密方式[EncryptionFormats]未加密=1, MD5SHA256加密(不可逆)=2, StrongSHA256加密(不可逆)=3, AES加密(可逆)=4 。 
        /// </summary> 
        public byte EncryptionFormat { get; set; }

        /// <summary> 
        /// 获取或设置 管理员类别([AdminTypes]超级管理员=1,普通管理员=2)。 
        /// </summary> 
        public byte AdminType { get; set; }

        /// <summary> 
        /// 获取或设置 管理员名称。 
        /// </summary> 
        public string AdminName { get; set; }

        /// <summary> 
        /// 获取或设置 管理员是否激活。 
        /// </summary> 
        public bool IsActived { get; set; }

        /// <summary> 
        /// 获取或设置 当前管理员所管理的应用程序模块，多个之间以,号分隔([ApplicationTypes]保险 = 1)。 
        /// </summary> 
        public string ManageApplication { get; set; }

        /// <summary> 
        /// 获取或设置 管理员注册日期。 
        /// </summary> 
        public DateTime CreateTime { get; set; }

        #endregion
    }
}
