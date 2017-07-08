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
    /// ������·���ݷ��ʶ���Ļ��ࡣ
    /// </summary>
    public abstract class AbstractDAL : BaseAbstractDAL
    {
        public AbstractDAL() { }        

        #region ִ�����ݷ��ʵķ���

        /// <summary>
        /// ��ExecuteNonQuery��ʽִ�У����Ȳ��������Ŀ��õ����ݿ����񣬴�֮�������������ݿ����ӣ������û�����׳��쳣��
        /// </summary>
        /// <param name="sql">sql���</param>
        /// <param name="parms">sql������</param>
        /// <returns></returns>
        protected int ExecuteNonQueryWithContextDBTran(string sql, SqlParameter[] parms)
        {
            if (null != ContextDBPool.ContextTran)
                return SqlHelper.ExecuteNonQuery((SqlTransaction)ContextDBPool.ContextTran, CommandType.StoredProcedure, sql, parms);

            if (null != ContextDBPool.ContextDBConn)
                return SqlHelper.ExecuteNonQuery((SqlConnection)ContextDBPool.ContextDBConn, CommandType.StoredProcedure, sql, parms);

            throw new Exception("�Բ��𣬵�ǰû�п��õ�����������Ҳû�п��õ����������ݿ����Ӷ���");
        }

        /// <summary>
        /// ��ExecuteScalar��ʽִ�У����Ȳ��������Ŀ��õ����ݿ����񣬴�֮�������������ݿ����ӣ������û�����׳��쳣��
        /// </summary>
        /// <param name="sql">sql���</param>
        /// <param name="parms">sql������</param>
        /// <returns></returns>
        protected object ExecuteScalarWithContextDBTran(string sql, SqlParameter[] parms)
        {
            if (null != ContextDBPool.ContextTran)
                return SqlHelper.ExecuteScalar((SqlTransaction)ContextDBPool.ContextTran, CommandType.StoredProcedure, sql, parms);

            if (null != ContextDBPool.ContextDBConn)
                return SqlHelper.ExecuteScalar((SqlConnection)ContextDBPool.ContextDBConn, CommandType.StoredProcedure, sql, parms);

            throw new Exception("�Բ��𣬵�ǰû�п��õ�����������Ҳû�п��õ����������ݿ����Ӷ���");
        }

        /// <summary>
        /// ��ExecuteScalar��ʽִ�У����Ȳ��������Ŀ��õ����ݿ����񣬴�֮�������������ݿ����ӣ������û�����׳��쳣��
        /// </summary>
        /// <param name="sql">sql���</param>
        /// <param name="parms">sql������</param>
        /// <returns></returns>
        protected SqlDataReader ExecuteReaderWithContextDBTran(string sql, SqlParameter[] parms)
        {
            if (null != ContextDBPool.ContextTran)
                return SqlHelper.ExecuteReader(((SqlTransaction)ContextDBPool.ContextTran), CommandType.StoredProcedure, sql, parms);

            if (null != ContextDBPool.ContextDBConn)
                return SqlHelper.ExecuteReader((SqlConnection)ContextDBPool.ContextDBConn, CommandType.StoredProcedure, sql, parms);

            throw new Exception("�Բ��𣬵�ǰû�п��õ�����������Ҳû�п��õ����������ݿ����Ӷ���");
        }

        /// <summary>
        /// ����һ��DataSet
        /// </summary>
        /// <param name="sql">sql���</param>
        /// <param name="parms">sql������</param>
        /// <returns></returns>
        protected DataSet ExecuteDataSetWithContentDBTran(string sql, SqlParameter[] parms)
        {
            if (null != ContextDBPool.ContextTran)
                return SqlHelper.ExecuteDataset(((SqlTransaction)ContextDBPool.ContextTran), CommandType.StoredProcedure, sql, parms);

            if (null != ContextDBPool.ContextDBConn)
                return SqlHelper.ExecuteDataset((SqlConnection)ContextDBPool.ContextDBConn, CommandType.StoredProcedure, sql, parms);

            throw new Exception("�Բ��𣬵�ǰû�п��õ�����������Ҳû�п��õ����������ݿ����Ӷ���");
        }

        #endregion
    }   
}
