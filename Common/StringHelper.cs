
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
        /// �Ƴ�Html���
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveHtml(string content)
        {
            string regexstr = @"<[^>]*>";
            return Regex.Replace(content, regexstr, string.Empty, RegexOptions.IgnoreCase).Replace("&amp;nbsp;", string.Empty).Replace("&nbsp;", string.Empty);

        }

        /// <summary>
        /// �Ƴ�Html���
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveHtmlSp(string content)
        {
            string regexstr = @"<[^>]*>";
            return Regex.Replace(content, regexstr, string.Empty, RegexOptions.IgnoreCase).Replace("&amp;nbsp;", string.Empty).Replace("&nbsp;", string.Empty).Replace("&amp;quot;", string.Empty).Replace("&lt;", string.Empty).Replace("&gt;", string.Empty);

        }

        /// <summary>
        /// �Ƴ�Html���,��������е�����
        /// </summary>
        /// <param name="content"></param>
        /// <param name="clearInnerText">����ı�����</param>
        /// <returns></returns>
        public static string RemoveHtml(string content, bool clearInnerText)
        {
            string regexstr = @"<[^>]*/?>(.+?<[^>]*>)?";
            return Regex.Replace(content, regexstr, string.Empty, RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }


        /// <summary>
        /// ��ȡ�ַ���
        /// </summary>
        /// <param name="content">�ַ���</param>
        /// <param name="length">��ȡ����</param>
        /// <param name="replaceChar">׷�ӵķ���</param>
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
        /// ��ȡ�ַ�����������ĵ�һ������
        /// </summary>
        /// <param name="str">�ַ���</param>
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
        static Regex nonPrefixUrlRegex = new Regex(@"(?<!(?:(https?|ftp|file)://|[a-z0-9.\-_@]))([a-z0-9.\-_]+\.(?:com|cn|org|gov|edu|tv|co|net|jp|mob|biz|info|name|cc|hk|��˾|����|�й�))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        const string nonPrefixUrlReplacement = "http://$0";
        /// <summary>
        /// ������http:// ��ͷ��������ȫhttp://
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string CompleteUrl(string content)
        {
            return nonPrefixUrlRegex.Replace(content, nonPrefixUrlReplacement);
        }

        #region ���� Sql ����ַ����е�ע��ű�

        /// <summary>
        /// ���� Sql ����ַ����е�ע��ű�
        /// zhj
        /// </summary>
        /// <param name="source">������ַ���</param>
        /// <returns></returns>
        public static string SqlFilter(string source)
        {
            //�������滻������������
            source = source.Replace("'", "''");

            //��Ƿ���滻Ϊȫ�Ƿ�ţ���ֹ�����ִ��


            source = source.Replace(";", "��")
                .Replace("&nbsp��", "&nbsp;");

            //��������滻Ϊȫ������

            source = source.Replace("(", "��");
            source = source.Replace(")", "��");

            ///////////////Ҫ��������ʽ�滻����ֹ��ĸ��Сд�����////////////////////

            //ȥ��ִ�д洢���̵�����ؼ���
            source = source.Replace("Exec", "");
            source = source.Replace("Execute", "");

            //ȥ��ϵͳ�洢���̻���չ�洢���̹ؼ���
            source = source.Replace("xp_", "x p_");
            source = source.Replace("sp_", "s p_");

            //��ֹ16����ע��
            source = source.Replace("0x", "0 x");

            return source;
        }
        #endregion

        /// <summary>
        /// �ַ���ת��ΪUnicode����,��ʽΪ\u....\u....
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
        /// Unicode����ת��Ϊ�ַ���
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
        /// ���ַ�����ָ��λ�ý�ȡָ�����ȵ����ַ���
        /// </summary>
        /// <param name="str">ԭ�ַ���</param>
        /// <param name="startIndex">���ַ�������ʼλ��</param>
        /// <param name="length">���ַ����ĳ���</param>
        /// <returns>���ַ���</returns>
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

        #region ȫ��ת����ַ�

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
        /// ȫ���ַ�ת������ַ�
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
        /// ����Ueditor�༭��TableBorder��Bug
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string UeditorHandler(string content)
        {
             return content.Replace("style.","style");
          
        }


        /// <summary>
        /// ���ַ������շָ���ת���� List
        /// </summary>
        /// <param name="str">Դ�ַ���</param>
        /// <param name="speater">�ָ���</param>
        /// <param name="toLower">�Ƿ�ת��ΪСд</param>
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
        /// �� List<string> ���շָ�����װ�� string
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
