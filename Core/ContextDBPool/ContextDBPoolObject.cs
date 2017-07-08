using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace YX.Core
{
    /// <summary>
    /// 数据库连接上下文共享池对象。
    /// </summary>
    public class ContextDBPoolObject
    {     
        #region 变量

        /// <summary>
        /// 表示当前上下文数据库连接对象集合。
        /// </summary>
        private Dictionary<int, DbConnection> _contextDBConns = new Dictionary<int, DbConnection>();
        /// <summary>
        /// 表示上下文数据库事务集合(集合键为当前访问者的线程编号)。
        /// </summary>
        private Dictionary<int, DbTransaction> _contextDBTrans = new Dictionary<int, DbTransaction>();
        /// <summary>
        /// 并发锁。
        /// </summary>
        private readonly object LOCK_ContextDBPool = new object();

        #endregion

        #region 属性

        /// <summary>
        /// 获取 表示当前上下文数据库连接对象集合。
        /// </summary>
        public Dictionary<int, DbConnection> ContextDBDConns
        {
            get { return this._contextDBConns; }
        }

        /// <summary>
        /// 获取 表示上下文数据库事务集合。
        /// </summary>
        public Dictionary<int, DbTransaction> ContextDBTrans
        {
            get { return this._contextDBTrans; }
        }

        /// <summary>
        /// 获取 当前上下文数据库连接。
        /// </summary>
        public DbConnection ContextDBConn
        {
            get
            {
                lock (this.LOCK_ContextDBPool)
                {
                    int id = System.Threading.Thread.CurrentThread.ManagedThreadId;

                    if (this._contextDBConns.ContainsKey(id))
                        return this._contextDBConns[id];

                    return null;
                }
            }
        }

        /// <summary>
        /// 获取 当前上下文事务。
        /// </summary>
        public DbTransaction ContextDBTran
        {
            get
            {
                lock (this.LOCK_ContextDBPool)
                {
                    int id = System.Threading.Thread.CurrentThread.ManagedThreadId;

                    if (this._contextDBTrans.ContainsKey(id))
                        return this._contextDBTrans[id];

                    return null;
                }
            }
        }

        #endregion
    }
}
