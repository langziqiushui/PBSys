// ===============================================================================


// Date：2015-06-08 14:42:02 
// Description：业务服务处理。 
// ===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

using YX.Core;
using YX.IServices.Glo;
using YX.Domain;
using YX.Domain.Glo;

namespace YX.Services.Glo
{
    /// <summary> 
    /// 业务服务处理(对应表 Glo.DBConfig(配置参数表) )。 
    /// </summary> 
    public partial class DbconfigServices : BaseServices, IDbconfigServices
    {
        public DbconfigServices() : base() { }

        #region 变量

        /// <summary> 
        /// 数据访问对象(DbconfigDAL(配置参数表))。 
        /// </summary> 
        internal static readonly YX.SqlserverDAL.Glo.DbconfigDAL dal = new YX.SqlserverDAL.Glo.DbconfigDAL();

        #endregion

        #region 获取数据

        /// <summary> 
        /// 获取所有配置参数。 
        /// </summary> 
        /// <returns>查询获取的数据</returns> 
        public IList<DbconfigInfo> GetData_All()
        {
            IList<DbconfigInfo> r = default(IList<DbconfigInfo>);

            ContextDB.DoDBExec(() =>
            {
                r = dal.GetData_All();
            });

            return r;
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
            object r = default(object);

            ContextDB.DoDBExec(() =>
            {
                //获取以前的配置。
                var entityOld = Factory.Glo.DbconfigDataProxy.GetData_All().FirstOrDefault(g => { return g.ConfigKey == entity.ConfigKey && g.Version == entity.Version; });
               
                r = dal.Modify(entity);
            });

            //清除缓存。
            Factory.Glo.DbconfigDataProxy.ClearCache();

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
            object r = default(object);

            ContextDB.DoDBExec(() =>
            {
                r = dal.Delete(configkey, version);
            });

            //清除缓存。
            Factory.Glo.DbconfigDataProxy.ClearCache();

            return r;
        }

        /// <summary> 
        /// 启用事务一次性删除多条数据。 
        /// </summary> 
        public bool Delete(IList<string> configkeys, IList<string> versions)
        {
            bool r = false;

            ContextDB.DoDBTrans(() =>
            {
                for (int i = 0; i < configkeys.Count; i++)
                {
                    dal.Delete(configkeys[i], versions[i]);
                }
                r = true;
            });

            //清除缓存。
            Factory.Glo.DbconfigDataProxy.ClearCache();

            return r;
        }

        #endregion

    }
}
