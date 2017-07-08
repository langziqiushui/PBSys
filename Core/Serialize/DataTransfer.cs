using System;
using System.Collections.Generic;
using System.Runtime;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace YX.Core
{
    /// <summary>
    /// 对象序列化及反序列化。
    /// </summary>
    public static class DataTransfer
    {
        #region 序列化为文件

        /// <summary>
        /// 将序列化后的信息转换为字符串。
        /// </summary>
        /// <param name="data">要序列化的对象</param>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static void Serialize(object data, string path)
        {
            if (null == data)
            {
                if (File.Exists(path))
                    File.Delete(path);
                return;
            }

            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                BinaryFormatter format = new BinaryFormatter();
                format.Serialize(fs, data);
            }
        }

        /// <summary>
        /// 反序列化数据
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>object对象</returns>
        public static object Deserialize(string path)
        {
            if (!File.Exists(path))
                return null;

            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                BinaryFormatter format = new BinaryFormatter();
                return format.Deserialize(fs);
            }
        }

        #endregion

        #region 克隆一个对象

        /// <summary>
        /// 克隆一个对象。
        /// </summary>
        /// <param name="data">源数据</param>
        /// <returns></returns>
        public static object Clone(object data)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter format = new BinaryFormatter();
                format.Serialize(ms, data);

                return format.Deserialize(ms);
            }
        }

        #endregion

        #region 序列化为数据

        /// <summary>
        /// 将对象实例序列化为二进制数据。
        /// </summary>
        /// <param name="ins">将要序列化的对象实例</param>
        /// <returns></returns>
        public static byte[] SerializeData(object ins)
        {
            using (var ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, ins);
                ms.Position = 0;
                var buffer = new byte[ms.Length];
                ms.Read(buffer, 0, buffer.Length);
                return buffer;
            }
        }

        /// <summary>
        /// 将二进制数据反序列化为实例对象。
        /// </summary>
        /// <param name="buffer">二进制数据</param>
        /// <returns>返回对象实例</returns>
        public static object DeserializeData(byte[] buffer)
        {
            using (var ms = new MemoryStream())
            {
                ms.Write(buffer, 0, buffer.Length);
                ms.Position = 0;
                return new BinaryFormatter().Deserialize(ms);
            }
        }

        #endregion

        #region 序列化为字符串

        /// <summary>
        /// 将对象实例序列化为文本。
        /// </summary>
        /// <param name="ins">将要序列化的对象实例</param>
        /// <param name="isEncrypt">是否需要加密</param>
        /// <returns></returns>
        public static string SerializeText(object ins, bool isEncrypt)
        {
            var text = Convert.ToBase64String(SerializeData(ins));
            if (isEncrypt)
                text = new CryptographyManager(EncryptionFormats.AES).Encrypt(text);
            return text;
        }

        /// <summary>
        /// 将文本反序列化为实例对象。
        /// </summary>
        /// <param name="text">需要反序列化的文本</param>
        /// <param name="isDecrypt">是否需要解密</param>
        /// <returns>返回对象实例</returns>
        public static object DeserializeText(string text, bool isDecrypt)
        {
            if (isDecrypt)
                text = new CryptographyManager(EncryptionFormats.AES).Decrypt(text);
            return DeserializeData(Convert.FromBase64String(text));
        }

        #endregion

    }
}
