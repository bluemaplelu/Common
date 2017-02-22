using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;


namespace DotNet.Utilities
{
    public class TypeParse
    {

        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="objectValue"></param>
        /// <returns></returns>
        public static bool IsNumeric(object objectValue)
        {
            if (objectValue != null)
            {
                string str = objectValue.ToString();
                if (str.Length > 0 && str.Length <= 11 && Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]*$"))
                {
                    if ((str.Length <= 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
                    {
                        return true;
                    }
                }
            }
            return false;

        }

        /// <summary>
        /// 判断是否为Double类型数据
        /// </summary>
        /// <param name="objectValue"></param>
        /// <returns></returns>
        public static bool IsDouble(object objectValue)
        {
            if (objectValue != null)
            {
                return Regex.IsMatch(objectValue.ToString(), @"^([0-9])[0-9]*(\.\w*)?$");
            }
            return false;
        }

        /// <summary>
        /// string型("true,false")转换为bool型
        /// </summary>
        /// <param name="stringValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool stringToBool(string stringValue, bool defValue)
        {
            if (stringValue != null)
            {
                if (string.Compare(stringValue, "true", true) == 0)
                {
                    return true;
                }
                else if (string.Compare(stringValue, "false", true) == 0)
                {
                    return false;
                }
            }
            return defValue;
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="objectValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int ObjectToInt(object objectValue, int defValue)
        {
            int result;
            if (!int.TryParse((objectValue ?? "").ToString(), out result))
            {
                return defValue;
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// 转换为float型
        /// </summary>
        /// <param name="objectValue">要转换的Object</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float ObjectToFloat(object objectValue, float defValue)
        {
            if ((objectValue == null) || (objectValue.ToString().Length > 10))
            {
                return defValue;
            }

            float intValue = defValue;
            if (objectValue != null)
            {
                bool IsFloat = Regex.IsMatch(objectValue.ToString(), @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (IsFloat)
                {
                    intValue = Convert.ToSingle(objectValue);
                }
            }
            return intValue;
        }

        /// <summary>
        /// 将对象转换为decimal类型
        /// </summary>
        /// <param name="objectValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的decimal类型结果</returns>
        public static decimal ObjectToDecimal(object objectValue, decimal defValue)
        {
            decimal result;
            if (!decimal.TryParse((objectValue ?? "").ToString(), out result))
            {
                return defValue;
            }
            else
            {
                return result;
            }
        }



        /// <summary>
        /// 判断给定的字符串数组(strNumber)中的数据是不是都为数值型
        /// </summary>
        /// <param name="strNumber">要确认的字符串数组</param>
        /// <returns>是则返加true 不是则返回 false</returns>
        public static bool IsNumericArray(string[] strNumber)
        {
            if (strNumber == null)
            {
                return false;
            }
            if (strNumber.Length < 1)
            {
                return false;
            }
            foreach (string id in strNumber)
            {
                if (!IsNumeric(id))
                {
                    return false;
                }
            }
            return true;

        }

        /// <summary>
        /// 将对象转换为DateTime类型
        /// </summary>
        /// <param name="objectValue"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime ObjectToDateTime(object objectValue, DateTime defaultValue)
        {
            DateTime time = DateTime.Now;
            DateTime dt = defaultValue;
            if (objectValue != null)
            {
                DateTime.TryParse(string.IsNullOrEmpty(objectValue.ToString()) ? DateTime.Now.ToString() : objectValue.ToString(), out time);
                int year = time.Year;

                if (year > 1)
                {
                    dt = time;
                }
            }
            return dt;
        }
    }
}
