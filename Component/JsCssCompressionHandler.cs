using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Compression;
using System.Web;
using System.IO;
using System.Web.Caching;
using System.Security.Cryptography;

using YX.Core;

namespace YX.Component
{
    /// <summary>
    /// JS、CSS资源合并压缩输出。
    /// </summary>
    public class JsCssCompressionHandler : IHttpHandler
    {
        #region 变量

        /// <summary>
        /// GZIP:gzip。
        /// </summary>
        private const string GZIP = "gzip";
        /// <summary>
        /// 并发锁(读取资源)。
        /// </summary>
        private readonly object LOCK_RESOURCE = new object();

        #endregion

        #region IHttpHandler

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            this._ProcessRequest(context);
        }

        #endregion

        #region 处理请求

        /// <summary>
        /// 处理请求。
        /// </summary>
        /// <param name="context"></param>
        private void _ProcessRequest(HttpContext context)
        {
            //获取需要载入的资源。
            var qsFiles = context.Request.QueryString["f"];
            if (string.IsNullOrEmpty(qsFiles))
                return;

            //解密字符串。
            var decryptedQSFiles = new CryptographyManager(EncryptionFormats.DES2QueryString).Decrypt(qsFiles);

            //缓存键。
            var cacheKey = "JsCssResource" + qsFiles;
            //获取资源。
            var buffer = this.ReadResource(cacheKey, decryptedQSFiles.Split("|"));

            //如果浏览器支持压缩则压缩资源。
            if (this.IsAcceptGZIP())
            {
                buffer = this.CompressionResource(buffer);
                context.Response.AppendHeader("Content-encoding", GZIP);
            }

            //输出资源。
            context.Response.BinaryWrite(buffer);
            //输出字符编码。
            context.Response.ContentEncoding = Encoding.UTF8;
            //输出MIME类型。
            if (context.Request.RawUrl.IgnoreCaseIndexOf("css.axd") >= 0)
                context.Response.ContentType = "text/css";
        }

        #endregion

        #region 其它方法

        /// <summary>
        /// 从缓存获取或从文件读取资源。
        /// </summary>
        /// <param name="cacheKey">资源缓存键</param>
        /// <param name="rsFiles">资源文件集合</param>
        /// <returns></returns>
        private byte[] ReadResource(string cacheKey, string[] rsFiles)
        {
            //如果是本地访问，则直接从文件中读取资源(不缓存)。
            if (CoreUtil.HostPort.IgnoreCaseIndexOf("localhost") >= 0)
                return this.ReadResource(rsFiles);

            var rs = HttpRuntime.Cache[cacheKey];
            if (null != rs)
                return (byte[])rs;

            var buffer = this.ReadResource(rsFiles);
            HttpRuntime.Cache.Add(cacheKey, buffer, null, DateTime.MaxValue, Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
            return buffer;
        }

        /// <summary>
        /// 当在本地调试时读取资源文件。
        /// </summary>
        /// <param name="rsFiles">资源文件集合</param>
        /// <returns></returns>
        private byte[] ReadResource(string[] rsFiles)
        {
            var sb = new StringBuilder();
            foreach (var _rsFile in rsFiles)
            {
                var filePath = AppDomain.CurrentDomain.BaseDirectory + _rsFile;
                if (File.Exists(filePath))
                {
                    sb.Append(File.ReadAllText(filePath, Encoding.UTF8));
                    sb.Append(System.Environment.NewLine);
                }
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        /// <summary>
        /// 压缩资源。
        /// </summary>
        /// <param name="rs">资源</param>
        /// <returns></returns>
        public byte[] CompressionResource(byte[] rs)
        {
            using (var ms = new MemoryStream())
            {
                using (var compressedzipStream = new GZipStream(ms, CompressionMode.Compress, true))
                {
                    compressedzipStream.Write(rs, 0, rs.Length);
                }

                ms.Position = 0;
                var buffer = new byte[ms.Length];
                ms.Read(buffer, 0, buffer.Length);

                return buffer;
            }
        }

        /// <summary>
        /// 判断浏览器是否支持GZIP压缩。
        /// </summary>
        public bool IsAcceptGZIP()
        {
            var context = HttpContext.Current;
            return context.Request.Headers["Accept-encoding"] != null && context.Request.Headers["Accept-encoding"].Contains("gzip");
        }

        #endregion

    }
}
