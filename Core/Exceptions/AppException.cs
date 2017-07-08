using System;
using System.Collections.Generic;
using System.Text;

namespace YX.Core
{
    /// <summary>
    /// �Զ����쳣�ࡣ
    /// </summary>
    public class AppException : ApplicationException
    {
        #region ���ػ��๹����

        /// <summary>
        /// ����һ��
        /// </summary>
        public AppException()
        {
            this._message = "����δ֪����";
        }

        /// <summary>
        /// ���ض���
        /// </summary>
        /// <param name="Message">������Ϣ</param>
        public AppException(string Message)
        {
            this._message = Message;
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="Message">������Ϣ</param>
        /// <param name="ExceptionLevel">�쳣����</param>
        public AppException(string Message, ExceptionLevels ExceptionLevel)
        {
            this._message = Message;
            this.ExceptionLevel = ExceptionLevel;
        }

        #endregion

        #region ����

        /// <summary>
        /// ϵͳ������Ϣ��
        /// </summary>
        protected string _message = string.Empty;

        #endregion

        #region ����

        /// <summary>
        /// ��ȡ ������Ϣ��
        /// </summary>
        public override string Message
        {
            get { return this._message; }
        }

        /// <summary>
        /// ��ȡ������ �쳣����
        /// </summary>
        public ExceptionLevels ExceptionLevel { get; set; }      

        #endregion

        #region ��̬����

        /// <summary>
        /// �׳��Զ����쳣��
        /// </summary>
        /// <param name="message">������Ϣ</param>
        public static void ThrowException(string message)
        {
            throw new AppException(message, ExceptionLevels.Error);
        }

        /// <summary>
        /// �׳������쳣��
        /// </summary>
        /// <param name="message">������Ϣ</param>
        public static void ThrowWaringException(string message)
        {
            throw new AppException(message, ExceptionLevels.Warning);
        }

        /// <summary>
        /// �׳����������쳣��
        /// </summary>
        /// <param name="message">������Ϣ</param>
        public static void ThrowSuperWarningException(string message)
        {
            throw new AppException(message, ExceptionLevels.SuperWarning);
        }

        #endregion
    }
}
