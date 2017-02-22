
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace DotNet.Utilities
{
    public class StringHelper
    {
        /// <summary>
        /// 移除Html标记
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveHtml(string content)
        {
            string regexstr = @"<[^>]*>";
            return Regex.Replace(content, regexstr, string.Empty, RegexOptions.IgnoreCase).Replace("&amp;nbsp;", string.Empty).Replace("&nbsp;", string.Empty);

        }

        /// <summary>
        /// 移除Html标记
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveHtmlSp(string content)
        {
            string regexstr = @"<[^>]*>";
            return Regex.Replace(content, regexstr, string.Empty, RegexOptions.IgnoreCase).Replace("&amp;nbsp;", string.Empty).Replace("&nbsp;", string.Empty).Replace("&amp;quot;", string.Empty).Replace("&lt;", string.Empty).Replace("&gt;", string.Empty);

        }

        /// <summary>
        /// 移除Html标记,并清除其中的内容
        /// </summary>
        /// <param name="content"></param>
        /// <param name="clearInnerText">清除文本内容</param>
        /// <returns></returns>
        public static string RemoveHtml(string content, bool clearInnerText)
        {
            string regexstr = @"<[^>]*/?>(.+?<[^>]*>)?";
            return Regex.Replace(content, regexstr, string.Empty, RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }


        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="content">字符串</param>
        /// <param name="length">截取长度</param>
        /// <param name="replaceChar">追加的符号</param>
        /// <returns></returns>
        public static string SubString(string content, int length, string replaceChar)
        {
            if (content.Length <= length)
            {
                return content;
            }
            else
            {
                return content.Substring(0, length) + replaceChar;
            }
        }

        /// <summary>
        /// 获取字符串从右至左的第一个数字
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static int GetRightFirstNum(string str)
        {
            int lastNum = 0;
            Regex regex = new Regex(@".+(\d).+?", RegexOptions.IgnoreCase);
            Match match = regex.Match(str);
            if (match.Success)
            {
                lastNum = TypeParse.ObjectToInt(match.Groups[1].Value, 1);
            }
            return lastNum;
        }
        static Regex urlRegex = new Regex(@"(?:https?|ftp|file)://[-A-Z0-9+&@#/%?=~_|$!:,.;]*[A-Z0-9+&@#/%=~_|$]", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        static Regex nonPrefixUrlRegex = new Regex(@"(?<!(?:(https?|ftp|file)://|[a-z0-9.\-_@]))([a-z0-9.\-_]+\.(?:com|cn|org|gov|edu|tv|co|net|jp|mob|biz|info|name|cc|hk|公司|网络|中国))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        const string nonPrefixUrlReplacement = "http://$0";
        /// <summary>
        /// 将不带http:// 开头的域名补全http://
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string CompleteUrl(string content)
        {
            return nonPrefixUrlRegex.Replace(content, nonPrefixUrlReplacement);
        }

        #region 过滤 Sql 语句字符串中的注入脚本

        /// <summary>
        /// 过滤 Sql 语句字符串中的注入脚本
        /// zhj
        /// </summary>
        /// <param name="source">传入的字符串</param>
        /// <returns></returns>
        public static string SqlFilter(string source)
        {
            //单引号替换成两个单引号
            source = source.Replace("'", "''");

            //半角封号替换为全角封号，防止多语句执行


            source = source.Replace(";", "；")
                .Replace("&nbsp；", "&nbsp;");

            //半角括号替换为全角括号

            source = source.Replace("(", "（");
            source = source.Replace(")", "）");

            ///////////////要用正则表达式替换，防止字母大小写得情况////////////////////

            //去除执行存储过程的命令关键字
            source = source.Replace("Exec", "");
            source = source.Replace("Execute", "");

            //去除系统存储过程或扩展存储过程关键字
            source = source.Replace("xp_", "x p_");
            source = source.Replace("sp_", "s p_");

            //防止16进制注入
            source = source.Replace("0x", "0 x");

            return source;
        }
        #endregion

        /// <summary>
        /// 字符串转换为Unicode编码,格式为\u....\u....
        /// </summary>
        /// <param name="srcText"></param>
        /// <returns></returns>
        public static string StringToUnicode(string srcText)
        {
            if (string.IsNullOrEmpty(srcText)) return string.Empty;
            StringBuilder dst = new StringBuilder();
            StringBuilder temp = new StringBuilder();
            char[] src = srcText.ToCharArray();
            for (int i = 0; i < src.Length; i++)
            {
                byte[] bytes = Encoding.Unicode.GetBytes(src[i].ToString());
                string str = new StringBuilder(@"\u").Append(bytes[1].ToString("X2")).Append(bytes[0].ToString("X2")).ToString();
                dst.Append(str);
            }
            return dst.ToString();
        }
        /// <summary>
        /// Unicode编码转换为字符串
        /// </summary>
        /// <param name="srcText"></param>
        /// <returns></returns>
        public static string UnicodeToString(string srcText)
        {

            StringBuilder dst = new StringBuilder();
            string[] ss = srcText.Split("\\u".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string src = srcText; int len = srcText.Length / 6;

            for (int i = 0; i < ss.Length; i++)
            {
                byte[] bytes = new byte[2];

                bytes[1] = byte.Parse(int.Parse(ss[i].Substring(0, 2), NumberStyles.HexNumber).ToString());

                bytes[0] = byte.Parse(int.Parse(ss[i].Substring(2, ss[i].Length - 2), NumberStyles.HexNumber).ToString());

                dst.Append(Encoding.Unicode.GetString(bytes));

            }

            return dst.ToString();

        }

        /// <summary>
        /// 从字符串的指定位置截取指定长度的子字符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <param name="length">子字符串的长度</param>
        /// <returns>子字符串</returns>
        public static string CutString(string str, int startIndex, int length)
        {
            if (startIndex >= 0)
            {
                if (length < 0)
                {
                    length = length * -1;
                    if (startIndex - length < 0)
                    {
                        length = startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        startIndex = startIndex - length;
                    }
                }


                if (startIndex > str.Length)
                {
                    return "";
                }


            }
            else
            {
                if (length < 0)
                {
                    return "";
                }
                else
                {
                    if (length + startIndex > 0)
                    {
                        length = length + startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        return "";
                    }
                }
            }

            if (str.Length - startIndex < length)
            {
                length = str.Length - startIndex;
            }

            return str.Substring(startIndex, length);
        }

        #region 全角转半角字符

        private static bool IsBjChar(char c)
        {
            int i = (int)c;
            return i >= 32 && i <= 126;
        }

        private static bool IsQjChar(char c)
        {
            if (c == '\u3000') return true;

            int i = (int)c - 65248;
            if (i < 32) return false;
            return IsBjChar((char)i);
        }
        /// <summary>
        /// 全角字符转换半角字符
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToBj(string s)
        {
            if (s == null || s.Trim() == string.Empty) return s;

            StringBuilder sb = new StringBuilder(s.Length);
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '\u3000')
                    sb.Append('\u0020');
                else if (IsQjChar(s[i]))
                    sb.Append((char)((int)s[i] - 65248));
                else
                    sb.Append(s[i]);
            }

            return sb.ToString();
        }

        #endregion


        /// <summary>
        /// 处理Ueditor编辑器TableBorder的Bug
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string UeditorHandler(string content)
        {
             return content.Replace("style.","style");
          
        }


        /// <summary>
        /// 把字符串按照分隔符转换成 List
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="speater">分隔符</param>
        /// <param name="toLower">是否转换为小写</param>
        /// <returns></returns>
        public static List<string> GetStrArray(string str, char speater, bool toLower)
        {
            if (string.IsNullOrEmpty(str))
            {
                str = "";
            }
            List<string> list = new List<string>();
            string[] ss = str.Split(speater);
            foreach (string s in ss)
            {
                if (!string.IsNullOrEmpty(s) && s != speater.ToString())
                {
                    string strVal = s;
                    if (toLower)
                    {
                        strVal = s.ToLower();
                    }
                    list.Add(strVal);
                }
            }
            return list;
        }

        /// <summary>
        /// 把 List<string> 按照分隔符组装成 string
        /// </summary>
        /// <param name="list"></param>
        /// <param name="speater"></param>
        /// <returns></returns>
        public static string GetArrayStr(List<string> list, string speater)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                {
                    sb.Append(list[i]);
                }
                else
                {
                    sb.Append(list[i]);
                    sb.Append(speater);
                }
            }
            return sb.ToString();
        }
    }
}
