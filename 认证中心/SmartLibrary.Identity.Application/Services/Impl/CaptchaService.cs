using Furion.DependencyInjection;
using Furion.DistributedIDGenerator;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SmartLibrary.Identity.Application.Dtos.Captcha;
using SmartLibrary.Identity.Application.Services.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Font = SixLabors.Fonts.Font;

namespace SmartLibrary.Identity.Application.Services.Impl
{
    /// <summary>
    /// 验证码生成
    /// </summary>
    public class CaptchaService : ICaptchaService, IScoped
    {
        private const string Numbers = "1,2,3,4,5,6,7,8,9";
        private const string Letters = "1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,J,K,L,M,N,P,Q,R,S,T,U,V,W,X,Y,Z";
        private Font[] _fontArr;
        private readonly int _fontSize = 23;

        public CaptchaService()
        {
            initFonts(_fontSize);
        }
        private string GenerateRandomChapter(string letters, int codeLength = 4)
        {
            var array = letters.Split(new[] { ',' });

            var random = new Random();

            var temp = -1;

            var captcheCode = string.Empty;

            for (int i = 0; i < codeLength; i++)
            {
                if (temp != -1)
                    random = new Random(i * temp * unchecked((int)DateTime.Now.Ticks));

                var index = random.Next(array.Length);

                if (temp != -1 && temp == index)
                    return GenerateRandomChapter(letters, codeLength);

                temp = index;

                captcheCode += array[index];
            }

            return captcheCode;
        }

        private CaptchaResultDto GenerateImg(string captchaCode, int width = 0, int height = 30)
        {
            var random = new Random();

            //验证码颜色集合
            Color[] c = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };

            Image<Rgba32> img = new Image<Rgba32>(width == 0 ? captchaCode.Length * 25 : width, height);

            img.Mutate(g =>
            {
                g.BackgroundColor(Color.White);
                for (var i = 0; i < captchaCode.Length; i++)
                {

                    using (var img2 = new Image<Rgba32>(25, _fontSize))
                    {
                        var point = new Point(i * 25, 0);
                        //随机颜色索引值
                        var cindex = random.Next(c.Length);
                        //颜色  
                        var b = Brushes.Solid(c[cindex]);

                        //随机字体索引值
                        var findex = random.Next(_fontArr.Length);

                        //字体
                        var f = new Font(_fontArr[findex], _fontSize, FontStyle.Bold);
                        img2.Mutate(ctx =>
                        {
                            ctx.DrawText(captchaCode[i].ToString(), f, b, new PointF(0, 0)).Rotate(random.Next(-30, 30));
                        });
                        g.DrawImage(img2, point, 1);
                        var points = new List<PointF> { new PointF(0, random.Next(0, height)) };

                        getCirclePoginF(width == 0 ? captchaCode.Length * 25 : width, height, 5, ref points);
                        points.Add(new PointF(width == 0 ? captchaCode.Length * 25 : width, random.Next(0, height)));
                        g.DrawLines(b, 1, points.ToArray());
                    }
                }


            });
            var ms = new MemoryStream();
            img.SaveAsPng(ms);
            var imgBase64 = Convert.ToBase64String(ms.GetBuffer());

            img.Dispose();
            ms.Dispose();
            ms.Close();

            return new CaptchaResultDto
            {
                Code = captchaCode,
                ImageBase64 = imgBase64,
            };
        }


        /// <summary>
        /// 散 随机点
        /// </summary>
        /// <param name="containerWidth"></param>
        /// <param name="containerHeight"></param>
        /// <param name="lapR"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private static PointF getCirclePoginF(int containerWidth, int containerHeight, double lapR, ref List<PointF> list)
        {
            Random random = new Random();
            PointF newPoint = new PointF();
            int retryTimes = 20;
            double tempDistance = 0;

            do
            {
                newPoint.X = random.Next(0, containerWidth);
                newPoint.Y = random.Next(0, containerHeight);
                bool tooClose = false;
                foreach (var p in list)
                {
                    tooClose = false;
                    tempDistance = Math.Sqrt((Math.Pow((p.X - newPoint.X), 2) + Math.Pow((p.Y - newPoint.Y), 2)));
                    if (tempDistance < lapR)
                    {
                        tooClose = true;
                        break;
                    }
                }
                if (tooClose == false)
                {
                    list.Add(newPoint);
                    break;
                }
            }
            while (retryTimes-- > 0);

            if (retryTimes <= 0)
            {
                list.Add(newPoint);
            }
            return newPoint;
        }

        /// <summary>
        /// 生成随机数字验证码
        /// </summary>
        /// <param name="codeLength"></param>
        /// <returns></returns>
        public string GenerateRandomNum(int codeLength = 4)
        {
            return GenerateRandomChapter(Numbers, codeLength);
        }
        /// <summary>
        /// 生成随机数字+字符验证码
        /// </summary>
        /// <param name="codeLength"></param>
        /// <returns></returns>
        public string GenerateRandomChapter(int codeLength = 4)
        {
            return GenerateRandomChapter(Letters, codeLength);
        }

        /// <summary>
        /// 生成随机数字验证码图片
        /// </summary>
        /// <param name="codeLength"></param>
        /// <returns></returns>
        public CaptchaResultDto GenerateRandomNumImg(int codeLength = 4)
        {
            var code = GenerateRandomNum(codeLength);
            return GenerateImg(code);
        }


        /// <summary>
        /// 生成随机数字+字符验证码图片
        /// </summary>
        /// <param name="codeLength"></param>
        /// <returns></returns>
        public CaptchaResultDto GenerateRandomChapterImg(int codeLength = 4)
        {
            var code = GenerateRandomChapter(codeLength);
            return GenerateImg(code);
        }

        /// <summary>
        /// 初始化字体池
        /// </summary>
        /// <param name="fontSize">一个初始大小</param>
        private void initFonts(int fontSize)
        {
            if (_fontArr == null)
            {
                var assembly = Assembly.GetExecutingAssembly();
                var names = assembly.GetManifestResourceNames();

                if (names?.Length > 0 == true)
                {
                    var fontList = new List<Font>();
                    var fontCollection = new FontCollection();

                    foreach (var name in names.Where(x => x.EndsWith(".ttf")))
                    {
                        var resource = assembly.GetManifestResourceInfo(name);
                        fontList.Add(new Font(fontCollection.Add(assembly.GetManifestResourceStream(name)), fontSize));
                    }

                    _fontArr = fontList.ToArray();
                }
                else
                {
                    throw new Exception($"绘制验证码字体文件加载失败");
                }
            }
        }
    }
}
