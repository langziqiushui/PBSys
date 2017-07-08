namespace Common.DB
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Common;


    public interface IDBHelper
    {



        /// <summary>
        /// 创建查询参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        DbParameter CreateParameter(string name, object value);

        /// <summary>
        /// 创建查询参数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        DbParameter CreateParameter(string name);


        /// <summary>
        /// 创建查询参数
        /// </summary>
        /// <returns></returns>
        DbParameter CreateParameter();

        /// <summary>
        /// 复制参数
        /// </summary>
        /// <param name="dbps"></param>
        /// <returns></returns>
        DbParameter[] CopyParameters(DbParameter[] dbps);


        /// <summary>
        /// 获得一个值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        T ExecuteScalar<T>(string sql, params DbParameter[] parameters);

        /// <summary>
        /// 获得一个数据读取器
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        DbDataReader ExecuteReader(string sql, params DbParameter[] parameters);

        /// <summary>
        /// 执行一条语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        int ExecuteNoneQuery(string sql, params DbParameter[] parameters);


        /// <summary>
        /// 获得数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Hashtable GetData(string sql, params DbParameter[] parameters);

        /// <summary>
        /// 获得数据集
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        List<Hashtable> GetDataList(string sql, params DbParameter[] parameters);


        /// <summary>
        /// 创建数据库链接
        /// </summary>
        /// <returns></returns>
        DbConnection CreateConnection();

        /// <summary>
        /// 创建数据库命令
        /// </summary>
        /// <returns></returns>
        DbCommand CreateCommand();
    }


}