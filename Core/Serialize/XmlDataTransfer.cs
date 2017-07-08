using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace YX.Core
{
    /// <summary>
    /// xml序列化和反序列化。
    /// </summary>
    public static class XmlDataTransfer
    {
        /// <summary>
        /// 将指定对象序列化为xml字符串。
        /// </summary>
        /// <param name="obj">将要序列化的对象</param>
        /// <returns></returns>
        public static string Serialize<T>(object obj)
        {
            var xs = new XmlSerializer(typeof(T));
            using (var ms = new MemoryStream())
            {
                xs.Serialize(ms, obj);
                ms.Position = 0;
                var buffer = new byte[ms.Length];
                ms.Read(buffer, 0, buffer.Length);
                return Encoding.UTF8.GetString(buffer);
            }
        }

        /// <summary>
        /// 将xml反序列化为指定对象。
        /// </summary>
        /// <param name="xmlText">XML字符串</param>
        /// <returns></returns>
        public static T Deserialize<T>(string xmlText)
        {
            var xs = new XmlSerializer(typeof(T));
            var buffer = Encoding.UTF8.GetBytes(xmlText);
            using (var ms = new MemoryStream())
            {
                ms.Write(buffer, 0, buffer.Length);
                ms.Position = 0;
                return (T)xs.Deserialize(ms);
            }
        }
    }
}
