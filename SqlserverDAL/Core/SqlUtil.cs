using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace YX.SqlserverDAL
{
    /// <summary>
    /// </summary>
    public static class SqlUtil
    {
        /// <summary>
        /// 静态构造器。
        /// </summary>
        static SqlUtil()
        {
            //获取OA数据库连接字符串。
            _yunMallDBConnection = System.Configuration.ConfigurationManager.ConnectionStrings["default"].ConnectionString;
        }

        #region 变量

        /// <summary>
        /// 旅游线路数据库连接字符串。
        /// </summary>
        private static string _yunMallDBConnection = string.Empty;      

        #endregion

        #region 属性
       
        #endregion

        #region 公用方法

        /// <summary>
        /// 创建OA数据库连接对象。
        /// </summary>
        /// <returns></returns>
        public static SqlConnection CreateDBConnection()
        {
            SqlConnection conn = new SqlConnection(_yunMallDBConnection);
            conn.Open();
            return conn;
        }      

        /// <summary>
        /// 创建SqlParameter参数对象。
        /// </summary>
        /// <param name="parmName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns></returns>
        public static SqlParameter CreateParameter(string parmName, object value)
        {
            return new SqlParameter() { ParameterName = parmName, Value = null == value ? DBNull.Value : value };
        }

        /// <summary>
        /// 创建SqlParameter参数对象。
        /// </summary>
        /// <param name="parmName">参数名称</param>
        /// <param name="type">表示参数的DbType类型</param>
        /// <param name="value">参数值</param>
        /// <returns></returns>
        public static SqlParameter CreateParameter(string parmName, DbType type, object value)
        {
            return new SqlParameter() { ParameterName = parmName, DbType = type, Value = null == value ? DBNull.Value : value };
        }

        /// <summary>
        /// 创建SqlParameter输入输出参数对象。
        /// </summary>
        /// <param name="parmName">参数名称</param>
        /// <param name="size">参数长度</param>
        /// <param name="value">参数值</param>
        /// <returns></returns>
        public static SqlParameter CreateInOutputParameter(string parmName, DbType dbType, int size, object value)
        {
            return new SqlParameter() { ParameterName = parmName, DbType = dbType, Direction = ParameterDirection.InputOutput, Size = size, Value = null == value ? DBNull.Value : value };
        }

        /// <summary>
        /// 创建SqlParameter输出参数对象。
        /// </summary>
        /// <param name="parmName">参数名称</param>
        /// <param name="dbType">DbType数据类型</param>
        /// <param name="size">参数长度</param>
        /// <returns></returns>
        public static SqlParameter CreateOutputParameter(string parmName, DbType dbType, int size)
        {
            SqlParameter parm = new SqlParameter() { ParameterName = parmName, Direction = ParameterDirection.Output, DbType = dbType, Size = size };
            return parm;
        }

        #endregion
    }
}
