using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.ComponentModel;
using System.Runtime.Serialization.Formatters.Binary;

using YX.Core;

namespace YX.Core
{
    /// <summary>
    /// 业务实体基类对象。
    /// </summary>
    /// <remarks>
    /// 功能描述：
    ///     1.继承了ICloneable接口。
    ///     2.提供了描述实体对象状态的属性。
    ///     3.获取或设置对象标签。
    ///     4.提供了标记业务实体对象状态的方法。
    ///     5.提交业务实体对象的更改。
    ///     6.克隆对象。
    /// </remarks>
    [Serializable]
    public abstract class BaseDomain : IDisposable, ICloneable
    {
        #region 变量

        /// <summary>
        /// 表示资源是否已经释放。
        /// </summary>
        protected bool disposed = false;

        #endregion

        #region 属性

        /// <summary>
        /// 获取或设置 业务实体对象的标签。
        /// </summary>
        [Browsable(false)]
        public object Tag { get; set; }

        #endregion

        #region 公用方法

        /// <summary>
        /// 呈视完整文本。
        /// </summary>
        /// <returns></returns>
        public virtual string ToFullString()
        {
            var sb = new StringBuilder();

            foreach (PropertyInfo _pi in this.GetType().GetProperties())
            {
                object o = _pi.GetValue(this, null);
                if (null == o)
                    continue;

                sb.Append(_pi.Name);
                sb.Append("：");
                sb.Append(o.ToString());
                sb.Append("\n");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 汉字转换为Unicode编码
        /// </summary>
        /// <param name="str">要编码的汉字字符串</param>
        /// <returns>Unicode编码的的字符串</returns>
        public static string ToUnicode(string str)
        {
            byte[] bts = Encoding.Unicode.GetBytes(str);
            string r = "";
            for (int i = 0; i < bts.Length; i += 2) r += "\\u" + bts[i + 1].ToString("x").PadLeft(2, '0') + bts[i].ToString("x").PadLeft(2, '0');
            return r;
        }

        /// <summary>
        /// 呈现Json字符串。
        /// </summary>
        /// <param name="isUnicode">是否转换为Unicode字符</param>
        /// <returns></returns>
        public virtual string ToJson(bool isUnicode)
        {
            var sb = new StringBuilder();
            foreach (PropertyInfo _pi in this.GetType().GetProperties())
            {
                var t = _pi.PropertyType.Name;
                var o = _pi.GetValue(this, null);

                sb.Append(_pi.Name);
                sb.Append(":'");
                if (t == "String")
                    sb.Append(null == o ? string.Empty : CoreUtil.QuotationTransferred(isUnicode ? ToUnicode(o.ToString()) : o.ToString()));
                else
                    sb.Append(null == o ? string.Empty : CoreUtil.QuotationTransferred(o.ToString()));
                sb.Append("',");
            }
            if (sb.Length > 0)
                sb = sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        #endregion

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

        #region ICloneable 成员

        /// <summary>
        /// 创建当前业务实体对象的一个副本。
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return EntityCloner.Clone(this);
        }

        /// <summary>
        /// 创建当前业务实体对象的一个副本。
        /// </summary>
        /// <returns></returns>
        public T Clone<T>() where T : BaseDomain
        {
            return DomainUtil.CloneFromBaseOjbect<T>(this);
        }

        #endregion
    }
}
