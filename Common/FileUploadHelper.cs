using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using System.Linq;
using System.Xml.Linq;
using System.Configuration;

namespace QHW.Common
{
    /// <summary>
    /// 上传文件操作类
    /// </summary>
    public class FileUploadHelper
    {
        static XElement _FileSaveConfig;
        public static XElement FileSaveConifg
        {
            get
            {
                if (_FileSaveConfig == null)
                {
                    _FileSaveConfig = XElement.Load(HttpContext.Current.Server.MapPath("~/App_Data/FileSavePath.xml"));
                }
                return _FileSaveConfig;
            }
        }
        static string[] _LimitExtensions;
        public static string[] LimitExtensions
        {
            get
            {
                if (_LimitExtensions == null || _LimitExtensions.Length == 0)
                {
                    _LimitExtensions = ConfigurationManager.AppSettings["LimitExtensions"].Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                }
                return _LimitExtensions;
            }
        }
        static int _LimitImageSizeBit = -1;
        public static int LimitImageSizeBit
        {
            get
            {
                if (_LimitImageSizeBit == -1)
                {
                    _LimitImageSizeBit = Common.TypeParse.ObjectToInt(ConfigurationManager.AppSettings["LimitImageSizeBit"], 0);
                }
                return _LimitImageSizeBit;
            }
        }
        public static FileUploadResult FileUpload(HttpPostedFileBase file, out string fileName)
        {

            fileName = Common.FileUploadHelper.GetFileMD5(file.InputStream) + Path.GetExtension(file.FileName);
            string[] saveDirs = Common.GDI.GetMD5Dirs(fileName);
            int loctionNodeCount = Common.TypeParse.ObjectToInt(FileSaveConifg.Attribute("loctionNodeCount").Value, 1);
            var q = FileSaveConifg.Elements("server").FirstOrDefault(a => Convert.ToInt32(saveDirs[0], 16) % loctionNodeCount == Common.TypeParse.ObjectToInt(a.Attribute("number").Value, 0));
            string fileSavePath = Path.Combine(q.Attribute("path").Value, Path.Combine(saveDirs), fileName);
            return Common.FileUploadHelper.FileUpload(file, fileSavePath, LimitImageSizeBit, LimitExtensions);
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file">上传的文件</param>
        /// <param name="savePath">文件存储路径(包含目录与文件名)</param>
        /// <param name="limitSize">限制的文件大小(以字节为单位)</param>
        /// <param name="limitExtensions">限制的文件扩展名</param>
        /// <param name="watermarkPath">水印路径(为空不加水印)</param>
        /// <param name="width">图片的宽度</param>
        /// <param name="height">图片的高度</param>
        /// <returns></returns>
        public static FileUploadResult FileUpload(HttpPostedFileBase file, string savePath, int limitSize, string[] limitExtensions, string watermarkPath, out int width, out int height)
        {
            width = 0;
            height = 0;
            if (string.IsNullOrEmpty(file.FileName)) return FileUploadResult.文件为空;

            if (file.ContentLength > limitSize) return FileUploadResult.文件大小超过限制;
            string extension = Path.GetExtension(file.FileName).ToLower();
            if (Array.IndexOf(limitExtensions, extension) < 0)
            {
                return FileUploadResult.文件扩展名不允许;
            }
            try
            {
                string dir = Path.GetDirectoryName(savePath);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                if (limitExtensions.Contains(extension))
                {
                    System.Drawing.Image img = System.Drawing.Image.FromStream(file.InputStream);
                    width = img.Width;
                    height = img.Height;
                    if (!string.IsNullOrEmpty(watermarkPath) && File.Exists(watermarkPath))
                    {
                        //AddImageWatermark(img, savePath, watermarkPath);
                    }
                    else
                    {
                        file.SaveAs(savePath);
                    }
                }
                else
                {
                    return FileUploadResult.文件扩展名不允许;
                }
                return FileUploadResult.文件上传成功;
            }
            catch (Exception ex)
            {
                File.WriteAllText(HttpContext.Current.Server.MapPath("~/App_Data/error.txt"), ex.Message.ToString());
                return FileUploadResult.文件上传异常;
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file">上传的文件</param>
        /// <param name="savePath">文件存储路径(包含目录与文件名)</param>
        /// <param name="limitSize">限制的文件大小(以字节为单位)</param>
        /// <param name="limitExtensions">限制的文件扩展名</param>
        /// <param name="watermarkPath">水印路径(为空不加水印)</param>
        /// <param name="width">图片保存的最大宽度</param>
        /// <returns></returns>
        public static FileUploadResult FileUpload(HttpPostedFileBase file, string savePath, int limitSize, string[] limitExtensions, string watermarkPath, int maxWidth)
        {
            if (string.IsNullOrEmpty(file.FileName)) return FileUploadResult.文件为空;
            if (file.ContentLength > limitSize) return FileUploadResult.文件大小超过限制;
            string extension = Path.GetExtension(file.FileName).ToLower();
            if (Array.IndexOf(limitExtensions, extension) < 0)
            {
                return FileUploadResult.文件扩展名不允许;
            }
            try
            {
                string dir = Path.GetDirectoryName(savePath);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                if (".jpg,.png,.bmp,.jpeg,.gif".Contains(extension))
                {
                    System.Drawing.Image img = System.Drawing.Image.FromStream(file.InputStream);

                    if (maxWidth < img.Width)
                    {
                        int height = img.Height * maxWidth / img.Width;
                        Bitmap bmp = new Bitmap(maxWidth, height, PixelFormat.Format32bppArgb);
                        Graphics g = Graphics.FromImage(bmp);
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                        g.DrawImage(img, 0, 0, maxWidth, height);
                        g.Dispose();
                        img = bmp;
                    }
                    if (!string.IsNullOrEmpty(watermarkPath) && File.Exists(watermarkPath))
                    {
                        //AddImageWatermark(img, savePath, watermarkPath,"企汇网");
                    }
                    else
                    {
                        file.SaveAs(savePath);

                    }
                }
                else
                {
                    return FileUploadResult.文件扩展名不允许;
                }

                return FileUploadResult.文件上传成功;
            }
            catch (Exception ex)
            {
                File.WriteAllText(HttpContext.Current.Server.MapPath("~/App_Data/error.txt"), ex.Message.ToString());
                return FileUploadResult.文件上传异常;
            }
        }



        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file">上传文件</param>
        /// <param name="savePath">文件存储路径(包含目录与文件名)</param>
        /// <param name="limitSize">限制的文件大小(以字节为单位)</param>
        /// <param name="limitExtensions">限制的文件扩展名</param>
        /// <returns></returns>
        public static FileUploadResult FileUpload(HttpPostedFileBase file, string savePath, int limitSize, string[] limitExtensions)
        {
            if (string.IsNullOrEmpty(file.FileName)) return FileUploadResult.文件为空;
            if (file.ContentLength > limitSize) return FileUploadResult.文件大小超过限制;
            string extension = Path.GetExtension(file.FileName).ToLower();
            if (Array.IndexOf(limitExtensions, extension) < 0)
            {
                return FileUploadResult.文件扩展名不允许;
            }
            try
            {
                string dir = Path.GetDirectoryName(savePath);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                file.SaveAs(savePath);

                return FileUploadResult.文件上传成功;
            }
            catch (Exception ex)
            {
                File.WriteAllText(HttpContext.Current.Server.MapPath("~/App_Data/error.txt"), ex.Message.ToString());
                return FileUploadResult.文件上传异常;
            }


        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file">上传文件</param>
        /// <param name="savePath">文件存储路径(包含目录与文件名)</param>
        /// <param name="limitSize">限制的文件大小(以字节为单位)</param>
        /// <param name="limitExtensions">限制的文件扩展名</param>
        /// <returns></returns>
        public static Common.FileUploadResult FileUpload(byte[] stream, string filename, string savePath, int limitSize, string[] limitExtensions)
        {
            if (string.IsNullOrEmpty(filename)) return Common.FileUploadResult.文件为空;
            if (stream.Length > limitSize) return Common.FileUploadResult.文件大小超过限制;
            string extension = Path.GetExtension(filename).ToLower();
            if (Array.IndexOf(limitExtensions, extension) < 0)
            {
                return Common.FileUploadResult.文件扩展名不允许;
            }
            try
            {
                string dir = Path.GetDirectoryName(savePath);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                using (FileStream pFileStream = new FileStream(savePath, FileMode.OpenOrCreate))
                {
                    pFileStream.Write(stream, 0, stream.Length);
                }

                return Common.FileUploadResult.文件上传成功;
            }
            catch (Exception ex)
            {
                File.WriteAllText(HttpContext.Current.Server.MapPath("~/App_Data/error.txt"), ex.Message.ToString());
                return Common.FileUploadResult.文件上传异常;
            }
        }


        /// <summary>
        /// 增加图片文字水印
        /// </summary>
        /// <param name="img">要添加水印的图片</param>
        /// <param name="savePath">保存文件名</param>
        /// <param name="watermarkText">水印文字</param>
        /// <param name="watermarkStatus">图片水印位置</param>
        /// <param name="fontname">水印文字字体</param>
        /// <param name="fontsize">水印文字大小</param>
        public static void AddImageSignText(System.Drawing.Image img, string savePath, string watermarkText, int watermarkStatus, string fontname, int fontsize)
        {
            Graphics g = Graphics.FromImage(img);
            Font drawFont = new Font(fontname, fontsize, FontStyle.Regular, GraphicsUnit.Pixel);
            SizeF crSize;
            crSize = g.MeasureString(watermarkText, drawFont);

            float xpos = 0;
            float ypos = 0;

            switch (watermarkStatus)
            {
                case 1:
                    xpos = (float)img.Width * (float).01;
                    ypos = (float)img.Height * (float).01;
                    break;
                case 2:
                    xpos = ((float)img.Width * (float).50) - (crSize.Width / 2);
                    ypos = (float)img.Height * (float).01;
                    break;
                case 3:
                    xpos = ((float)img.Width * (float).99) - crSize.Width;
                    ypos = (float)img.Height * (float).01;
                    break;
                case 4:
                    xpos = (float)img.Width * (float).01;
                    ypos = ((float)img.Height * (float).50) - (crSize.Height / 2);
                    break;
                case 5:
                    xpos = ((float)img.Width * (float).50) - (crSize.Width / 2);
                    ypos = ((float)img.Height * (float).50) - (crSize.Height / 2);
                    break;
                case 6:
                    xpos = ((float)img.Width * (float).99) - crSize.Width;
                    ypos = ((float)img.Height * (float).50) - (crSize.Height / 2);
                    break;
                case 7:
                    xpos = (float)img.Width * (float).01;
                    ypos = ((float)img.Height * (float).99) - crSize.Height;
                    break;
                case 8:
                    xpos = ((float)img.Width * (float).50) - (crSize.Width / 2);
                    ypos = ((float)img.Height * (float).99) - crSize.Height;
                    break;
                case 9:
                    xpos = ((float)img.Width * (float).99) - crSize.Width;
                    ypos = ((float)img.Height * (float).99) - crSize.Height;
                    break;
            }

            g.DrawString(watermarkText, drawFont, new SolidBrush(Color.White), xpos + 1, ypos + 1);
            g.DrawString(watermarkText, drawFont, new SolidBrush(Color.Black), xpos, ypos);

            ImageCodecInfo ici = GDIHelper.GetEncoder(img.RawFormat);
            EncoderParameters encoderParams = GDIHelper.LosslessParam;

            if (ici != null)
            {
                img.Save(savePath, ici, encoderParams);
            }
            else
            {
                img.Save(savePath);
            }
            g.Dispose();
            img.Dispose();
        }

        /// <summary>
        /// 加图片水印
        /// </summary>
        /// <param name="img">要处理的图片</param>
        /// <param name="savePath">文件存储路径(包含目录与文件名)</param>
        /// <param name="watermarkPath">水印文件名</param>
        static void AddImageWatermark(System.Drawing.Image img, string savePath, string watermarkPath, string defaultWatermarkPText)
        {
            int watermarkStatus = 9;//水印位置
            int watermarkTransparency = 5;//水印透明度
            Graphics g;
            try
            {
                g = Graphics.FromImage(img);
            }
            catch (Exception)
            {
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(img.Width, img.Height);
                g = System.Drawing.Graphics.FromImage(bitmap);
                g.DrawImage(img, 0, 0, img.Width, img.Height);
            }
            //设置高质量插值法
            //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度
            //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            System.Drawing.Image watermark = new Bitmap(watermarkPath);

            if (watermark.Height >= img.Height || watermark.Width >= img.Width)
            {
                try
                {
                    AddImageSignText(img, savePath, defaultWatermarkPText, watermarkStatus, "Tahoma", 12);
                    return;
                }
                catch (Exception)
                {
                }
            }

            ImageAttributes imageAttributes = new ImageAttributes();
            ColorMap colorMap = new ColorMap();

            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
            ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

            float transparency = 0.5F;
            if (watermarkTransparency >= 1 && watermarkTransparency <= 10)
            {
                transparency = (watermarkTransparency / 10.0F);
            }

            float[][] colorMatrixElements = {
												new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
												new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
												new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
												new float[] {0.0f,  0.0f,  0.0f,  transparency, 0.0f},
												new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
											};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            int xpos = 0;
            int ypos = 0;

            switch (watermarkStatus)
            {
                case 1:
                    xpos = (int)(img.Width * (float).01);
                    ypos = (int)(img.Height * (float).01);
                    break;
                case 2:
                    xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                    ypos = (int)(img.Height * (float).01);
                    break;
                case 3:
                    xpos = (int)((img.Width * (float).99) - (watermark.Width));
                    ypos = (int)(img.Height * (float).01);
                    break;
                case 4:
                    xpos = (int)(img.Width * (float).01);
                    ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                    break;
                case 5:
                    xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                    ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                    break;
                case 6:
                    xpos = (int)((img.Width * (float).99) - (watermark.Width));
                    ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                    break;
                case 7:
                    xpos = (int)(img.Width * (float).01);
                    ypos = (int)((img.Height * (float).99) - watermark.Height);
                    break;
                case 8:
                    xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                    ypos = (int)((img.Height * (float).99) - watermark.Height);
                    break;
                case 9:
                    xpos = (int)((img.Width * (float).99) - (watermark.Width));
                    ypos = (int)((img.Height * (float).99) - watermark.Height);
                    break;
            }

            g.DrawImage(watermark, new Rectangle(xpos, ypos, watermark.Width, watermark.Height), 0, 0, watermark.Width, watermark.Height, GraphicsUnit.Pixel, imageAttributes);
            img.Save(savePath, GDIHelper.GetEncoder(ImageFormat.Jpeg), GDIHelper.LosslessParam);

            g.Dispose();
            img.Dispose();
            watermark.Dispose();
            imageAttributes.Dispose();
        }
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="orginalImagePat">原图片地址</param>
        /// <param name="thumNailPath">缩略图地址</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="model">生成缩略的模式,</param>
        public static void MakeThumNail(string originalImagePath, string thumNailPath, int width, int height, string model)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);


            int thumWidth = width;      //缩略图的宽度
            int thumHeight = height;    //缩略图的高度

            int x = 0;
            int y = 0;

            int originalWidth = originalImage.Width;    //原始图片的宽度
            int originalHeight = originalImage.Height;  //原始图片的高度

            switch (model.ToLower())
            {
                case "hw":      //指定高宽缩放,可能变形
                    break;
                case "w":       //指定宽度,高度按照比例缩放
                    thumHeight = originalImage.Height * width / originalImage.Width;
                    break;
                case "h":       //指定高度,宽度按照等比例缩放
                    thumWidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "cut":
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)thumWidth / (double)thumHeight)
                    {
                        originalHeight = originalImage.Height;
                        originalWidth = originalImage.Height * thumWidth / thumHeight;
                        y = 0;
                        x = (originalImage.Width - originalWidth) / 2;
                    }
                    else
                    {
                        originalWidth = originalImage.Width;
                        originalHeight = originalWidth * height / thumWidth;
                        x = 0;
                        y = (originalImage.Height - originalHeight) / 2;
                    }
                    break;
                case "qhw": //企汇网金铺缩略图，缩略图比例与原图一致；原图宽大于高时,缩略图实际的宽等于缩略图标准宽;否则缩略图实际的高等于缩略图标准高
                    if (originalWidth > originalHeight)
                    {
                        if (originalWidth > width) //如果原图的宽小于标准宽不压缩
                        {
                            thumWidth = width;
                            thumHeight = (int)(originalHeight * width / originalWidth);
                        }
                    }
                    else
                    {
                        if (originalHeight > height)
                        {
                            thumHeight = height;
                            thumWidth = thumHeight * originalWidth / originalHeight;
                        }
                    }
                    if (originalWidth <= width && originalHeight <= height)
                    {
                        thumWidth = originalWidth;
                        thumHeight = originalHeight;
                    }

                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(thumWidth, thumHeight);

            //新建一个画板
            System.Drawing.Graphics graphic = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量查值法
            graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量，低速度呈现平滑程度
            graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            graphic.Clear(System.Drawing.Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            graphic.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, thumWidth, thumHeight), new System.Drawing.Rectangle(x, y, originalWidth, originalHeight), System.Drawing.GraphicsUnit.Pixel);

            try
            {
                string dir = Path.GetDirectoryName(thumNailPath);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                bitmap.Save(thumNailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                graphic.Dispose();
            }

        }


        /// <summary>
        /// 获取文件二进制内容的MD5校验码作为文件的名称
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <returns>校验码</returns>
        public static string GetFileMD5(Stream stream)
        {
            try
            {
                System.Security.Cryptography.MD5CryptoServiceProvider get_md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] hash_byte = get_md5.ComputeHash(stream);
                string resule = System.BitConverter.ToString(hash_byte);
                resule = resule.Replace("-", "");
                return resule;
            }
            catch (Exception e)
            {
                return e.ToString();

            }
        }

    }
}
