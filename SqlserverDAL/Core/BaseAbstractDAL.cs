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
    /// 所有数据访问对象的基类。
    /// </summary>
    public abstract class BaseAbstractDAL : IDisposable
    {
        /// <summary>
        /// 是否已经执行过释放资源的操作。
        /// </summary>
        protected bool disposed = false;

        public BaseAbstractDAL() { }        

        #region IDisposable 成员

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源。
        /// </summary>
        /// <param name="disposing">是否释放资源</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    //此处释放托管资源。
                    //......
                }

                // 在此处释放非托管资源。
                // ......
            }

            this.disposed = true;
        }

        #endregion
    }   
}
