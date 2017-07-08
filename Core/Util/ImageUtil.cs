using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Drawing;
using System.Web;
using System.IO;
using System.Drawing.Imaging;

namespace YX.Core
{
    /// <summary>
    /// 图片处理类。
    /// </summary>
    public static class ImageUtil
    {
        #region 构造器

        static ImageUtil()
        {
            htmimes = new Hashtable();
            htmimes[".jpe"] = "image/jpeg";
            htmimes[".jpeg"] = "image/jpeg";
            htmimes[".jpg"] = "image/jpeg";
            htmimes[".png"] = "image/png";
            htmimes[".gif"] = "image/gif";
            htmimes[".tif"] = "image/tiff";
            htmimes[".tiff"] = "image/tiff";
            htmimes[".bmp"] = "image/bmp";
        }

        #endregion

        #region 变量

        /// <summary>
        /// Hashtable.
        /// </summary>
        private static Hashtable htmimes = null;

        #endregion

        #region 保存图片

        /// <summary>
        /// 将指定的图片保存为图片文件。
        /// </summary>
        /// <param name="image">指定图片</param>
        /// <param name="savePath">图片保存路径</param>
        public static void SaveImage(System.Drawing.Image image, string savePath)
        {
            //获取图片扩展名。
            string ex = ".bmp";
            int lastIndex = savePath.LastIndexOf(".");
            if (lastIndex > 0)
                ex = savePath.Substring(lastIndex).ToLower();

            //保存图片。
            SaveImage(image, savePath, GetCodecInfo((string)htmimes[ex]));
        }

        /// <summary>
        /// 将指定的图片保存为图片文件。
        /// </summary>
        /// <param name="image">指定图片</param>
        /// <param name="savePath">图片保存路径</param>
        /// <param name="ici">指定格式的编解码参数</param>
        public static void SaveImage(System.Drawing.Image image, string savePath, ImageCodecInfo ici)
        {
            //设置 原图片 对象的 EncoderParameters 对象
            using (EncoderParameters parameters = new EncoderParameters(1))
            {
                parameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, ((long)90));
                image.Save(savePath, ici, parameters);
            }
        }

        /// <summary>
        /// 获取图像编码解码器的所有相关信息
        /// </summary>
        /// <param name="mimeType">包含编码解码器的多用途网际邮件扩充协议 (MIME) 类型的字符串</param>
        /// <returns>返回图像编码解码器的所有相关信息</returns>
        private static ImageCodecInfo GetCodecInfo(string mimeType)
        {
            ImageCodecInfo[] CodecInfo = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo ici in CodecInfo)
            {
                if (ici.MimeType == mimeType) return ici;
            }

