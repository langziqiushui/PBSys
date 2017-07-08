// ===============================================================================


// Date：2015-06-08 14:42:02 
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
    /// 业务服务接口(对应表 Glo.DBConfig (配置参数表) )。 
    /// </summary> 
    public interface IDbconfigServices : IDisposable
    {
        #region 获取数据

        /// <summary> 
        /// 获取所有配置参数。 
        /// </summary> 
        /// <returns>查询获取的数据</returns> 
        IList<DbconfigInfo> GetData_All();

        #endregion

        #region 修改数据

        /// <summary> 
        /// 根据主键修改某一配置参数表。 
        /// </summary> 
        /// <param name="entity">DbconfigInfo业务实体对象</param> 
        /// <returns>数据修改是否成功</returns> 
        object Modify(DbconfigInfo entity);

        #endregion

        #region 删除数据

        /// <summary> 
        /// 根据主键删除某一配置参数表。 
        /// </summary> 
        /// <param name="configkey">配置参数名(枚举：DBConfigKeys)</param> 
        /// <param name="version">系统程序版本号</param> 
        /// <returns>数据删除是否成功</returns> 
        object Delete(string configkey, string version);

        /// <summary> 
        /// 启用事务一次性删除多条数据。 
        /// </summary> 
        bool Delete(IList<string> configkeys, IList<string> versions);

        #endregion

    }

}
