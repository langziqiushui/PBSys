using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;

using YX.Core;

namespace YX.SqlserverDAL 
{
    /// <summary>
    /// 旅游线路数据访问对象的基类。
    /// </summary>
    public abstract class AbstractDAL : BaseAbstractDAL
    {
        public AbstractDAL() { }        

        #region 执行数据访问的方法

        /// <summary>
        /// 以ExecuteNonQuery方式执行，优先查找上下文可用的数据库事务，次之查找上下文数据库连接，如果都没有则抛出异常。
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parms">sql参数集</param>
        /// <returns></returns>
        protected int ExecuteNonQueryWithContextDBTran(string sql, SqlParameter[] parms)
        {
            if (null != ContextDBPool.ContextTran)
                return SqlHelper.ExecuteNonQuery((SqlTransaction)ContextDBPool.ContextTran, CommandType.StoredProcedure, sql, parms);

            if (null != ContextDBPool.ContextDBConn)
                return SqlHelper.ExecuteNonQuery((SqlConnection)ContextDBPool.ContextDBConn, CommandType.StoredProcedure, sql, parms);

            throw new Exception("对不起，当前没有可用的上下文事务，也没有可用的上下文数据库连接对象。");
        }

        /// <summary>
        /// 以ExecuteScalar方式执行，优先查找上下文可用的数据库事务，次之查找上下文数据库连接，如果都没有则抛出异常。
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parms">sql参数集</param>
        /// <returns></returns>
        protected object ExecuteScalarWithContextDBTran(string sql, SqlParameter[] parms)
        {
            if (null != ContextDBPool.ContextTran)
                return SqlHelper.ExecuteScalar((SqlTransaction)ContextDBPool.ContextTran, CommandType.StoredProcedure, sql, parms);

            if (null != ContextDBPool.ContextDBConn)
                return SqlHelper.ExecuteScalar((SqlConnection)ContextDBPool.ContextDBConn, CommandType.StoredProcedure, sql, parms);

            throw new Exception("对不起，当前没有可用的上下文事务，也没有可用的上下文数据库连接对象。");
        }

        /// <summary>
        /// 以ExecuteScalar方式执行，优先查找上下文可用的数据库事务，次之查找上下文数据库连接，如果都没有则抛出异常。
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parms">sql参数集</param>
        /// <returns></returns>
        protected SqlDataReader ExecuteReaderWithContextDBTran(string sql, SqlParameter[] parms)
        {
            if (null != ContextDBPool.ContextTran)
                return SqlHelper.ExecuteReader(((SqlTransaction)ContextDBPool.ContextTran), CommandType.StoredProcedure, sql, parms);

            if (null != ContextDBPool.ContextDBConn)
                return SqlHelper.ExecuteReader((SqlConnection)ContextDBPool.ContextDBConn, CommandType.StoredProcedure, sql, parms);

            throw new Exception("对不起，当前没有可用的上下文事务，也没有可用的上下文数据库连接对象。");
        }

        /// <summary>
        /// 返回一个DataSet
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parms">sql参数集</param>
        /// <returns></returns>
        protected DataSet ExecuteDataSetWithContentDBTran(string sql, SqlParameter[] parms)
        {
            if (null != ContextDBPool.ContextTran)
                return SqlHelper.ExecuteDataset(((SqlTransaction)ContextDBPool.ContextTran), CommandType.StoredProcedure, sql, parms);

            if (null != ContextDBPool.ContextDBConn)
                return SqlHelper.ExecuteDataset((SqlConnection)ContextDBPool.ContextDBConn, CommandType.StoredProcedure, sql, parms);

            throw new Exception("对不起，当前没有可用的上下文事务，也没有可用的上下文数据库连接对象。");
        }

        #endregion
    }   
}
