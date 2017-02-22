using System;
using System.Drawing;

namespace ECS.Common.ToolsCollection
{
    /// <summary>
    /// 验证码 继承 System.Web.UI.Page ，Session["xk_validate_code"]
    /// </summary>
    public class ValidateImg : System.Web.UI.Page
    {
        private void Page_Load(object sender, EventArgs e)
        {
            char[] chars = "023456789".ToCharArray();
            System.Random random = new Random();

            string validateCode = string.Empty;
            for (int i = 0; i < 4; i++)
            {
                char rc = chars[random.Next(0, chars.Length)];
                if (validateCode.IndexOf(rc) > -1)
                {
                    i--;
                    continue;
                }
                validateCode += rc;
            }
            Session["xk_validate_code"] = validateCode;
            CreateImage(validateCode);
        }
        /// <summary>
        /// 创建图片
        /// </summary>
        /// <param name="checkCode"></param>
        private void CreateImage(string checkCode)
        {
            int iwidth = (int)(checkCode.Length * 11);
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(iwidth, 19);
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.White);
            //定义颜色
            Color[] c = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Chocolate, Color.Brown, Color.DarkCyan, Color.Purple };
            Random rand = new Random();

            //输出不同字体和颜色的验证码字符
            for (int i = 0; i < checkCode.Length; i++)
            {
                int cindex = rand.Next(7);
                Font f = new System.Drawing.Font("Microsoft Sans Serif", 11);
                Brush b = new System.Drawing.SolidBrush(c[cindex]);
                g.DrawString(checkCode.Substring(i, 1), f, b, (i * 10) + 1, 0, StringFormat.GenericDefault);
            }
            //画一个边框
            g.DrawRectangle(new Pen(Color.Black, 0), 0, 0, image.Width - 1, image.Height - 1);

            //输出到浏览器
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            Response.ClearContent();
            Response.ContentType = "image/Jpeg";
            Response.BinaryWrite(ms.ToArray());
            g.Dispose();
            image.Dispose();
        }

        public static string CreateVerifyCode()
        {
            char[] chars = "023456789".ToCharArray();
            System.Random random = new Random();

            string validateCode = string.Empty;
            for (int i = 0; i < 4; i++)
            {
                char rc = chars[random.Next(0, chars.Length)];
                if (validateCode.IndexOf(rc) > -1)
                {
                    i--;
                    continue;
                }
                validateCode += rc;
            }
            return validateCode;

        }

        public static byte[] CreateVerifyImage(string checkCode)
        {
            int iwidth = 100;//(int)(checkCode.Length * 11);
            int iheight = 42;
            //画验证码字符
            int iLen = checkCode.Length;
            int iCharWidth = iwidth / iLen;

            System.Drawing.Bitmap image = new System.Drawing.Bitmap(iwidth, iheight);
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.White);
            //定义随机颜色                            
            Color[] c = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Chocolate, Color.Brown, Color.DarkCyan, Color.Purple };
            string[] fonts = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };
            Random rand = new Random();

            for (int i = 0; i < 100; i++)
            {
                int x1 = rand.Next(image.Width);
                int x2 = rand.Next(image.Width);
                int y1 = rand.Next(image.Height);
                int y2 = rand.Next(image.Height);
                g.DrawLine(new Pen(Color.LightGray, 2), x1, y1, x2, y2);//根据坐标画线
            }

            //输出不同字体和颜色的验证码字符
            for (int i = 0; i < checkCode.Length; i++)
            {
                //字符
                var item = checkCode.Substring(i, 1);

                //设置随机颜色、字体
                int cindex = rand.Next(7);
                int findex = rand.Next(5);
                Font f = new System.Drawing.Font(fonts[findex], 15, FontStyle.Bold);
                Brush b = new System.Drawing.SolidBrush(c[cindex]);

                //计算字体大小
                //Int32 textFontSize = Convert.ToInt32(f.Size);
                SizeF fSize = new SizeF();
                fSize = g.MeasureString(item, f);
                int iFontWidth = (int)fSize.Width;
                int iFontHeight = (int)fSize.Height;

                //计算标准字符起止宽度
                int iCharXStart = (iCharWidth * i) + iFontWidth / 2;
                int iCharXEnd = iCharWidth * (i + 1);
                int iCharYStart = iFontHeight / 2;
                int iCharYEnd = iheight - iFontHeight / 2;
                //生成单个字符横纵起止坐标
                int iCharX = rand.Next(iCharXStart, iCharXEnd);
                int iCharY = rand.Next(iCharYStart, iCharYEnd);


                //检查当前字符坐标是否字符超过标准字符宽高边距
                if (iCharX + iFontWidth >= iCharWidth * (i + 1))
                {
                    iCharX = iCharX - iFontWidth / 2;
                }
                if (iCharX <= 0)
                {
                    iCharX = iCharX + iFontWidth;
                }
                if (iCharY + iFontHeight >= iheight)
                {
                    iCharY = iCharY - iFontHeight / 2;
                }



                g.DrawString(item, f, b, iCharX, iCharY, StringFormat.GenericDefault);
            }

            //画噪点
            for (int i = 0; i < 100; i++)
            {
                int x = rand.Next(image.Width);

                int y = rand.Next(image.Height);

                Color clr = c[rand.Next(c.Length)];

                image.SetPixel(x, y, clr);
            }


            //画一个边框
            g.DrawRectangle(new Pen(Color.Black, 0), 0, 0, image.Width - 1, image.Height - 1);

            //输出到浏览器
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            g.Dispose();
            image.Dispose();
            return ms.ToArray();
        }
    }
}
