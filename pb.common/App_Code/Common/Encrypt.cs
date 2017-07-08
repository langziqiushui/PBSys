using System;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
    /// <summary>
    /// MD5 的摘要说明
    /// </summary>
    public class Encrypt
    {
        public static string MD5Encrypt(string str)
        {
            MD5 md5 = MD5.Create();
            byte[] data = Encoding.UTF8.GetBytes(str);
            byte[] data2 = md5.ComputeHash(data);

            return MD5Encrypt(data2);

        }

        public static string MD5Encrypt(byte[] data)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x2"));
            }
            return sb.ToString();
        }

        public static string EncodeBase64(string str)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
        }

        public static string DecodeBase64(string base64)
        {
            string decode = null;


            try
            {
                byte[] bytes = Convert.FromBase64String(base64);

                decode = System.Text.Encoding.ASCII.GetString(bytes);
            }
            catch
            {
            }

            return decode;
        }

    }

}