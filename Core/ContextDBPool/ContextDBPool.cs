using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace YX.Core
{
    /// <summary>
    /// 封装OA数据库连接上下文共享池对象。
    /// </summary>
    public static class ContextDBPool
    {
        #region 变量

        /// <summary>
        /// 数据库上下文共享对象。
        /// </summary>
        private static ContextDBPoolObject contextDBPoolObject = new ContextDBPoolObject();

        #endregion

        #region 属性

        /// <summary>
        /// 获取 默认的数据库连接上下文共享池对象。
        /// </summary>
        public static ContextDBPoolObject ContextDBPoolObject
        {
            get { return contextDBPoolObject; }
        }

        /// <summary>
        /// 获取 表示当前上下文数据库连接对象集合。
        /// </summary>
        public static Dictionary<int, DbConnection> ContextConns
        {
            get { return contextDBPoolObject.ContextDBDConns; }
        }

        /// <summary>
        /// 获取 表示上下文数据库事务集合。
        /// </summary>
        public static Dictionary<int, DbTransaction> ContextTrans
        {
            get { return contextDBPoolObject.ContextDBTrans; }
        }

        /// <summary>
        /// 获取 当前上下文数据库连接。
        /// </summary>
        public static DbConnection ContextDBConn
        {
            get
            {
                return contextDBPoolObject.ContextDBConn;               
            }
        }

        /// <summary>
        /// 获取 当前上下文事务。
        /// </summary>
        public static DbTransaction ContextTran
        {
            get
            {
                return contextDBPoolObject.ContextDBTran;
            }
        }

        #endregion
    }
}
