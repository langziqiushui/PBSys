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
    /// �������ݷ��ʶ���Ļ��ࡣ
    /// </summary>
    public abstract class BaseAbstractDAL : IDisposable
    {
        /// <summary>
        /// �Ƿ��Ѿ�ִ�й��ͷ���Դ�Ĳ�����
        /// </summary>
        protected bool disposed = false;

        public BaseAbstractDAL() { }        

        #region IDisposable ��Ա

        /// <summary>
        /// �ͷ���Դ
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// �ͷ���Դ��
        /// </summary>
        /// <param name="disposing">�Ƿ��ͷ���Դ</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    //�˴��ͷ��й���Դ��
                    //......
                }

                // �ڴ˴��ͷŷ��й���Դ��
                // ......
            }

            this.disposed = true;
        }

        #endregion
    }   
}
