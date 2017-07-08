// ===============================================================================
// Powered by：.net项目开发工具(V3.3.00.00)  
// Author：有容乃大，Hxw (mrhgw@sohu.com;Http://mrhgw.cnblogs.com) 
// Date：2017/3/2 15:14:15 
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
    /// 业务服务处理(对应表 Glo.DBLog )。 
    /// </summary> 
    public partial class DblogServices : BaseServices, IDblogServices
    {
        public DblogServices() : base() { }

        #region 变量

        /// <summary> 
        /// 数据访问对象(DblogDAL)。 
        /// </summary> 
        internal static readonly YX.SqlserverDAL.Glo.DblogDAL dal = new YX.SqlserverDAL.Glo.DblogDAL();

        #endregion

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
            IList<DblogInfo> r = default(IList<DblogInfo>);
            int __numrecord = default(int);

            ContextDB.DoDBExec(() =>
            {
                r = dal.GetData_Paging(pagesize, pageindex, ref __numrecord, applicationtype, logtype, restype, keywords, startdate, enddate, version);
            });

            numrecord = __numrecord;
            return r;
        }

        /// <summary> 
        /// 根据主键获取某一系统日志表。 
        /// </summary> 
        /// <param name="dblogid">dblogid</param> 
        /// <returns>查询获取的数据</returns> 
        public DblogInfo GetData(int? dblogid)
        {
            DblogInfo r = default(DblogInfo);

            ContextDB.DoDBExec(() =>
            {
                r = dal.GetData(dblogid);
            });

            return r;
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
            object r = default(object);
            int __dblogid = default(int);

            ContextDB.DoDBExec(() =>
            {
                r = dal.Add(entity, ref __dblogid);
            });

            dblogid = __dblogid;
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
            object r = default(object);

            ContextDB.DoDBExec(() =>
            {
                r = dal.Delete(dblogid);
            });

            return r;
        }

        /// <summary> 
        /// 删除某个类别的日志或清空所有日志。 
        /// </summary> 
        /// <param name="logtype">日志类别[LogTypes]400 = 0,500 = 1, 自定义=2 (如果为null，则清空所有日志;否则删除当前类别下日志)</param> 
        /// <returns>数据删除是否成功</returns> 
        public object Delete_LogType(byte? logtype)
        {
            object r = default(object);

            ContextDB.DoDBExec(() =>
            {
                r = dal.Delete_LogType(logtype);
            });

            return r;
        }

        /// <summary> 
        /// 定期删除系统日志。 
        /// </summary> 
        /// <returns>数据删除是否成功</returns> 
        public object Delete_Regular()
        {
            object r = default(object);

            ContextDB.DoDBExec(() =>
            {
                r = dal.Delete_Regular();
            });

            return r;
        }

        /// <summary> 
        /// 启用事务一次性删除多条数据。 
        /// </summary> 
        public bool Delete(params int[] dblogids)
        {
            bool r = false;

            ContextDB.DoDBTrans(() =>
            {
                for (int i = 0; i < dblogids.Length; i++)
                {
                    dal.Delete(dblogids[i]);
                }
                r = true;
            });

            return r;
        }

        #endregion

    }
}
