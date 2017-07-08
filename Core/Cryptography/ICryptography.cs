using System;
using System.Collections.Generic;
using System.Text;

namespace YX.Core
{
    /// <summary>
    /// 数据加、解密接口。
    /// </summary>
    public interface ICryptography
    {
        /// <summary>
        /// 加密。
        /// </summary>
        /// <param name="text">待加密的文本</param>
        /// <param name="key">加密密钥</param>
        /// <param name="iv">加密向量</param>
        /// <returns>加密后的文本</returns>
        string Encrypt(string text, string key, string iv);

        /// <summary>
        /// 解密。
        /// </summary>
        /// <param name="text">待解密的文本</param>
        /// <param name="key">解密密钥</param>
        /// <param name="iv">解密向量</param>
        /// <returns>解密后的文本</returns>
        string Decrypt(string text, string key, string iv);

        /// <summary>
        /// 获取 当前加密算法是否可以争密。
        /// </summary>
        bool CanDecrypt { get; }
    }


    /// <summary>
    /// 密码加解密方式。
    /// </summary>
    public enum EncryptionFormats
    {
        /// <summary>
        /// 未加密。
        /// </summary>
        [EnumDescription("未加密")]
        UnEncrypted = 0,
        /// <summary>
        /// MD5SHA256联合加密算法(不可逆)
        /// </summary>
        [EnumDescription("MD5SHA256加密(不可逆)")]
        MD5SHA256 = 1,
        /// <summary>
        /// 增强型SHA256加密算法(不可逆)
        /// </summary>
        [EnumDescription("StrongSHA256加密(不可逆)")]
        StrongSHA256 = 2,
        /// <summary>
        /// AES加密(可逆)
        /// </summary>
        [EnumDescription("AES加密(可逆)")]
        AES = 3,
        /// <summary>
        /// MD5简单加密(不可逆)
        /// </summary>
        [EnumDescription("MD5简单加密(不可逆)")]
        MD5 = 4,
        /// <summary>
        /// 专门用于加、解密网页查询参数的对象
        /// </summary>
        [EnumDescription("DES2QueryString加密(可逆)")]
        DES2QueryString = 10
    }

    //密码加解密方式[EncryptionFormats]未加密=0, MD5SHA256加密(不可逆)=1, StrongSHA256加密(不可逆)=2, AES加密(可逆)=3,MD5 = 4
}
