using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace YX.Core
{
    /// <summary>
    /// AES加、解密算法。
    /// </summary>
    public class AES : ICryptography
    {
        #region ICryptography 成员      

        /// <summary>
        /// 加密。
        /// </summary>
        /// <param name="text">将要加密的明文字符串</param>
        /// <param name="key">32位密钥</param>
        /// <param name="iv">16位加密向量</param>
        /// <returns></returns>
        public string Encrypt(string text, string key, string iv)
        {
            //重新确定密钥。
            key = this.MD5(key);
            Byte[] bKey = new Byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(bKey.Length)), bKey, bKey.Length);
            //重新确定IV向量。
            iv = this.MD5(iv);
            Byte[] bVector = new Byte[16];
            Array.Copy(Encoding.UTF8.GetBytes(iv.PadRight(bVector.Length)), bVector, bVector.Length);
            //重新确实加密字符串。
            var data = Encoding.UTF8.GetBytes(text);

            Byte[] cryptograph = null;
            Rijndael Aes = Rijndael.Create();

            // 开辟一块内存流
            using (MemoryStream Memory = new MemoryStream())
            {
                // 把内存流对象包装成加密流对象
                using (CryptoStream Encryptor = new CryptoStream(Memory, Aes.CreateEncryptor(bKey, bVector), CryptoStreamMode.Write))
                {
                    // 明文数据写入加密流
                    Encryptor.Write(data, 0, data.Length);
                    Encryptor.FlushFinalBlock();
                    cryptograph = Memory.ToArray();
                }
            }

            return Convert.ToBase64String(cryptograph);
        }

        /// <summary>
        /// 解密。
        /// </summary>
        /// <param name="text">将要解密的密文字符串</param>
        /// <param name="key">32位密钥</param>
        /// <param name="iv">16位加密向量</param>
        /// <returns></returns>
        public string Decrypt(string text, string key, string iv)
        {
            //重新确定密钥。
            key = this.MD5(key);
            Byte[] bKey = new Byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(bKey.Length)), bKey, bKey.Length);
            //重新确定IV向量。
            iv = this.MD5(iv);
            Byte[] bVector = new Byte[16];
            Array.Copy(Encoding.UTF8.GetBytes(iv.PadRight(bVector.Length)), bVector, bVector.Length);
            //重新确实加密字符串。
            var data = Convert.FromBase64String(text);

            Byte[] original = null; 
            Rijndael Aes = Rijndael.Create();

            // 开辟一块内存流，存储密文
            using (MemoryStream Memory = new MemoryStream(data))
            {
                // 把内存流对象包装成加密流对象
                using (CryptoStream Decryptor = new CryptoStream(Memory,
                Aes.CreateDecryptor(bKey, bVector),
                CryptoStreamMode.Read))
                {
                    // 明文存储区
                    using (MemoryStream originalMemory = new MemoryStream())
                    {
                        Byte[] Buffer = new Byte[1024];
                        Int32 readBytes = 0;
                        while ((readBytes = Decryptor.Read(Buffer, 0, Buffer.Length)) > 0)
                        {
                            originalMemory.Write(Buffer, 0, readBytes);
                        }

                        original = originalMemory.ToArray();
                    }
                }
            }

            return Encoding.UTF8.GetString(original);
        }

        /// <summary>
        /// 获取 当前加密算法是否可以争密。
        /// </summary>
        public bool CanDecrypt
        {
            get { return true; }
        }


        #endregion

        #region 对字符串进行MD5散列

        /// <summary>
        /// 对字符串进行MD5散列。
        /// </summary>
        /// <param name="text">待加密的文本</param>
        /// <returns>加密后的文本</returns>
        public string MD5(string text)
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
