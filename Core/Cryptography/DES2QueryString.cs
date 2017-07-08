using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace YX.Core
{
    /// <summary>
    /// 专门用于加、解密网页查询参数的对象。
    /// </summary>
    public class DES2QueryString : ICryptography
    {
        /// <summary>
        /// 固定的加密字符串。
        /// </summary>
        private string key = "P0x(4Jw%wslo";

        /// <summary>
        /// 使用对称加密技术 DES 加密字符串。
        /// </summary>
        /// <param name="text">要加密的字符串。</param>
        /// <param name="__key">为了继承接口方法必须有此参数，请直接设为null</param>
        /// <param name="__iv">为了继承接口方法必须有此参数，请直接设为null</param>
        /// <returns>加密后的字符串</returns>
        public string Encrypt(string text, string __key, string __iv)
        {
            if (key.Length < 8)
                AppException.ThrowWaringException("对不起，加密密钥必须为8位。");
            if (key.Length > 8)
                key = key.Substring(0, 8);

            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            provider.Key = Encoding.UTF8.GetBytes(key);
            provider.IV = Encoding.UTF8.GetBytes(key);
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write);
            stream2.Write(bytes, 0, bytes.Length);
            stream2.FlushFinalBlock();
            StringBuilder builder = new StringBuilder();
            foreach (byte num in stream.ToArray())
            {
                builder.AppendFormat("{0:X2}", num);
            }
            builder.ToString();
            stream2.Close();
            stream.Close();
            return builder.ToString();
        }

        /// <summary>
        /// 使用对称加密技术 DES 解密字符串。
        /// </summary>
        /// <param name="text">要解密的字符串。</param>
        /// <param name="__key">为了继承接口方法必须有此参数，请直接设为null</param>
        /// <param name="__iv">为了继承接口方法必须有此参数，请直接设为null</param>
        /// <returns>解密后的字符串</returns>
        public string Decrypt(string text, string __key, string __iv)
        {
            if (key.Length < 8)
                AppException.ThrowWaringException("对不起，加密密钥必须为8位。");
            if (key.Length > 8)
                key = key.Substring(0, 8);

            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            byte[] buffer = new byte[text.Length / 2];
            for (int i = 0; i < (text.Length / 2); i++)
            {
                int num2 = Convert.ToInt32(text.Substring(i * 2, 2), 0x10);
                buffer[i] = (byte)num2;
            }
            provider.Key = Encoding.UTF8.GetBytes(key);
            provider.IV = Encoding.UTF8.GetBytes(key);
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(), CryptoStreamMode.Write);
            stream2.Write(buffer, 0, buffer.Length);
            stream2.FlushFinalBlock();
            StringBuilder builder = new StringBuilder();
            stream2.Close();
            stream.Close();
            return Encoding.UTF8.GetString(stream.ToArray());
        }

        /// <summary>
        /// 获取 当前加密算法是否可以争密。
        /// </summary>
        public bool CanDecrypt
        {
            get { return true; }
        }

    }
}

