using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace YX.Core
{
    /// <summary>
    /// 增强型SHA256加密算法(不可逆)。
    /// </summary>
    public class StrongSHA256 : ICryptography
    {
        /// <summary>
        /// 加密。
        /// </summary>
        /// <param name="text">待加密的文本</param>
        /// <param name="__key">为了继承接口方法必须有此参数，请直接设为null</param>
        /// <param name="__iv">为了继承接口方法必须有此参数，请直接设为null</param>
        /// <returns>加密后的文本</returns>
        public string Encrypt(string text, string __key, string __iv)
        {
            //先对密码进行DES加密。
            text = DESEncrypt(text, MD5(text));
            var data = new UnicodeEncoding().GetBytes(text);
            var result = new SHA256Managed().ComputeHash(data);
            return BitConverter.ToString(result);
        }

        /// <summary>
        /// 解密。
        /// </summary>
        /// <param name="text">待解密的文本</param>
        /// <param name="__key">为了继承接口方法必须有此参数，请直接设为null</param>
        /// <param name="__iv">为了继承接口方法必须有此参数，请直接设为null</param>
        /// <returns>加密后的文本</returns>
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

        #region 其它方法

        /// <summary>
        /// DES向量。
        /// </summary>
        private byte[] DES_IV = { 34, 144, 78, 23, 231, 111, 26, 46 };

        /// <summary>
        /// 加密。
        /// </summary>
        /// <param name="text">待加密的文本</param>
        /// <param name="key">加密密钥</param>
        /// <returns>加密后的文本</returns>
        private string DESEncrypt(string text, string key)
        {
            byte[] byKey = null;
            string encryptResult = "";

            byKey = System.Text.Encoding.UTF8.GetBytes(key.PadRight(8).Substring(0, 8));
            DESCryptoServiceProvider Des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(text);
            var Ms = new MemoryStream();
            using (Ms)
            {
                CryptoStream Cs = new CryptoStream(Ms, Des.CreateEncryptor(byKey, DES_IV), CryptoStreamMode.Write);
                using (Cs)
                {
                    Cs.Write(inputByteArray, 0, inputByteArray.Length);
                    Cs.FlushFinalBlock();
                    encryptResult = Convert.ToBase64String(Ms.ToArray());
                }
            }

            return encryptResult;
        }

        /// <summary>
        /// 对字符串进行MD5散列。
        /// </summary>
        /// <param name="text">待加密的文本</param>
        /// <returns>加密后的文本</returns>
        private string MD5(string text)
        {
            byte[] b = System.Text.Encoding.Default.GetBytes(text);
            b = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(b);
            var sb = new StringBuilder();
            for (int i = 0; i < b.Length; i++)
            {
                sb.Append(b[i].ToString("x").PadLeft(2, '0'));
            }

            return sb.ToString();
        }

        #endregion
    }
}
