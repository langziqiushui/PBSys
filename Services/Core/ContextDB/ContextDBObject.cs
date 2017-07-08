using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;

using YX.Core;

namespace YX.Services
{
    /// <summary>
    /// 数据访问上下文处理对象。
    /// </summary>
    public class ContextDBObject
    {
        #region 构造器

        /// <summary>
        /// 构造器。
        /// </summary>
        /// <param name="contextDBPoolObject">数据库连接上下文共享池对象</param>
        /// <param name="funCreateDbConnection">创建数据库连接对象的方法</param>
        public ContextDBObject(ContextDBPoolObject contextDBPoolObject, Func<DbConnection> funCreateDbConnection)
        {
            this.contextDBPoolObject = contextDBPoolObject;
            this.funCreateDbConnection = funCreateDbConnection;
        }

        #endregion

        #region 变量

        /// <summary>
        /// 数据库连接上下文共享池对象。
        /// </summary>
        private ContextDBPoolObject contextDBPoolObject = null;
        /// <summary>
        /// 创建数据库连接对象的方法。
        /// </summary>
        private Func<DbConnection> funCreateDbConnection = null;
        /// <summary>
        /// 已经启用事务的数据库连接对象集合。
        /// </summary>
        private Dictionary<int, DbConnection> _contextUseDBTranConns = new Dictionary<int, DbConnection>();
        /// <summary>
        /// 并发锁。
        /// </summary>
        private readonly object LOCK_ContextDB = new object();

        #endregion

        #region 获取或创建上下文数据库连接

        /// <summary>
        /// 获取或创建上下文数据库连接。
        /// </summary>
        /// <returns></returns>
        public DbConnection CreateDBConnection()
        {
            lock (this.LOCK_ContextDB)
            {
                int id = System.Threading.Thread.CurrentThread.ManagedThreadId;

                //先查找上下文事务。
                if (this.contextDBPoolObject.ContextDBTrans.ContainsKey(id))
                    return null;
                //查找上下文连接。
                if (this.contextDBPoolObject.ContextDBDConns.ContainsKey(id))
                    return null;

                //创建新连接。
                var newDBConn = this.funCreateDbConnection();
                //打开数据库连接。
                if (newDBConn.State != ConnectionState.Open)
                    newDBConn.Open();
                //附加数据库连接秋放事件。
                newDBConn.Disposed += new EventHandler(ContextDBConn_Disposed);
                //加入集合中备共享。
                this.contextDBPoolObject.ContextDBDConns.Add(id, newDBConn);

                return newDBConn;
            }
        }

        void ContextDBConn_Disposed(object sender, EventArgs e)
        {
            lock (this.LOCK_ContextDB)
            {
                this.contextDBPoolObject.ContextDBDConns.Remove(System.Threading.Thread.CurrentThread.ManagedThreadId);
            }
        }

        #endregion

        #region 获取或创建上下文事务

        /// <summary>
        /// 创建当前上下文数据库事务对象(默认事务隔离级别为：ReadUncommitted)。
        /// </summary>
        /// <returns>创建事务是否成功(如果当前线程中有未回收的事务，则直接获取该事务返回flase; 否则创建新事务并返回true)</returns>
        public DbTransaction CreateDBTran()
        {
            return this.CreateDBTran(IsolationLevel.ReadUncommitted);
        }

        /// <summary>
        /// 创建当前上下文数据库事务对象。
        /// </summary>
        /// <param name="iso">事务应在其下运行的隔离级别</param>
        /// <returns>创建事务是否成功(如果当前线程中有未回收的事务，则直接获取该事务返回flase; 否则创建新事务并返回true)</returns>
        public DbTransaction CreateDBTran(IsolationLevel? iso)
        {
            lock (this.LOCK_ContextDB)
            {
                int id = System.Threading.Thread.CurrentThread.ManagedThreadId;

                //检查当前线程中是否有可用的事务，如果有直接返回。
                if (this.contextDBPoolObject.ContextDBTrans.ContainsKey(id))
                    return null;

                //创建新连接并加入共享集合中。
                var newDBConn = this.funCreateDbConnection();
                //移除上一次未清除的数据连接对象。
                this._contextUseDBTranConns.Remove(id);
                //加入新的数据连接对象。
                this._contextUseDBTranConns.Add(id, newDBConn);

                //附加数据库连接释放事件。
                newDBConn.Disposed += new EventHandler(ContextUseTranDBConn_Disposed);
                //打开数据库连接。
                if (newDBConn.State != ConnectionState.Open)
                    newDBConn.Open();

                //启用事务并加入共享集合中。
                DbTransaction newDBTran = null == iso ? newDBConn.BeginTransaction() : newDBConn.BeginTransaction(iso.Value);
                this.contextDBPoolObject.ContextDBTrans.Add(id, newDBTran);

                return newDBTran;
            }
        }

