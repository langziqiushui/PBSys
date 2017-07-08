// ===============================================================================


// Date：2015-06-08 14:42:02 
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
    /// 提供数据查询查关功能(对应表 Glo.DBConfig(配置参数表) )。
    /// </summary> 
    public partial class DbconfigDAL : AbstractDAL
    {
        public DbconfigDAL() : base() { }

        #region 获取数据

        /// <summary> 
        /// 获取所有配置参数。 
        /// </summary> 
        /// <returns>查询获取的数据</returns> 
        public IList<DbconfigInfo> GetData_All()
        {
            IDataReader reader = null;
            var parms = new SqlParameterList();
            IList<DbconfigInfo> entities = null;

            using (reader = this.ExecuteReaderWithContextDBTran("[Glo].[UP_Sel_DBConfig_All]", parms.ToArray()))
            {
                entities = DomainUtil.PopulateDataList<DbconfigInfo>(reader);
            }
            return entities;
        }

        #endregion

        #region 修改数据

        /// <summary> 
        /// 根据主键修改某一配置参数表。 
        /// </summary> 
        /// <param name="entity">DbconfigInfo业务实体对象</param> 
        /// <returns>数据修改是否成功</returns> 
        public object Modify(DbconfigInfo entity)
        {
            var parms = new SqlParameterList();

            parms.Add(SqlUtil.CreateParameter("@ConfigKey", DbType.String, entity.ConfigKey));
            parms.Add(SqlUtil.CreateParameter("@Version", DbType.String, entity.Version));
            parms.Add(SqlUtil.CreateParameter("@Value", DbType.String, entity.Value));

            object r = this.ExecuteScalarWithContextDBTran("[Glo].[UP_Upd_DBConfig_PrimaryKey]", parms.ToArray());
            return r;
        }

        #endregion

        #region 删除数据

        /// <summary> 
        /// 根据主键删除某一配置参数表。 
        /// </summary> 
        /// <param name="configkey">配置参数名(枚举：DBConfigKeys)</param> 
        /// <param name="version">系统程序版本号</param> 
        /// <returns>数据删除是否成功</returns> 
        public object Delete(string configkey, string version)
        {
            var parms = new SqlParameterList();

            parms.Add(SqlUtil.CreateParameter("@ConfigKey", DbType.String, configkey));
            parms.Add(SqlUtil.CreateParameter("@Version", DbType.String, version));

            object r = this.ExecuteScalarWithContextDBTran("[Glo].[UP_Del_DBConfig_PrimaryKey]", parms.ToArray());
            return r;
        }

        #endregion

    }
}
