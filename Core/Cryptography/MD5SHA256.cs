using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace YX.Core
{
    /// <summary>
    /// MD5SHA256���ϼ����㷨(������)��
    /// </summary>
    public class MD5SHA256 : ICryptography
    {
        #region ICryptography ��Ա

        /// <summary>
        /// ���ܡ�
        /// </summary>
        /// <param name="text">�����ܵ��ı�</param>
        /// <param name="__key">Ϊ�˼̳нӿڷ��������д˲�������ֱ����Ϊnull</param>
        /// <param name="__iv">Ϊ�˼̳нӿڷ��������д˲�������ֱ����Ϊnull</param>
        /// <returns>���ܺ���ı�</returns>
        public string Encrypt(string text, string __key, string __iv)
        {
            //��MD5���ܡ�
            byte[] b = System.Text.Encoding.Default.GetBytes(text);
            b = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(b);
            var sb = new StringBuilder();
            for (int i = 0; i < b.Length; i++)
                sb.Append(b[i].ToString("x").PadLeft(2, '0'));

            //SHA256���ܡ�
            var data = new UnicodeEncoding().GetBytes(sb.ToString());
            var result = new SHA256Managed().ComputeHash(data);
            return BitConverter.ToString(result);
        }

        /// <summary>
        /// ���ܡ�
        /// </summary>
        /// <param name="text">�����ܵ��ı�</param>
        /// <param name="__key">Ϊ�˼̳нӿڷ��������д˲�������ֱ����Ϊnull</param>
        /// <param name="__iv">Ϊ�˼̳нӿڷ��������д˲�������ֱ����Ϊnull</param>
        /// <returns>���ܺ���ı�</returns>
        public string Decrypt(string text, string __key, string __iv)
        {
            return text;
        }

        /// <summary>
        /// ��ȡ ��ǰ�����㷨�Ƿ�������ܡ�
        /// </summary>
        public bool CanDecrypt
        {
            get { return false; }
        }

        #endregion       
    }
}
