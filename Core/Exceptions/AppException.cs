using System;
using System.Collections.Generic;
using System.Text;

namespace YX.Core
{
    /// <summary>
    /// 自定义异常类。
    /// </summary>
    public class AppException : ApplicationException
    {
        #region 重载基类构造器

        /// <summary>
        /// 重载一。
        /// </summary>
        public AppException()
        {
            this._message = "发生未知错误";
        }

        /// <summary>
        /// 重载二。
        /// </summary>
        /// <param name="Message">错误消息</param>
        public AppException(string Message)
        {
            this._message = Message;
        }

        /// <summary>
        /// 重载三。
        /// </summary>
        /// <param name="Message">错误消息</param>
        /// <param name="ExceptionLevel">异常级别</param>
        public AppException(string Message, ExceptionLevels ExceptionLevel)
        {
            this._message = Message;
            this.ExceptionLevel = ExceptionLevel;
        }

        #endregion

        #region 变量

        /// <summary>
        /// 系统错误消息。
        /// </summary>
        protected string _message = string.Empty;

        #endregion

        #region 属性

        /// <summary>
        /// 获取 错误消息。
        /// </summary>
        public override string Message
        {
            get { return this._message; }
        }

        /// <summary>
        /// 获取或设置 异常级别。
        /// </summary>
        public ExceptionLevels ExceptionLevel { get; set; }      

        #endregion

        #region 静态方法

        /// <summary>
        /// 抛出自定义异常。
        /// </summary>
        /// <param name="message">错误消息</param>
        public static void ThrowException(string message)
        {
            throw new AppException(message, ExceptionLevels.Error);
        }

        /// <summary>
        /// 抛出警告异常。
        /// </summary>
        /// <param name="message">错误消息</param>
        public static void ThrowWaringException(string message)
        {
            throw new AppException(message, ExceptionLevels.Warning);
        }

        /// <summary>
        /// 抛出超级警告异常。
        /// </summary>
        /// <param name="message">错误消息</param>
        public static void ThrowSuperWarningException(string message)
        {
            throw new AppException(message, ExceptionLevels.SuperWarning);
        }

        #endregion
    }
}
