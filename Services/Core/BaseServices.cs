using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YX.Services
{
    /// <summary>
    /// 业务服务基类对象。
    /// </summary>
    public class BaseServices : IDisposable
    {
        /// <summary>
        /// 是否已经执行过释放资源的操作。
        /// </summary>
        protected bool disposed = false;

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
