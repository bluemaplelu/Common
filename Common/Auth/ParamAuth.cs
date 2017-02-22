using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet.Utilities.Auth
{
    public class ParamAuth
    {
        /// <summary>
        /// 验证字符型日期
        /// </summary>
        /// <param name="strDateTime">字符串日期</param>
        /// <returns></returns>
        public static bool IsDateTime(string strDateTime)
        {
            DateTime dt = DateTime.Now;
            bool result = DateTime.TryParse(strDateTime, out dt);
            return result;
        }

        /// <summary>
        /// 是否为数字
        /// </summary>
        /// <param name="numberString"></param>
        /// <returns></returns>
        public static bool IsNumber(string numberString)
        {
            if (!string.IsNullOrEmpty(numberString))
            {
                double i = 1;
                bool result = double.TryParse(numberString, out i);
                return result;
            }
            else
            {
                return false;
            }
        }
    }
}
