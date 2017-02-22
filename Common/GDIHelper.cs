using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;

namespace QHW.Common
{
    public class GDIHelper
    {
        static GDIHelper()
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {

                if (codec.FormatID == ImageFormat.Bmp.Guid)
                {
                    codecDict.Add(ImageFormat.Bmp, codec);
                    continue;
                }
                if (codec.FormatID == ImageFormat.Emf.Guid)
                {
                    codecDict.Add(ImageFormat.Emf, codec);
                    continue;
                }
                if (codec.FormatID == ImageFormat.Exif.Guid)
                {
                    codecDict.Add(ImageFormat.Exif, codec);
                    continue;
                }
                if (codec.FormatID == ImageFormat.Gif.Guid)
                {
                    codecDict.Add(ImageFormat.Gif, codec);
                    continue;
                }
                if (codec.FormatID == ImageFormat.Icon.Guid)
                {
                    codecDict.Add(ImageFormat.Icon, codec);
                    continue;
                }
                if (codec.FormatID == ImageFormat.Jpeg.Guid)
                {
                    codecDict.Add(ImageFormat.Jpeg, codec);
                    continue;
                }
                if (codec.FormatID == ImageFormat.MemoryBmp.Guid)
                {
                    codecDict.Add(ImageFormat.MemoryBmp, codec);
                    continue;
                }
                if (codec.FormatID == ImageFormat.Png.Guid)
                {
                    codecDict.Add(ImageFormat.Png, codec);
                    continue;
                }
                if (codec.FormatID == ImageFormat.Tiff.Guid)
                {
                    codecDict.Add(ImageFormat.Tiff, codec);
                    continue;
                }
                if (codec.FormatID == ImageFormat.Wmf.Guid)
                {
                    codecDict.Add(ImageFormat.Wmf, codec);
                    continue;
                }

            }
            LosslessParam = new EncoderParameters(3);
            LosslessParam.Param[1] = new EncoderParameter(System.Drawing.Imaging.Encoder.RenderMethod, (int)System.Drawing.Imaging.EncoderValue.RenderProgressive);
            LosslessParam.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
            LosslessParam.Param[2] = new EncoderParameter(System.Drawing.Imaging.Encoder.RenderMethod, (int)System.Drawing.Imaging.EncoderValue.ScanMethodInterlaced);
        }

        static Dictionary<ImageFormat, ImageCodecInfo> codecDict = new Dictionary<ImageFormat, ImageCodecInfo>();
        /// <summary>
        /// 获取无损压缩参数
        /// </summary>
        public static EncoderParameters LosslessParam { get; private set; }
        /// <summary>
        /// 获取图形编码器信息
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            if (codecDict.ContainsKey(format)) return codecDict[format];
            else return codecDict[ImageFormat.Png];
        }
    }
}
