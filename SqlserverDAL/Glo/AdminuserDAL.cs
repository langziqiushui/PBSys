// ===============================================================================
// Powered by：.net项目开发工具(V3.3.00.00)  
// Author：有容乃大，Hxw (mrhgw@sohu.com;Http://mrhgw.cnblogs.com) 
// Date：2013-8-13 13:44:18 
// Description：实现数据查询相关功能(适用于Access数据库)。 
// ===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using YX.Domain.Glo;
using YX.Core;

namespace YX.SqlserverDAL.Glo
{
    /// <summary> 
    /// 提供数据查询查关功能(对应表 Glo.AdminUser(管理员用户表) )。
    /// </summary> 
    public partial class AdminuserDAL : AbstractDAL
    {
        public AdminuserDAL() : base() { }

        #region 获取数据

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
        public IList<AdminuserInfo> GetData_Paging(int? pagesize, int? pageindex, ref int numrecord, string username, byte? admintype, bool? isactived)
        {
            IDataReader reader = null;
            var parms = new SqlParameterList();
            IList<AdminuserInfo> entities = null;

            parms.Add(SqlUtil.CreateParameter("@pageSize", DbType.Int32, pagesize));
            parms.Add(SqlUtil.CreateParameter("@pageIndex", DbType.Int32, pageindex));
            parms.Add(SqlUtil.CreateOutputParameter("@numRecord", DbType.Int32, 4));
            parms.Add(SqlUtil.CreateParameter("@UserName", DbType.String, username));
            parms.Add(SqlUtil.CreateParameter("@AdminType", DbType.Byte, admintype));
            parms.Add(SqlUtil.CreateParameter("@IsActived", DbType.Boolean, isactived));

            using (reader = this.ExecuteReaderWithContextDBTran("[Glo].[UP_Sel_AdminUser_Paging]", parms.ToArray()))
            {
                entities = DomainUtil.PopulateDataList<AdminuserInfo>(reader);
            }

            #region 存储过程输出参数

            if (DBNull.Value != parms["@numRecord"].Value)
                numrecord = Convert.ToInt32(parms["@numRecord"].Value);

            #endregion 存储过程输出参数

            return entities;
        }

        /// <summary> 
        /// 获取某一管理员用户。 
        /// </summary> 
        /// <param name="username">用户名</param> 
        /// <returns>查询获取的数据</returns> 
        public AdminuserInfo GetData(string username)
        {
            IDataReader reader = null;
            AdminuserInfo entity = null;
            var parms = new SqlParameterList();

            parms.Add(SqlUtil.CreateParameter("@UserName", DbType.String, username));

            using (reader = this.ExecuteReaderWithContextDBTran("[Glo].[UP_Sel_AdminUser_PrimaryKey]", parms.ToArray()))
            {
                entity = DomainUtil.PopulateData<AdminuserInfo>(reader);
            }
            return entity;
        }

        /// <summary> 
        /// 获取所有管理员用户。 
        /// </summary> 
        /// <returns>查询获取的数据</returns> 
        public IList<AdminuserInfo> GetData_All()
        {
            IDataReader reader = null;
            var parms = new SqlParameterList();
            IList<AdminuserInfo> entities = null;

            using (reader = this.ExecuteReaderWithContextDBTran("[Glo].[UP_Sel_AdminUser_All]", parms.ToArray()))
            {
                entities = DomainUtil.PopulateDataList<AdminuserInfo>(reader);
            }
            return entities;
        }

        #endregion

        #region 添加数据

        /// <summary> 
        /// 创建管理员用户。 
        /// </summary> 
        /// <param name="entity">AdminuserInfo业务实体对象</param> 
        /// <returns>数据添加是否成功</returns> 
        public object Add(AdminuserInfo entity)
        {
            var parms = new SqlParameterList();

            parms.Add(SqlUtil.CreateParameter("@UserName", DbType.String, entity.UserName));
            parms.Add(SqlUtil.CreateParameter("@Password", DbType.String, entity.Password));
            parms.Add(SqlUtil.CreateParameter("@EncryptionFormat", DbType.Byte, entity.EncryptionFormat));
            parms.Add(SqlUtil.CreateParameter("@AdminType", DbType.Byte, entity.AdminType));
            parms.Add(SqlUtil.CreateParameter("@AdminName", DbType.String, entity.AdminName));
            parms.Add(SqlUtil.CreateParameter("@IsActived", DbType.Boolean, entity.IsActived));
            parms.Add(SqlUtil.CreateParameter("@ManageApplication", DbType.String, entity.ManageApplication));
            parms.Add(SqlUtil.CreateParameter("@CreateTime", DbType.DateTime, entity.CreateTime));

            object r = this.ExecuteScalarWithContextDBTran("[Glo].[UP_Ins_AdminUser]", parms.ToArray());
            return r;
        }

        #endregion

        #region 修改数据

        /// <summary> 
        /// 修改某一管理员用户。 
        /// </summary> 
        /// <param name="entity">AdminuserInfo业务实体对象</param> 
        /// <returns>数据修改是否成功</returns> 
        public object Modify(AdminuserInfo entity)
        {
            var parms = new SqlParameterList();

            parms.Add(SqlUtil.CreateParameter("@UserName", DbType.String, entity.UserName));
            parms.Add(SqlUtil.CreateParameter("@Password", DbType.String, entity.Password));
            parms.Add(SqlUtil.CreateParameter("@EncryptionFormat", DbType.Byte, entity.EncryptionFormat));
            parms.Add(SqlUtil.CreateParameter("@AdminType", DbType.Byte, entity.AdminType));
            parms.Add(SqlUtil.CreateParameter("@AdminName", DbType.String, entity.AdminName));
            parms.Add(SqlUtil.CreateParameter("@IsActived", DbType.Boolean, entity.IsActived));
            parms.Add(SqlUtil.CreateParameter("@ManageApplication", DbType.String, entity.ManageApplication));
            parms.Add(SqlUtil.CreateParameter("@CreateTime", DbType.DateTime, entity.CreateTime));

            object r = this.ExecuteScalarWithContextDBTran("[Glo].[UP_Upd_AdminUser_PrimaryKey]", parms.ToArray());
            return r;
        }

        #endregion

        #region 删除数据

        /// <summary> 
        /// 删除某一管理员用户。 
        /// </summary> 
        /// <param name="username">用户名</param> 
        /// <returns>数据删除是否成功</returns> 
        public object Delete(string username)
        {
            var parms = new SqlParameterList();

            parms.Add(SqlUtil.CreateParameter("@UserName", DbType.String, username));

            object r = this.ExecuteScalarWithContextDBTran("[Glo].[UP_Del_AdminUser_PrimaryKey]", parms.ToArray());
            return r;
        }

        #endregion

    }
}