            return null;
        }

        #endregion

        #region 生成缩略图

        /// <summary>
        /// 生成缩略图并保存为指定路径的图片。
        /// </summary>
        /// <param name="originalImage">原始图片对象</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="transparent">是否使用透明背景</param>
        public static void MakeThumbnail(Image originalImage, string thumbnailPath, int width, int height, bool transparent)
        {
            //再次生成符合预设缩略大小的图。
            using (Image thumbnailImage = MakeThumbnail(originalImage, width, height, transparent))
            {
                SaveImage(thumbnailImage, thumbnailPath);
            }
        }


        /// <summary>
        /// 生成一个全新的缩略图。
        /// </summary>
        /// <param name="originalImage">原始图片对象</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="transparent">是否使用透明背景</param>
        public static Image MakeThumbnail(Image originalImage, int width, int height, bool transparent)
        {
            //如果图片长、宽都小于指定的长、宽，则直接生成一个副本并返回。
            if (originalImage.Width <= width && originalImage.Height <= height)
                return DrawImage(originalImage, width, height, transparent);

            //计算将要达到目标的长宽比率。
            double oRate = (double)width / (double)height;
            //计算当前图片的长宽比率。
            double cRate = (double)originalImage.Width / (double)originalImage.Height;

            Image tImage = null;

            if (cRate > oRate)
            {
                //图片偏宽，将以高度为基准进行缩略。
                tImage = DrawImage(
                    originalImage,
                    (originalImage.Width * height) / originalImage.Height,
                    height,
                    new Rectangle(0, 0, originalImage.Width, originalImage.Height),
                    transparent
                );
            }
            else
            {
                //图片偏高，将以宽度为基准进行缩略。
                tImage = DrawImage(
                    originalImage,
                    width,
                    (width * originalImage.Height) / originalImage.Width,
                    new Rectangle(0, 0, originalImage.Width, originalImage.Height),
                    transparent
                );
            }

            //return tImage;

            using (tImage)
            {
                //从图片中间位置开始裁。
                var lx = 0;
                var ly = 0;
                if (tImage.Width > width)
                    lx = (tImage.Width - width) / 2;
                if (tImage.Height > height)
                    ly = (tImage.Height - height) / 2;

                return DrawImage(tImage, width, height, new Rectangle(lx, ly, width, height), transparent);
            }
        }

        /// <summary>
        /// 裁剪图片。
        /// </summary>
        /// <param name="originalImage">原始图像</param>
        /// <param name="width">背景图宽度</param>
        /// <param name="height">背景图高度</param>
        /// <param name="transparent">是否使用透明背景</param>
        /// <returns></returns>
        public static Image DrawImage(Image originalImage, int width, int height, bool transparent)
        {
            return DrawImage(originalImage, width, height, new Rectangle(0, 0, originalImage.Width, originalImage.Height), transparent);
        }

        /// <summary>
        /// 裁剪图片。
        /// </summary>
        /// <param name="originalImage">原始图像</param>
        /// <param name="width">背景图宽度</param>
        /// <param name="height">背景图高度</param>
        /// <param name="srcRect">源图片要截取的位置和大小</param>
        /// <param name="transparent">是否使用透明背景</param>
        /// <returns></returns>
        public static Image DrawImage(Image originalImage, int width, int height, Rectangle srcRect, bool transparent)
        {
            //新建一个bmp图片。
            Image bitmap = new Bitmap(width, height);
            //新建一个画板
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                //设置高质量插值法
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                //设置高质量,低速度呈现平滑程度
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //填充背景色。
                if (transparent)
                    g.Clear(Color.Transparent);
                else
                    g.Clear(Color.White);
                //绘制图片。
                g.DrawImage(
                    originalImage,
                    new Rectangle(0, 0, width, height),
                    srcRect,
                    GraphicsUnit.Pixel
                    );
            }

            return bitmap;
        }

        #region 废

        /// <summary>
        /// 生成一个全新的缩略图(如果指定的长宽大于原始图片的长宽，则以原始图片的长宽为准)。
        /// </summary>
        /// <param name="originalImage">原始图片对象</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        //public static Image MakeThumbnail(Image originalImage, int width, int height)
        //{
        //    int towidth = width;
        //    int toheight = height;
        //    int x = 0;
        //    int y = 0;
        //    int ow = originalImage.Width;
        //    int oh = originalImage.Height;
        //    Image thumbImage = null;

        //    //如果图片长、宽都小于指定的长、宽，则直接生成一个副本并返回。
        //    if (originalImage.Width <= width && originalImage.Height <= height)
        //        return ClipImage(originalImage, originalImage.Width, originalImage.Height, new Rectangle(0, 0, ow, oh));
        //    //如果长宽比例一致，，则直接生成一个副本并返回。
        //    if (originalImage.Width == originalImage.Height)
        //        return ClipImage(originalImage, width, height, new Rectangle(0, 0, ow, oh));

        //    //获取原图长、宽比例。
        //    double imageRate = (double)originalImage.Width / (double)originalImage.Height;

        //    //如果长、宽都大于指定长、宽，进行缩略。
        //    if (originalImage.Width > width && originalImage.Height > height)
        //    {
        //        if (originalImage.Width > width)
        //        {
        //            //根据宽度按比例缩小。
        //            towidth = width;
        //            toheight = (int)((double)towidth / imageRate);

        //            //如果缩小后的高度小于目标高度，则以目标高度为准放大。
        //            if (toheight < height)
        //            {
        //                toheight = height;
        //                towidth = (int)((double)toheight * imageRate);
        //            }
        //        }

        //        //生成一个缩略图。
        //        thumbImage = ClipImage(originalImage, towidth, toheight, new System.Drawing.Rectangle(x, y, ow, oh));
        //    }
        //    else
        //    {
        //        //这里只创建一个副本。
        //        thumbImage = ClipImage(originalImage, ow, oh, new System.Drawing.Rectangle(x, y, ow, oh));
        //    }

        //    x = 0;
        //    y = 0;
        //    ow = thumbImage.Width;
        //    oh = thumbImage.Height;

        //    //裁剪多余部分，使之匹配要求的长度和宽度。
        //    if (thumbImage.Width > width)
        //    {
        //        x = (thumbImage.Width - width) / 2;
        //        ow = width;
        //    }

        //    if (thumbImage.Height > height)
        //    {
        //        y = (thumbImage.Height - height) / 2;
        //        oh = height;
        //    }

        //    //再次生成符合预设缩略大小的图。
        //    Image thumbImage2 = null;
        //    using (thumbImage)
        //    {
        //        thumbImage2 = ClipImage(thumbImage, Math.Min(width, thumbImage.Width), Math.Min(height, thumbImage.Height), new System.Drawing.Rectangle(x, y, ow, oh));
        //    }

        //    return thumbImage2;
        //}

        /// <summary>
        /// 裁剪图片。
        /// </summary>
        /// <param name="originalImage">将要裁剪的图片</param>
        /// <param name="towidth">缩略宽度</param>
        /// <param name="toheight">缩略长度</param>
        /// <param name="drawRect">指定 image 对象中要绘制的部分的</param>
        /// <returns></returns>
        //private static Image ClipImage(System.Drawing.Image originalImage, int towidth, int toheight, System.Drawing.Rectangle drawRect)
        //{
        //    //新建一个bmp图片
        //    System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);
        //    //新建一个画板
        //    System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
        //    //设置高质量插值法
        //    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
        //    //设置高质量,低速度呈现平滑程度
        //    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        //    //清空画布并以透明背景色填充
        //    g.Clear(System.Drawing.Color.Transparent);
        //    //在指定位置并且按指定大小绘制原图片的指定部分
        //    g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),
        //       drawRect,
        //        System.Drawing.GraphicsUnit.Pixel);
        //    g.Dispose();
        //    g = null;

        //    return bitmap;
        //}

        #endregion

        #endregion

        #region 标记水印

        /// <summary>
        /// 标记水印。
        /// </summary>
        /// <param name="originalImage">需要标记水印的原始Image</param>
        /// <param name="imgWaterMark">水印Image</param>
        /// <param name="backupOriginalImageFilePath">在标记水印前备份原Image的路径(null或空字符串表示不备份)</param>
        /// <returns>标记水印后的新的图像流或null</returns>
        public static Stream MakeWaterMark(Stream originalStream, Image imgWaterMark, string backupOriginalImageFilePath)
        {
            if (null == imgWaterMark)
                return null;

            //创建原始Image。
            using (var originalImage = Image.FromStream(originalStream))
            {
                //备份原Image。
                if (!string.IsNullOrEmpty(backupOriginalImageFilePath))
                {
                    var folder = Path.GetDirectoryName(backupOriginalImageFilePath);
                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    SaveImage(originalImage, backupOriginalImageFilePath);
                }

                MemoryStream ms = null;

                //根据上传的图像大小创建图像。
                using (var bitmap = new Bitmap(originalImage.Width, originalImage.Height))
                {
                    //根据原图大小自动设置水印图片大小(1/5)。
                    var wmDestWidth = (originalImage.Width + originalImage.Height) / 5;
                    int wmWidth = imgWaterMark.Width;
                    int wmHeight = imgWaterMark.Height;
                    if (wmDestWidth < wmWidth)
                    {
                        wmHeight = (int)((double)wmDestWidth * ((double)wmWidth / (double)wmHeight));
                        wmWidth = wmDestWidth;
                    }

                    using (var g = Graphics.FromImage(bitmap))
                    {
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        //绘制原始图像。
                        g.DrawImage(originalImage, 0, 0, originalImage.Width, originalImage.Height);
                        //绘制水印。
                        g.DrawImage(
                            imgWaterMark,
                            bitmap.Width - 2 - wmWidth,
                            bitmap.Height - 2 - wmHeight,
                            wmWidth,
                            wmHeight
                            );
                    }

                    ms = new MemoryStream();
                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                }

                return ms;
            }
        }

        #endregion

        #region 将图片转为jpeg格式

        /// <summary>
        /// 将图片转为jpeg格式。
        /// </summary>
        /// <param name="buffer">图片内容</param>
        /// <param name="imgSavePath">图片保存路径(如果不保存置为null)</param>
        /// <returns></returns>
        public static byte[] ConvertToJPEG(byte[] buffer, string imgSavePath)
        {
            using (var ms = new System.IO.MemoryStream())
            {
                ms.Write(buffer, 0, buffer.Length);
                using (var img = System.Drawing.Image.FromStream(ms))
                {
                    //保存到文件中。
                    if (!string.IsNullOrEmpty(imgSavePath))
                        img.Save(imgSavePath, System.Drawing.Imaging.ImageFormat.Jpeg);

                    using (var ms2 = new System.IO.MemoryStream())
                    {
                        img.Save(ms2, System.Drawing.Imaging.ImageFormat.Jpeg);
                        ms2.Position = 0;
                        buffer = new byte[ms2.Length];
                        ms2.Read(buffer, 0, buffer.Length);
                        return buffer;
                    }
                }
            }
        }

        #endregion

        #region 根据设定的最大宽度、高度计算获取最合适的宽度和高度

        /// <summary>
        /// 根据设定的最大宽度、高度计算获取最合适的宽度和高度.
        /// </summary>
        /// <param name="img">图像</param>
        /// <param name="maxWidth">设定的最大宽度</param>
        /// <param name="maxHeight">设定的最大高度</param>
        /// <returns></returns>
        public static Size CalculationOptimumSize(Image img, int maxWidth, int maxHeight)
        {
            int ow = 0;
            int oh = 0;

            var imageRate = (decimal)img.Width / (decimal)img.Height;

            if (img.Width > maxWidth)
            {
                ow = maxWidth;
                oh = (int)((decimal)maxWidth / imageRate);

                if (oh > maxHeight)
                {
                    ow = (int)((decimal)maxHeight * imageRate);
                    oh = maxHeight;
                }
            }

            if (img.Height > maxHeight)
            {
                ow = (int)((decimal)maxHeight * imageRate);
                oh = maxHeight;

                if (ow > maxWidth)
                {
                    ow = maxWidth;
                    oh = (int)((decimal)maxWidth / imageRate);
                }
            }

            return new Size() { Width = ow, Height = oh };
        }

        #endregion
    }

    /// <summary>
    /// 自动匹配并保存图片。
    /// </summary>
    public class AutoUploadImage
    {
        /// <summary>
        /// 构造器。
        /// </summary>
        /// <param name="html">将要匹配的html标签</param>
        /// <param name="uploadFolder">图片上传目录</param>
        /// <param name="domain">当前网站域名</param>
        public AutoUploadImage(string html, string uploadFolder, string domain)
        {
            this._html = html;
            this._uploadFolder = uploadFolder;
            this._domain = domain;
        }

        #region 变量

        /// <summary>
        /// 将要匹配的html标签。
        /// </summary>
        private string _html = string.Empty;
        /// <summary>
        /// 当前网站域名。
        /// </summary>
        private string _domain = string.Empty;
        /// <summary>
        /// 图片上传目录。
        /// </summary>
        private string _uploadFolder = string.Empty;
        /// <summary>
        /// 已经上传到当前服务器的图片集合。
        /// </summary>
        private List<string> _uploadImages = new List<string>();
        /// <summary>
        /// 匹配出图片的正则表达式。
        /// </summary>
        private const string PATTERN_IMG = @"(?:<\s*img\s+src)|(?:<\s*img\s+.*?\s+src)\s*=\s*['""](.*?)['""]";
        /// <summary>
        /// 匹配出ActiveX控件的正则表达式。
        /// </summary>
        private const string PATTERN_OBJECT = @"<object\s.*?</object>";

        #endregion

        #region 属性

        /// <summary>
        /// 获取 将要匹配的html标签。
        /// </summary>
        public string Html
        {
            get { return this._html; }
        }

        /// <summary>
        /// 获取 上传目录。
        /// </summary>
        public string UploadFolder
        {
            get { return this._uploadFolder; }
        }

        /// <summary>
        /// 获取 已经上传到当前服务器的图片集合。
        /// </summary>
        public List<string> UploadImages
        {
            get { return this._uploadImages; }
        }

        #endregion

        /// <summary>
        /// 匹配并上传图片。
        /// </summary>
        /// <returns></returns>
        public void Match()
        {
            System.Threading.Thread.Sleep(1000);
            //去掉ActiveX控件。
            this._html = Regex.Replace(this._html, PATTERN_OBJECT, string.Empty, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            //匹配图片。
            this._html = Regex.Replace(_html, PATTERN_IMG, new MatchEvaluator(ReplaceCC), RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }

        /// <summary>
        /// 正则式匹配回调函数。
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public string ReplaceCC(Match match)
        {
            //此正则将会匹配出如：
            // (<img class="hq2010_0501" alt="" src="http://china.huanqiu.com/roll/2010-09/attachment/100928/76c2003d03.jpg") (http://china.huanqiu.com/roll/2010-09/attachment/100928/76c2003d03.jpg)
            //注意是不完整的图片html。

            try
            {
                //获取匹配的图片引用路径。
                var srcImageUrl = match.Groups[1].Value.Trim().ToLower();
                if (srcImageUrl.IndexOf("http://") < 0)
                    srcImageUrl = this._domain + srcImageUrl;

                //设置图片保存路径。
                string ext = ".gif";
                if (srcImageUrl.LastIndexOf(".") > 0)
                    ext = srcImageUrl.Substring(srcImageUrl.LastIndexOf("."));
                string newFileName = Guid.NewGuid().ToString() + ext;

                //下载并保存到服务器。
                //new WebClient().DownloadFile(srcImageUrl, HttpContext.Current.Server.MapPath(this._uploadFolder + "/" + newFileName));
                this.PowerDownloadImage(srcImageUrl, HttpContext.Current.Server.MapPath(this._uploadFolder + "/" + newFileName));
                this._uploadImages.Add(newFileName);

                return match.Value.Replace(match.Groups[1].Value, this._uploadFolder.Replace("~/", "/") + "/" + newFileName);
            }
            catch (Exception ex)
            {
                return match.Value;
            }
        }

        /// <summary>
        /// 下载图片。
        /// </summary>
        /// <param name="srcImageUrl"></param>
        /// <param name="newImagePath"></param>
        public void PowerDownloadImage(string srcImageUrl, string newImagePath)
        {
            //从网址匹配出域名。
            Match mc = Regex.Match(srcImageUrl, @"(\w+:\/\/)?([^\.]+)(\.[^/:]+)(:\d*)?([^# ]*)", RegexOptions.IgnoreCase);
            var domain = mc.Groups[1].Value + mc.Groups[2].Value + mc.Groups[3].Value;

            var req = (HttpWebRequest)WebRequest.Create(srcImageUrl);
            req.Referer = domain;
            req.KeepAlive = true;
            req.Method = "GET";
            req.ContentType = "application/x-www-form-urlencoded";
            req.Timeout = 100000;
            req.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-silverlight, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, */*";

            req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

            var myCookieContainer = new CookieContainer();
            myCookieContainer.SetCookies(new Uri(domain), "__utma=257672927.931789114.1237337096.1238547927.1238557056.15; __utmz=257672927.1237337096.1.1.utmccn=(direct)|utmcsr=(direct)|utmcmd=(none); ystat_bc_891716=36362456653840709312; ASP.NET_SessionId=fec3mzbupityrmu20ujfbemf");

            //自己创建的CookieContainer
            req.CookieContainer = myCookieContainer;

            //下载并保存图片。
            var rep = (HttpWebResponse)req.GetResponse();
            var img = Image.FromStream(rep.GetResponseStream());
            ImageUtil.SaveImage(img, newImagePath);
        }
    }
}
