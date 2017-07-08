using System;
using System.Collections.Generic;
using System.Text;

namespace YX.Core
{
    /// <summary>
    /// ���ݼӡ����ܽӿڡ�
    /// </summary>
    public interface ICryptography
    {
        /// <summary>
        /// ���ܡ�
        /// </summary>
        /// <param name="text">�����ܵ��ı�</param>
        /// <param name="key">������Կ</param>
        /// <param name="iv">��������</param>
        /// <returns>���ܺ���ı�</returns>
        string Encrypt(string text, string key, string iv);

        /// <summary>
        /// ���ܡ�
        /// </summary>
        /// <param name="text">�����ܵ��ı�</param>
        /// <param name="key">������Կ</param>
        /// <param name="iv">��������</param>
        /// <returns>���ܺ���ı�</returns>
        string Decrypt(string text, string key, string iv);

        /// <summary>
        /// ��ȡ ��ǰ�����㷨�Ƿ�������ܡ�
        /// </summary>
        bool CanDecrypt { get; }
    }


    /// <summary>
    /// ����ӽ��ܷ�ʽ��
    /// </summary>
    public enum EncryptionFormats
    {
        /// <summary>
        /// δ���ܡ�
        /// </summary>
        [EnumDescription("δ����")]
        UnEncrypted = 0,
        /// <summary>
        /// MD5SHA256���ϼ����㷨(������)
        /// </summary>
        [EnumDescription("MD5SHA256����(������)")]
        MD5SHA256 = 1,
        /// <summary>
        /// ��ǿ��SHA256�����㷨(������)
        /// </summary>
        [EnumDescription("StrongSHA256����(������)")]
        StrongSHA256 = 2,
        /// <summary>
        /// AES����(����)
        /// </summary>
        [EnumDescription("AES����(����)")]
        AES = 3,
        /// <summary>
        /// MD5�򵥼���(������)
        /// </summary>
        [EnumDescription("MD5�򵥼���(������)")]
        MD5 = 4,
        /// <summary>
        /// ר�����ڼӡ�������ҳ��ѯ�����Ķ���
        /// </summary>
        [EnumDescription("DES2QueryString����(����)")]
        DES2QueryString = 10
    }

    //����ӽ��ܷ�ʽ[EncryptionFormats]δ����=0, MD5SHA256����(������)=1, StrongSHA256����(������)=2, AES����(����)=3,MD5 = 4
}