        /// <summary>
        /// 关闭当前上下文已启用事务的数据库连接。
        /// </summary>
        public void CloseContextDBTranConn()
        {
            lock (this.LOCK_ContextDB)
            {
                int id = System.Threading.Thread.CurrentThread.ManagedThreadId;

                if (this._contextUseDBTranConns.ContainsKey(id))
                {
                    DbConnection conn = this._contextUseDBTranConns[id];
                    conn.Dispose();
                }
            }
        }

        /// <summary>
        /// 当数据库连接被释放时。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ContextUseTranDBConn_Disposed(object sender, EventArgs e)
        {
            lock (this.LOCK_ContextDB)
            {
                int id = System.Threading.Thread.CurrentThread.ManagedThreadId;
                this._contextUseDBTranConns.Remove(id);
                this.contextDBPoolObject.ContextDBTrans.Remove(id);
            }
        }

        #endregion

        #region 封装一个启用数据库事务的方法

        /// <summary>
        /// 封装一个启用数据库事务的方法。
        /// </summary>
        /// <param name="fun">启用事务后执行的方法</param>
        /// <param name="funError">发生异常时执行的方法</param>
        /// <param name="funFinaly">最后执行的方法</param>
        public void DoDBTrans(Action fun, Action<Exception> funError, Action funFinaly)
        {
            DbTransaction newDBTran = null;

            try
            {
                /*-----------------------------------------------------------------------
                 * 以下为创建数据库事务，请注意：
                 *  1. 当newDBTran不为null时，表示已经创建一个新的事务。 
                 *  2. 当newDBTran不为null时，说明当前线程中已经创建了事务并且没有被回收，则直接使用该事务。
                 *  3. 当newDBTran不为null时(使用以前创建的事务)，则在此方法体中不提交事务(完成操作)，不回滚事务(出错)，不释放数据库连接。
                 -----------------------------------------------------------------------*/
                newDBTran = this.CreateDBTran();

                //执行启用事务后的相关操作。
                if (null != fun)
                    fun();

                //提交事务。
                if (null != newDBTran)
                    newDBTran.Commit();
            }
            catch (Exception ex)
            {
                //发生异常，回滚事务。
                if (null != newDBTran)
                    newDBTran.Rollback();

                //执行发生异常时的相关操作。
                if (null != funError)
                    funError(ex);
                throw;
            }
            finally
            {
                if (null != newDBTran)
                {
                    CloseContextDBTranConn();
                    newDBTran = null;
                }

                //执行最后的相关操作。
                if (null != funFinaly)
                    funFinaly();
            }
        }

        #endregion

        #region 封装创建共享数据库连接的方法

        /// <summary>
        /// 封装创建共享数据库连接的方法。
        /// </summary>
        /// <param name="fun">创建数据库连接后执行的方法</param>
        /// <param name="funError">发生异常时执行的方法</param>
        /// <param name="funFinaly">最后执行的方法</param>
        public void DoDBExec(Action fun, Action<Exception> funError, Action funFinaly)
        {
            if (null == fun)
                return;

            //创建上下文事务。
            DbConnection newDBConn = null;

            try
            {
                //如果创建成功，则在执行特定方法后释放连接。
                //创建不成功(表示已经存在上下文连接)，则执行方法并且使用上下文连接。
                newDBConn = this.CreateDBConnection();
                fun();
            }
            catch (Exception ex)
            {
                //执行发生异常时的相关操作。
                if (null != funError)
                    funError(ex);
                throw;
            }
            finally
            {
                if (null != newDBConn)
                {
                    newDBConn.Dispose();
                    newDBConn = null;
                }

                //执行最后的相关操作。
                if (null != funFinaly)
                    funFinaly();
            }
        }

        #endregion
    }
}
