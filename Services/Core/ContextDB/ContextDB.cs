using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;

using YX.Core;
using YX.SqlserverDAL;

namespace YX.Services
{
    /// <summary>
    /// 旅游线路数据访问上下文处理。
    /// </summary>
    public class ContextDB
    {
        #region 变量

        /// <summary>
        /// OA数据库连接上下文共享池对象。
        /// </summary>
        private static ContextDBObject contextDBObject = new ContextDBObject(ContextDBPool.ContextDBPoolObject, SqlUtil.CreateDBConnection);

        #endregion    

        #region 封装一个启用数据库事务的方法

        /// <summary>
        /// 封装一个启用数据库事务的方法。
        /// </summary>
        /// <param name="fun">启用事务后执行的方法</param>
        public static void DoDBTrans(Action fun)
        {
            contextDBObject.DoDBTrans(fun, null, null);
        }


        /// <summary>
        /// 封装一个启用数据库事务的方法。
        /// </summary>
        /// <param name="fun">启用事务后执行的方法</param>
        /// <param name="funError">发生异常时执行的方法</param>
        public static void DoDBTrans(Action fun, Action<Exception> funError)
        {
            contextDBObject.DoDBTrans(fun, funError, null);
        }

        /// <summary>
        /// 封装一个启用数据库事务的方法。
        /// </summary>
        /// <param name="fun">启用事务后执行的方法</param>
        /// <param name="funError">发生异常时执行的方法</param>
        /// <param name="funFinaly">最后执行的方法</param>
        public static void DoDBTrans(Action fun, Action<Exception> funError, Action funFinaly)
        {
            contextDBObject.DoDBTrans(fun, funError, funFinaly);            
        }

        #endregion

        #region 封装创建共享数据库连接的方法

         /// <summary>
        /// 封装创建共享数据库连接的方法。
        /// </summary>
        /// <param name="fun">创建数据库连接后执行的方法</param>
        public static void DoDBExec(Action fun)
        {
            contextDBObject.DoDBExec(fun, null, null);
        }

        /// <summary>
        /// 封装创建共享数据库连接的方法。
        /// </summary>
        /// <param name="fun">创建数据库连接后执行的方法</param>
        /// <param name="funError">发生异常时执行的方法</param>
        public static void DoDBExec(Action fun, Action<Exception> funError)
        {
            contextDBObject.DoDBExec(fun, funError, null);
        }

        /// <summary>
        /// 封装创建共享数据库连接的方法。
        /// </summary>
        /// <param name="fun">创建数据库连接后执行的方法</param>
        /// <param name="funError">发生异常时执行的方法</param>
        /// <param name="funFinaly">最后执行的方法</param>
        public static void DoDBExec(Action fun, Action<Exception> funError, Action funFinaly)
        {
            contextDBObject.DoDBExec(fun, funError, funFinaly);          
        }

        #endregion
    }
}
