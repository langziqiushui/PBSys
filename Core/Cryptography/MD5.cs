using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace YX.Core
{
    /// <summary>
    /// MD5加密算法(不可逆)。
    /// </summary>
    public class MD5 : ICryptography
    {
        #region ICryptography 成员

        /// <summary>
        /// 加密。
        /// </summary>
        /// <param name="text">待加密的文本</param>
        /// <param name="__key">为了继承接口方法必须有此参数，请直接设为null</param>
        /// <param name="__iv">为了继承接口方法必须有此参数，请直接设为null</param>
        /// <returns>加密后的文本</returns>
        public string Encrypt(string text, string __key, string __iv)
        {
            //先MD5加密。
            byte[] b = System.Text.Encoding.Default.GetBytes(text);
            b = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(b);
            var sb = new StringBuilder();
            for (int i = 0; i < b.Length; i++)
                sb.Append(b[i].ToString("x").PadLeft(2, '0'));

            return sb.ToString();
        }

        /// <summary>
        /// 解密。
        /// </summary>
        /// <param name="text">待解密的文本</param>
        /// <param name="__key">为了继承接口方法必须有此参数，请直接设为null</param>
        /// <param name="__iv">为了继承接口方法必须有此参数，请直接设为null</param>
        /// <returns>解密后的文本</returns>
        public string Decrypt(string text, string __key, string __iv)
        {
            return text;
        }

        /// <summary>
        /// 获取 当前加密算法是否可以争密。
        /// </summary>
        public bool CanDecrypt
        {
            get { return false; }
        }

        #endregion       
    }
}
