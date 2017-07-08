using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Web;
using System.Configuration;
using System.Security.Cryptography;

using YX.Core;

namespace YX.Component
{
    /// <summary>
    /// 生成验证码图片。
    /// </summary>
    public sealed class CreateVerifyCodeImage
    {
        #region 构造器

        static CreateVerifyCodeImage() { }
        private CreateVerifyCodeImage() { }

        #endregion

        #region 变量

        /// <summary>
        /// 定义字体集。
        /// </summary>
        private static string[] fonts = { "Georgia", "Palatino Linotype", "Times New Roman", "Sylfaen" };
        private static string[] chars = { "a", "b", "c", "d", "e", "f", "g", "h", "k", "m", "n", "p", "q", "r", "s", "t", "u", "w", "x", "y", "z", "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "W", "X", "Y", "Z" };

        #endregion

        #region 生成图片(外部唯一调用方法)

        /// <summary>
        /// 生成图片(外部唯一调用方法)。
        /// </summary>
        /// <param name="sessionID">SessionID</param>
        public static void DrawImage(string sessionID)
        {
            var creater = new CreateVerifyCodeImage();
            string validateCode = creater.RndNum(4);
            HttpContext.Current.Session[sessionID] = validateCode;
            creater.CreateImage(validateCode);
        }

        #endregion

        #region 生成验证图片

        /// <summary>
        /// 绘制单个文本并旋转图片。
        /// </summary>
        /// <param name="text">要绘制的文本</param>
        /// <param name="fontColor">文本颜色</param>
        /// <param name="font">字体</param>
        /// <returns></returns>
        private Bitmap DrawString(string text, Color fontColor, Font font)
        {
            Random r = new Random(Guid.NewGuid().GetHashCode());
            using (Bitmap imgText = new Bitmap(50, 63))
            {
                using (Graphics gText = Graphics.FromImage(imgText))
                {
                    //呈现文本高质量。
                    gText.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                    //绘制字符。
                    gText.DrawString(text, font, new SolidBrush(fontColor), 0, 0);
                    //旋转图像。
                    return this.RotateImage(imgText, r.Next(-20, 20));
                }
            }
        }

        /// <summary>
        /// 生成验证图片。
        /// </summary>
        /// <param name="checkCode">验证字符</param>
        private void CreateImage(string checkCode)
        {
            checkCode = checkCode.ToUpper();
            Random r = new Random();
            using (Bitmap img = new Bitmap(130, 53))
            {
                using (Graphics g = Graphics.FromImage(img))
                {
                    //绘制随机背景色。
                    g.Clear(Color.FromArgb(r.Next(150, 255), r.Next(150, 255), r.Next(150, 255)));

                    int fontIndex = r.Next(0, fonts.Length) % fonts.Length;
                    int locationX = 3;

                    //输出不同字体和颜色的验证码字符。
                    var fontColor = Color.FromArgb(r.Next(0, 150), r.Next(0, 150), r.Next(0, 150));
                    for (int i = 0; i < checkCode.Length; i++)
                    {
                        string c = checkCode.Substring(i, 1);
                        float fontSize = c == c.ToLower() ? 38 : 28; //小写和大写字母设置不同的字体大小。
                        using (Bitmap imgText = this.DrawString(c, fontColor, new Font(fonts[fontIndex], fontSize, FontStyle.Bold)))
                        {
                            g.DrawImage(imgText, new Point(locationX, -5));
                            locationX += 23;
                        }
                    }

                    //画贝塞尔干拢线。
                    for (int i = 0; i < 2; i++)
                    {
                        g.DrawBezier(
                            new Pen(fontColor, r.Next(1, 3)),
                            new Point(0, r.Next(img.Height)),
                            new Point(40, r.Next(img.Height)),
                            new Point(80, r.Next(img.Height)),
                            new Point(img.Width, r.Next(img.Height))
                        );
                    }

                    //输出到浏览器。
                    using (var ms = new System.IO.MemoryStream())
                    {
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        HttpContext.Current.Response.ClearContent();
                        HttpContext.Current.Response.ContentType = "image/Jpeg";
                        HttpContext.Current.Response.BinaryWrite(ms.ToArray());
                    }
                }
            }
        }

        #endregion

        #region 随机生成验证字符

        /// <summary>
        /// 随机生成验证字符。
        /// </summary>
        /// <param name="VcodeNum">生成字母的个数</param>
        /// <returns>string</returns>
        private string RndNum(int VcodeNum)
        {
            string VNum = ""; //由于字符串很短，就不用StringBuilder了
            int temp = -1; //记录上次随机数值，尽量避免生产几个一样的随机数

            //采用一个简单的算法以保证生成随机数的不同
            Random rand = new Random();
            for (int i = 1; i < VcodeNum + 1; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * unchecked((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(chars.Length);
                if (temp != -1 && temp == t)
                {
                    return RndNum(VcodeNum);
                }
                temp = t;
                VNum += chars[t];
            }
            return VNum;
        }

        #endregion

        #region 旋转图像

        /// <summary>
        /// 旋转图像。
        /// </summary>
        /// <param name="image">The <see cref="System.Drawing.Image"/> to rotate</param>
        /// <param name="angle">The amount to rotate the image, clockwise, in degrees</param>
        /// <returns>A new <see cref="System.Drawing.Bitmap"/> that is just large enough
        /// to contain the rotated image without cutting any corners off.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <see cref="image"/> is null.</exception>
        private Bitmap RotateImage(Image image, float angle)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            const double pi2 = Math.PI / 2.0;

            // Why can't C# allow these to be const, or at least readonly
            // *sigh*  I'm starting to talk like Christian Graus :omg:
            double oldWidth = (double)image.Width;
            double oldHeight = (double)image.Height;

            // Convert degrees to radians
            double theta = ((double)angle) * Math.PI / 180.0;
            double locked_theta = theta;

            // Ensure theta is now [0, 2pi)
            while (locked_theta < 0.0)
                locked_theta += 2 * Math.PI;

            double newWidth, newHeight;
            int nWidth, nHeight; // The newWidth/newHeight expressed as ints

            #region Explaination of the calculations
            /**/
            /*
         * The trig involved in calculating the new width and height
         * is fairly simple; the hard part was remembering that when 
         * PI/2 <= theta <= PI and 3PI/2 <= theta < 2PI the width and 
         * height are switched.
         * 
         * When you rotate a rectangle, r, the bounding box surrounding r
         * contains for right-triangles of empty space.  Each of the 
         * triangles hypotenuse's are a known length, either the width or
         * the height of r.  Because we know the length of the hypotenuse
         * and we have a known angle of rotation, we can use the trig
         * function identities to find the length of the other two sides.
         * 
         * sine = opposite/hypotenuse
         * cosine = adjacent/hypotenuse
         * 
         * solving for the unknown we get
         * 
         * opposite = sine * hypotenuse
         * adjacent = cosine * hypotenuse
         * 
         * Another interesting point about these triangles is that there
         * are only two different triangles. The proof for which is easy
         * to see, but its been too long since I've written a proof that
         * I can't explain it well enough to want to publish it.  
         * 
         * Just trust me when I say the triangles formed by the lengths 
         * width are always the same (for a given theta) and the same 
         * goes for the height of r.
         * 
         * Rather than associate the opposite/adjacent sides with the
         * width and height of the original bitmap, I'll associate them
         * based on their position.
         * 
         * adjacent/oppositeTop will refer to the triangles making up the 
         * upper right and lower left corners
         * 
         * adjacent/oppositeBottom will refer to the triangles making up 
         * the upper left and lower right corners
         * 
         * The names are based on the right side corners, because thats 
         * where I did my work on paper (the right side).
         * 
         * Now if you draw this out, you will see that the width of the 
         * bounding box is calculated by adding together adjacentTop and 
         * oppositeBottom while the height is calculate by adding 
         * together adjacentBottom and oppositeTop.
         */
            #endregion

            double adjacentTop, oppositeTop;
            double adjacentBottom, oppositeBottom;

            // We need to calculate the sides of the triangles based
            // on how much rotation is being done to the bitmap.
            //   Refer to the first paragraph in the explaination above for 
            //   reasons why.
            if ((locked_theta >= 0.0 && locked_theta < pi2) ||
                (locked_theta >= Math.PI && locked_theta < (Math.PI + pi2)))
            {
                adjacentTop = Math.Abs(Math.Cos(locked_theta)) * oldWidth;
                oppositeTop = Math.Abs(Math.Sin(locked_theta)) * oldWidth;

                adjacentBottom = Math.Abs(Math.Cos(locked_theta)) * oldHeight;
                oppositeBottom = Math.Abs(Math.Sin(locked_theta)) * oldHeight;
            }
            else
            {
                adjacentTop = Math.Abs(Math.Sin(locked_theta)) * oldHeight;
                oppositeTop = Math.Abs(Math.Cos(locked_theta)) * oldHeight;

                adjacentBottom = Math.Abs(Math.Sin(locked_theta)) * oldWidth;
                oppositeBottom = Math.Abs(Math.Cos(locked_theta)) * oldWidth;
            }

            newWidth = adjacentTop + oppositeBottom;
            newHeight = adjacentBottom + oppositeTop;

            nWidth = (int)Math.Ceiling(newWidth);
            nHeight = (int)Math.Ceiling(newHeight);

            Bitmap rotatedBmp = new Bitmap(nWidth, nHeight);

            using (Graphics g = Graphics.FromImage(rotatedBmp))
            {
                // This array will be used to pass in the three points that 
                // make up the rotated image
                Point[] points;

                /**/
                /*
             * The values of opposite/adjacentTop/Bottom are referring to 
             * fixed locations instead of in relation to the
             * rotating image so I need to change which values are used
             * based on the how much the image is rotating.
             * 
             * For each point, one of the coordinates will always be 0, 
             * nWidth, or nHeight.  This because the Bitmap we are drawing on
             * is the bounding box for the rotated bitmap.  If both of the 
             * corrdinates for any of the given points wasn't in the set above
             * then the bitmap we are drawing on WOULDN'T be the bounding box
             * as required.
             */
                if (locked_theta >= 0.0 && locked_theta < pi2)
                {
                    points = new Point[] { 
                                             new Point( (int) oppositeBottom, 0 ), 
                                             new Point( nWidth, (int) oppositeTop ),
                                             new Point( 0, (int) adjacentBottom )
                                         };

                }
                else if (locked_theta >= pi2 && locked_theta < Math.PI)
                {
                    points = new Point[] { 
                                             new Point( nWidth, (int) oppositeTop ),
                                             new Point( (int) adjacentTop, nHeight ),
                                             new Point( (int) oppositeBottom, 0 )                         
                                         };
                }
                else if (locked_theta >= Math.PI && locked_theta < (Math.PI + pi2))
                {
                    points = new Point[] { 
                                             new Point( (int) adjacentTop, nHeight ), 
                                             new Point( 0, (int) adjacentBottom ),
                                             new Point( nWidth, (int) oppositeTop )
                                         };
                }
                else
                {
                    points = new Point[] { 
                                             new Point( 0, (int) adjacentBottom ), 
                                             new Point( (int) oppositeBottom, 0 ),
                                             new Point( (int) adjacentTop, nHeight )        
                                         };
                }

                g.DrawImage(image, points);
            }

            return rotatedBmp;
        }

        #endregion
    }
}
