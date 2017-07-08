using System;
using System.Collections.Generic;
using System.Text;

namespace YX.Core
{
    /// <summary>
    /// �ַ����ܡ����ܹ�������
    /// </summary>
    public class CryptographyManager
    {
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="type">����ӡ����뷽ʽ</param>
        /// <remarks>Ĭ��ΪMD5���ܷ�ʽ</remarks>
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
        /// ���ܸ�ʽ��
        /// </summary>
        private EncryptionFormats format = EncryptionFormats.UnEncrypted;
        /// <summary>
        /// �̳���ICryptography�ӿڵĶ���
        /// </summary>
        private ICryptography cryptographyObject = null;

        /// <summary>
        /// ʹ��ϵͳ�����ļ��趨����Կ��IV���ܡ�
        /// </summary>
        /// <param name="text">�����ܵ��ı�</param>
        /// <returns>���ܺ���ı�</returns>
        public string Encrypt(string text)
        {
            return this.Encrypt(text, ConfigAppSettings.Cryptography_Key, ConfigAppSettings.Cryptography_IV);
        }

        /// <summary>
        /// ���ܡ�
        /// </summary>
        /// <param name="text">�����ܵ��ı�</param>
        /// <param name="key">������Կ</param>
        /// <param name="iv">��������</param>
        /// <returns>���ܺ���ı�</returns>
        public string Encrypt(string text, string key, string iv)
        {
            if (null == this.cryptographyObject)
                return text;

            return this.cryptographyObject.Encrypt(text, key, iv);
        }

        /// <summary>
        /// ʹ��ϵͳ�����ļ��趨����Կ��IV���ܡ�
        /// </summary>
        /// <param name="text">�����ܵ��ı�</param>
        /// <returns>���ܺ���ı�</returns>
        public string Decrypt(string text)
        {
            return this.Decrypt(text, ConfigAppSettings.Cryptography_Key, ConfigAppSettings.Cryptography_IV);
        }

        /// <summary>
        /// ���ܡ�
        /// </summary>
        /// <param name="text">�����ܵ��ı�</param>
        /// <param name="key">������Կ</param>
        /// <param name="iv">��������</param>
        /// <returns>���ܺ���ı�</returns>
        public string Decrypt(string text, string key, string iv)
        {
            if (null == this.cryptographyObject)
                return text;

            return this.cryptographyObject.Decrypt(text, key, iv);
        }

        /// <summary>
        /// ��ȡ ��ǰ�����㷨�Ƿ���Խ��ܡ�
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
        /// ��ȡ ���ܷ�ʽ��
        /// </summary>
        public EncryptionFormats Format
        {
            get { return this.format; }
        }
    }
}
