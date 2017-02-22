namespace QHW.Common
{
    /// <summary>
    /// 正则验证
    /// 作者：李赟
    /// 时间：2016-01-22
    /// </summary>
    public class Regexlib
    {
        /// <summary>
        /// 非中文字符的一些特殊符号的匹配  
        /// </summary>
        public const string SpecialTextString = @"[&\|\\\*^%$#@\-/<>]";

        public const string UriTextString = @"^(http|https)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*$";

        /// <summary>
        /// 邮箱的正则
        /// </summary>
        public const string EmailRegexString = @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

        /// <summary>
        /// 移动电话的正则  [1]+[3,5,7,8]+\d{9}
        /// </summary>
        public const string MobileRegexString = @"^(((13[0-9]{1})|(15[0-9]{1})|(18[0-9]{1})|(17[0-9]{1}))+\d{8})$";

        /// <summary>
        /// 固定电话的正则
        /// </summary>
        public const string TelphoneRegexString = @"^(\d{3,4}-)?\d{6,8}$";
        /// <summary>
        /// 邮政编码正则
        /// </summary>
        public const string PostalCodeRegexString = @"^[1-9]\d{5}$";
        /// <summary>
        /// 联系方式的正则（包含移动和固定电话）
        /// </summary>
        public const string ContactNoRegexString = MobileRegexString + "|" + TelphoneRegexString;

        /// <summary>
        /// 用户名正则（英文字母和数字组成的4-16位字符，英文字母开头）
        /// </summary>
        public const string AdminRegexString = @"^[a-zA-Z][a-zA-Z0-9]{4,15}$";
        /// <summary>
        /// 判断字符串是否是a-zA-Z0-9_范围内（3-50位范围内）
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static bool IsValidUserName(string strIn)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(strIn, @"^[A-Za-z0-9_]{3,50}$");
        }
        /// <summary>
        /// 验证邮箱
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static bool IsValidEmail(string strIn)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(strIn, EmailRegexString);
        }
        /// <summary>
        /// 验证手机
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static bool IsValidMobile(string strIn)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(strIn, MobileRegexString);
        }
        /// <summary>
        /// 验证手机号
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static bool IsValidTelPhone(string strIn)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(strIn, TelphoneRegexString);

        }

        /// <summary>
        /// 验证用户名
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static bool IsValidAdmin(string strIn)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(strIn, AdminRegexString);

        }
        public static string GetNormalText(string strIn)
        {
            return System.Text.RegularExpressions.Regex.Replace(strIn, @"[&\|\\\*^%$#@\-/<>]", "");
        }
    }
}
