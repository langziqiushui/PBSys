// ===============================================================================
// Powered by：.net项目开发工具(V3.3.00.00)  
// Author：有容乃大，Hxw (mrhgw@sohu.com;Http://mrhgw.cnblogs.com) 
// Date：2017/3/2 15:14:15 
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
    /// 提供数据查询查关功能(对应表 Glo.DBLog )。
    /// </summary> 
    public partial class DblogDAL : AbstractDAL
    {
        public DblogDAL() : base() { }

        #region 获取数据

        /// <summary> 
        /// 分页获取。 
        /// </summary> 
        /// <param name="pagesize">单页显示的记录数</param> 
        /// <param name="pageindex">当前页码</param> 
        /// <param name="numrecord">所有符合查询条件的总记录数</param> 
        /// <param name="applicationtype">应用程序类别[ApplicationTypes]  1= yx  2=anxiu 3=pp8 4=...</param> 
        /// <param name="logtype">日志类别[LogTypes]400 = 0,500 = 1, 自定义=2</param> 
        /// <param name="restype">类别类别[Restypes]M站 = 0,WEB站 = 1, 后台=2, APP = 3, 接口=4</param> 
        /// <param name="keywords">搜索关键词</param> 
        /// <param name="startdate">起始日期</param> 
        /// <param name="enddate">载止日期</param> 
        /// <param name="version">版本号</param> 
        /// <returns>查询获取的数据集</returns> 
        public IList<DblogInfo> GetData_Paging(int? pagesize, int? pageindex, ref int numrecord, byte? applicationtype, byte? logtype, byte? restype, string keywords, DateTime? startdate, DateTime? enddate, string version)
        {
            IDataReader reader = null;
            var parms = new SqlParameterList();
            IList<DblogInfo> entities = null;

            parms.Add(SqlUtil.CreateParameter("@pageSize", DbType.Int32, pagesize));
            parms.Add(SqlUtil.CreateParameter("@pageIndex", DbType.Int32, pageindex));
            parms.Add(SqlUtil.CreateOutputParameter("@numRecord", DbType.Int32, 4));
            parms.Add(SqlUtil.CreateParameter("@ApplicationType", DbType.Byte, applicationtype));
            parms.Add(SqlUtil.CreateParameter("@LogType", DbType.Byte, logtype));
            parms.Add(SqlUtil.CreateParameter("@Restype", DbType.Byte, restype));
            parms.Add(SqlUtil.CreateParameter("@Keywords", DbType.String, keywords));
            parms.Add(SqlUtil.CreateParameter("@StartDate", DbType.DateTime, startdate));
            parms.Add(SqlUtil.CreateParameter("@EndDate", DbType.DateTime, enddate));
            parms.Add(SqlUtil.CreateParameter("@Version", DbType.String, version));

            using (reader = this.ExecuteReaderWithContextDBTran("[Glo].[UP_Sel_DBLog_Paging]", parms.ToArray()))
            {
                entities = DomainUtil.PopulateDataList<DblogInfo>(reader);
            }

            #region 存储过程输出参数 

            if (DBNull.Value != parms["@numRecord"].Value)
                numrecord = Convert.ToInt32(parms["@numRecord"].Value);

            #endregion 存储过程输出参数 

            return entities;
        }

        /// <summary> 
        /// 根据主键获取某一系统日志表。 
        /// </summary> 
        /// <param name="dblogid">dblogid</param> 
        /// <returns>查询获取的数据</returns> 
        public DblogInfo GetData(int? dblogid)
        {
            IDataReader reader = null;
            DblogInfo entity = null;
            var parms = new SqlParameterList();

            parms.Add(SqlUtil.CreateParameter("@DBLogID", DbType.Int32, dblogid));

            using (reader = this.ExecuteReaderWithContextDBTran("[Glo].[UP_Sel_DBLog_PrimaryKey]", parms.ToArray()))
            {
                entity = DomainUtil.PopulateData<DblogInfo>(reader);
            }
            return entity;
        }

        #endregion

        #region 添加数据

        /// <summary> 
        /// 创建。 
        /// </summary> 
        /// <param name="entity">DblogInfo业务实体对象</param> 
        /// <param name="dblogid">插入对象的自动递增编号</param> 
        /// <returns>数据添加是否成功</returns> 
        public object Add(DblogInfo entity, ref int dblogid)
        {
            var parms = new SqlParameterList();

            parms.Add(SqlUtil.CreateParameter("@ApplicationType", DbType.Byte, entity.ApplicationType));
            parms.Add(SqlUtil.CreateParameter("@LogType", DbType.Byte, entity.LogType));
            parms.Add(SqlUtil.CreateParameter("@PrimaryKeyData", DbType.String, entity.PrimaryKeyData));
            parms.Add(SqlUtil.CreateParameter("@Subject", DbType.String, entity.Subject));
            parms.Add(SqlUtil.CreateParameter("@LogContent", DbType.String, entity.LogContent));
            parms.Add(SqlUtil.CreateParameter("@Restype", DbType.Byte, entity.Restype));
            parms.Add(SqlUtil.CreateParameter("@Resurl", DbType.String, entity.Resurl));
            parms.Add(SqlUtil.CreateParameter("@ResBrowser", DbType.String, entity.ResBrowser));
            parms.Add(SqlUtil.CreateParameter("@ResPlatform", DbType.String, entity.ResPlatform));
            parms.Add(SqlUtil.CreateParameter("@ResSource", DbType.String, entity.ResSource));
            parms.Add(SqlUtil.CreateParameter("@ResIP", DbType.String, entity.ResIP));
            parms.Add(SqlUtil.CreateParameter("@CreateTime", DbType.DateTime, entity.CreateTime));
            parms.Add(SqlUtil.CreateParameter("@Version", DbType.String, entity.Version));
            parms.Add(SqlUtil.CreateOutputParameter("@DBLogID", DbType.Int32, 4));

            object r = this.ExecuteScalarWithContextDBTran("[Glo].[UP_Ins_DBLog]", parms.ToArray());

            #region 存储过程输出参数 

            if (DBNull.Value != parms["@DBLogID"].Value)
                dblogid = Convert.ToInt32(parms["@DBLogID"].Value);

            #endregion 存储过程输出参数 

            return r;
        }

        #endregion

        #region 删除数据

        /// <summary> 
        /// 根据主键删除某一系统日志表。 
        /// </summary> 
        /// <param name="dblogid">dblogid</param> 
        /// <returns>数据删除是否成功</returns> 
        public object Delete(int? dblogid)
        {
            var parms = new SqlParameterList();

            parms.Add(SqlUtil.CreateParameter("@DBLogID", DbType.Int32, dblogid));

            object r = this.ExecuteScalarWithContextDBTran("[Glo].[UP_Del_DBLog_PrimaryKey]", parms.ToArray());
            return r;
        }

        /// <summary> 
        /// 删除某个类别的日志或清空所有日志。 
        /// </summary> 
        /// <param name="logtype">日志类别[LogTypes]400 = 0,500 = 1, 自定义=2 (如果为null，则清空所有日志;否则删除当前类别下日志)</param> 
        /// <returns>数据删除是否成功</returns> 
        public object Delete_LogType(byte? logtype)
        {
            var parms = new SqlParameterList();

            parms.Add(SqlUtil.CreateParameter("@LogType", DbType.Byte, logtype));

            object r = this.ExecuteScalarWithContextDBTran("[Glo].[UP_Del_DBLog_LogType]", parms.ToArray());
            return r;
        }

        /// <summary> 
        /// 定期删除系统日志。 
        /// </summary> 
        /// <returns>数据删除是否成功</returns> 
        public object Delete_Regular()
        {
            var parms = new SqlParameterList();

            object r = this.ExecuteScalarWithContextDBTran("[Glo].[UP_Del_DBLog_Regular]", parms.ToArray());
            return r;
        }

        #endregion

    }
}
