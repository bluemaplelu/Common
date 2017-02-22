using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;
using System.Web;
using System.IO;

namespace QHW.Common
{
    public class GDI
    {
        public static string[] GetMD5Dirs(string MD5)
        {
            string[] dirs = new string[3];
            //string firstChars = fileName.Substring(0, 2);
            dirs[0] = MD5.Substring(0, 2);
            dirs[1] = MD5.Substring(3, 1);
            dirs[2] = (int.Parse(MD5.Substring(4, 5), System.Globalization.NumberStyles.HexNumber) % 1024).ToString();
            return dirs;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="photoUrl"></param>
        /// <param name="height"></param>
        /// <param name="width"></param>
        /// <param name="saveType">0:default;1:fill,2:max</param>
        /// <returns></returns>
        static string GetImgHandlerUrl(string photoUrl, int? height, int width, int saveType, int? quality, bool isUseRealUrlResize)
        {
            if (string.IsNullOrWhiteSpace(photoUrl))
            {
                return string.Format("http://img.shipinhui.cc/error-{0}.jpg", width);
            }
            string oldUrl = photoUrl;
            bool isMD5Url = Regex.IsMatch(photoUrl, @"[0-9a-fA-F]{32,32}\.\w+");
            string subfix = "jpg";
            if (isMD5Url)
            {
                photoUrl = Path.GetFileNameWithoutExtension(photoUrl);
            }
            else if (photoUrl.ToLower().StartsWith("http://"))
            {
                if (isUseRealUrlResize)
                {
                    photoUrl = GetImgRealUrl(photoUrl);
                }
                photoUrl = photoUrl.Replace("http://", "");
                int subfixIndex = photoUrl.LastIndexOf('.');
                if (subfixIndex > 0)
                {
                    subfix = photoUrl.Remove(0, photoUrl.LastIndexOf('.') + 1);
                    photoUrl = photoUrl.Replace("http://", "").Remove(photoUrl.LastIndexOf('.'));
                }
            }
            else
            {
                return null;
            }
            switch (saveType)
            {
                case 0://限制宽高
                    photoUrl = string.Format("{0}_{1}_{2}", photoUrl, width, height ?? 0);
                    break;
                case 1://填充
                    photoUrl = string.Format("{0}_{1}_{2}_fill", photoUrl, width, height ?? 0);
                    break;
                case 2://最大宽
                    photoUrl = string.Format("{0}_{1}_{2}_max", photoUrl, width, height ?? 0);
                    break;
                case 3://最大高
                    photoUrl = string.Format("{0}_{1}_{2}_maxh", photoUrl, width, height ?? 0);
                    break;
                case 4://最大宽
                    photoUrl = string.Format("{0}_{1}_{2}_maxw", photoUrl, width, height ?? 0);
                    break;
            }
            if (!isMD5Url)
            {
                photoUrl = string.Format("V3{0}_{1}", photoUrl, subfix);
            }
            if (quality.HasValue)
            {
                photoUrl = string.Format("{0}_{1}", photoUrl, quality.Value);
            }
            return GetImgAPP(oldUrl) + photoUrl + ".jpg";

        }
        static string GetImgHandlerUrl(string photoUrl, int? height, int width, int saveType, int? quality)
        {
            return GetImgHandlerUrl(photoUrl, height, width, saveType, quality, true);
        }
        /// <summary>
        /// 获取原图片地址
        /// </summary>
        /// <param name="photoUrl"></param>
        /// <returns></returns>
        public static string GetImgRealUrl(string photoUrl)
        {
            if (string.IsNullOrEmpty(photoUrl))
            {
                return "";
            }
            Regex reg = new Regex(@".+?V3(?<url>.*)_(?<width>\d+)_(?<height>\d+)(_(?<isfill>fill|max|maxh))?_(?<suffix>[a-zA-Z]+)(_(?<QualityValue>\d+))?");
            int subfixIndex = photoUrl.LastIndexOf(".");
            if (subfixIndex > 0)
            {
                if (reg.IsMatch(photoUrl.Remove(subfixIndex)))
                {
                    GroupCollection gc = reg.Match(photoUrl).Groups;
                    List<string> arr = new List<string>();
                    photoUrl = "http://" + gc["url"].Value + "." + gc["suffix"].Value;
                }
            }
            else { photoUrl = ""; }
            return photoUrl;
        }
        /// <summary>
        /// 重新设置图片大小
        /// </summary>
        /// <param name="photoUrl">原有图片路径</param>
        /// <param name="height">期望高</param>
        /// <param name="width">期望宽</param>
        /// <returns>重新设置图片后路径</returns>
        public static string ResizeImage(string photoUrl, int height, int width)
        {
            //photoUrl = GetUploadWebUrl(photoUrl);
            string originalUrl = GetImgHandlerUrl(photoUrl, height, width, 0, null);
            return originalUrl;

        }

        /// <summary>
        /// 图片最大宽度
        /// </summary>
        public static int ImageMaxWidth
        {
            get
            {
                int width = 530;
                int.TryParse(ConfigurationManager.AppSettings["ImageMaxWidth"], out width);
                return width;
            }
        }

        /// <summary>
        /// 重新设置图片大小
        /// </summary>
        /// <param name="photoUrl">原有图片路径</param>
        /// <param name="maxWidth">图片最大边的长度</param>
        /// <returns></returns>
        public static string ResizeImage(string photoUrl, int maxWidth)
        {

            //photoUrl = GetUploadWebUrl(photoUrl);
            return GetImgHandlerUrl(photoUrl, null, maxWidth, 2, null);
        }
        public static string ResizeMaxHeightImage(string photoUrl, int maxHeight)
        {
            //photoUrl = GetUploadWebUrl(photoUrl);
            string originalUrl = GetImgHandlerUrl(photoUrl, maxHeight, 0, 3, null, false);
            return originalUrl;
        }
        public static string ResizeMaxWidthImage(string photoUrl, int maxWidth)
        {
            //photoUrl = GetUploadWebUrl(photoUrl);
            string originalUrl = GetImgHandlerUrl(photoUrl, 0, maxWidth, 4, null, false);
            return originalUrl;
        }
        /// <summary>
        /// 重新设置图片大小
        /// </summary>
        /// <param name="photoUrl">原有图片路径</param>
        /// <param name="longestBorder">最长边</param>
        /// <param name="isFill">是否以白色背景填充</param>
        /// <returns>重新设置图片后路径</returns>
        public static string ResizeImage(string photoUrl, int longestBorder, bool isFill)
        {
            //photoUrl = GetUploadWebUrl(photoUrl);

            return GetImgHandlerUrl(photoUrl, null, longestBorder, isFill ? 1 : 0, null);
        }
        public static string GetImgFullURL(string photoUrl)
        {
            if (photoUrl == null)
            {
                return "http://img.shipinhui.cc/error-100.jpg";
            }
            bool isMD5Url = Regex.IsMatch(photoUrl, @"[0-9a-fA-F]{32,32}\.\w+");
            if (isMD5Url)
            {
                string oldStr = photoUrl.Substring(photoUrl.IndexOf(".")+1, photoUrl.Length - (photoUrl.IndexOf(".") + 1));
                return string.Format("{0}{1}", GetImgAPP(photoUrl), photoUrl.Replace(oldStr, "jpg"));
            }
            return "http://img.shipinhui.cc/error-100.jpg";
        }
        public static string GetImgAPP(string url)
        {
            //string key = MD5.Md5En(url).Substring(1, 1);
            //int hostNum = int.Parse(key, System.Globalization.NumberStyles.HexNumber) % 4;
            //return string.Format("http://img{0}.yangdou.com/", hostNum == 0 ? "" : hostNum.ToString());
            return "http://img.shipinhui.cc/";
        }
    }
}
