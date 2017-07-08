// ===============================================================================
// Powered by：.net项目开发工具(V3.3.00.00)  
// Author：有容乃大，Hxw (mrhgw@sohu.com;Http://mrhgw.cnblogs.com) 
// Date：2013-8-12 10:24:44 
// Description：业务服务接口。 
// ===============================================================================

using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;

using YX.Core;
using YX.Domain;

using YX.Domain.Glo;

namespace YX.IServices.Glo
{
    /// <summary> 
    /// 业务服务接口(对应表 Glo.AdminUser (管理员用户表) )。 
    /// </summary> 
    public interface IAdminuserServices : IDisposable
    {
        #region 获取数据

        /// <summary> 
        /// 获取某一管理员用户。 
        /// </summary> 
        /// <param name="username">用户名</param> 
        /// <returns>查询获取的数据</returns> 
        AdminuserInfo GetData(string username);

        /// <summary> 
        /// 分页获取管理员用户。 
        /// </summary> 
        /// <param name="pagesize">单页显示的记录数</param> 
        /// <param name="pageindex">当前页码</param> 
        /// <param name="numrecord">所有符合查询条件的总记录数</param> 
        /// <param name="username">用户名</param> 
        /// <param name="admintype">管理员类别([AdminTypes]普通管理员=1,超级管理员=2)</param> 
        /// <param name="isactived">管理员是否激活</param> 
        /// <returns>查询获取的数据集</returns> 
        IList<AdminuserInfo> GetData_Paging(int? pagesize, int? pageindex, ref int numrecord, string username, byte? admintype, bool? isactived);

        /// <summary> 
        /// 获取所有管理员用户。 
        /// </summary> 
        /// <returns>查询获取的数据</returns> 
        IList<AdminuserInfo> GetData_All();

        /// <summary> 
        /// 用户登陆。 
        /// </summary> 
        /// <param name="userName">用户名</param> 
        /// <param name="password">密码</param> 
        /// <returns>登录是否成功</returns> 
        AdminuserInfo Login(string userName, string password);

        #endregion

        #region 注册管理员

        /// <summary> 
        /// 注册管理员。 
        /// </summary> 
        /// <param name="entity">AdminuserInfo业务实体对象</param> 
        /// <returns>注册管理员是否成功</returns> 
        bool RegAdmin(AdminuserInfo entity);

        #endregion

        #region 修改数据

        /// <summary> 
        /// 修改某一管理员用户。 
        /// </summary> 
        /// <param name="entity">AdminuserInfo业务实体对象</param> 
        /// <param name="newPassword">新密码(如果不需要修改密码请为null)</param>
        /// <returns>数据修改是否成功</returns> 
        object Modify(AdminuserInfo entity, string newPassword);

        /// <summary>
        /// 激活或禁用用户。
        /// </summary>
        /// <param name="isActived">是否激活用户</param>
        /// <param name="adminUserNames">管理员用户集合</param>
        /// <returns></returns>
        bool ActivateOrDisableUser(bool isActived, string[] adminUserNames);

        #endregion

        #region 删除数据

        /// <summary> 
        /// 删除某一管理员用户。 
        /// </summary> 
        /// <param name="username">用户名</param> 
        /// <returns>数据删除是否成功</returns> 
        object Delete(string username);

        #endregion

    }

}
