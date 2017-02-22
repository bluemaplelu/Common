using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet.Utilities
{
    public class QRCodeHelper
    {
        /// <summary>
        /// 企汇网生成二维码公共方法
        /// </summary>
        /// <param name="str">扫描二维码时显示的文字内容</param>
        /// <returns></returns>
        public static string QRCodeUrl(string str)
        {
            return QRCodeUrl(str, 12);
        }
        public static string QRCodeUrl(string str, int codeSize)
        {
            return string.Format("http://qr.yangdou.com/{1}/{0}.jpg", Common.Encrypt.EncryptDES(str, "YDQR1219"), codeSize);
        }
        public static string DESQRCode(string str)
        {
            return Common.Encrypt.DecryptDES(str, "YDQR1219");
        }
    }
}
