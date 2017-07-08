using System;
using System.Collections.Generic;
using System.Text;

namespace YX.Core
{
    /// <summary>
    /// 字符加密、解密管理器。
    /// </summary>
    public class CryptographyManager
    {
        /// <summary>
        /// 构造器。
        /// </summary>
        /// <param name="type">密码加、解码方式</param>
        /// <remarks>默认为MD5加密方式</remarks>
        public CryptographyManager(EncryptionFormats format)
        {
            this.format = format;

            switch (this.format)
            {
                case EncryptionFormats.MD5SHA256:
                    this.cryptographyObject = new MD5SHA256();
                    break;
                case EncryptionFormats.DES2QueryString:
                    this.cryptographyObject = new DES2QueryString();
                    break;
                case EncryptionFormats.AES:
                    this.cryptographyObject = new AES();
                    break;
                case EncryptionFormats.StrongSHA256:
                    this.cryptographyObject = new StrongSHA256();
                    break;
                case EncryptionFormats.MD5:
                    this.cryptographyObject = new MD5();
                    break;
            }
        }


        /// <summary>
        /// 加密格式。
        /// </summary>
        private EncryptionFormats format = EncryptionFormats.UnEncrypted;
        /// <summary>
        /// 继承了ICryptography接口的对象。
        /// </summary>
        private ICryptography cryptographyObject = null;

        /// <summary>
        /// 使用系统配置文件设定的密钥和IV加密。
        /// </summary>
        /// <param name="text">待加密的文本</param>
        /// <returns>加密后的文本</returns>
        public string Encrypt(string text)
        {
            return this.Encrypt(text, ConfigAppSettings.Cryptography_Key, ConfigAppSettings.Cryptography_IV);
        }

        /// <summary>
        /// 加密。
        /// </summary>
        /// <param name="text">待加密的文本</param>
        /// <param name="key">加密密钥</param>
        /// <param name="iv">加密向量</param>
        /// <returns>加密后的文本</returns>
        public string Encrypt(string text, string key, string iv)
        {
            if (null == this.cryptographyObject)
                return text;

            return this.cryptographyObject.Encrypt(text, key, iv);
        }

        /// <summary>
        /// 使用系统配置文件设定的密钥和IV解密。
        /// </summary>
        /// <param name="text">待加密的文本</param>
        /// <returns>解密后的文本</returns>
        public string Decrypt(string text)
        {
            return this.Decrypt(text, ConfigAppSettings.Cryptography_Key, ConfigAppSettings.Cryptography_IV);
        }

        /// <summary>
        /// 解密。
        /// </summary>
        /// <param name="text">待加密的文本</param>
        /// <param name="key">加密密钥</param>
        /// <param name="iv">加密向量</param>
        /// <returns>解密后的文本</returns>
        public string Decrypt(string text, string key, string iv)
        {
            if (null == this.cryptographyObject)
                return text;

            return this.cryptographyObject.Decrypt(text, key, iv);
        }

        /// <summary>
        /// 获取 当前加密算法是否可以解密。
        /// </summary>
        public bool CanDecrypt
        {
            get
            {
                if (null == this.cryptographyObject)
                    return true;

                return this.cryptographyObject.CanDecrypt;
            }
        }

        /// <summary>
        /// 获取 加密方式。
        /// </summary>
        public EncryptionFormats Format
        {
            get { return this.format; }
        }
    }
}
