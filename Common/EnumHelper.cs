using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using System.ComponentModel;
using System.Web.UI.WebControls;

namespace DotNet.Utilities
{
    public class EnumHelper
    {
        /// <summary>
        /// 得到枚举的描述
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDescription(Enum value)
        {
            Type type = value.GetType();
            MemberInfo[] memInfo = type.GetMember(value.ToString());
            string returnStr = string.Empty;
            if (memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs.Length > 0)
                {
                    returnStr = ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            return returnStr;
        }

        /// <summary>
        /// 得到包含值和名称的枚举集合
        /// </summary>
        /// <param name="type">枚举类型</param>
        /// <returns></returns>
        public static IList GetEnumTextList(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (!type.IsEnum) throw new ArgumentException("Type provided must be an Enum.", "type");

            ArrayList list = new ArrayList();
            Array array = Enum.GetValues(type);
            foreach (Enum value in array)
            {
                list.Add(new KeyValuePair<int, string>(Convert.ToInt32(value), value.ToString()));
            }
            return list;
        }
        /// <summary>
        /// 得到包含值和名称的枚举集合
        /// </summary>
        /// <param name="type">枚举类型</param>
        /// <returns></returns>
        public static Dictionary<int, string> GetEnumTextDic(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (!type.IsEnum) throw new ArgumentException("Type provided must be an Enum.", "type");

            Array array = Enum.GetValues(type);
            Dictionary<int, string> list = new Dictionary<int, string>();
            foreach (Enum value in array)
            {
                list.Add(Convert.ToInt32(value), value.ToString());
            }
            return list;
        }
        /// <summary>
        /// 得到枚举描述集合
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IList GetEnumDescriptionList(Type type)
        {
            if (type == null || !type.IsEnum) return null;
            ArrayList list = new ArrayList();
            Array array = Enum.GetValues(type);
            foreach (Enum value in array)
            {
                list.Add(new KeyValuePair<int, string>(Convert.ToInt32(value), GetEnumDescription(value)));
            }
            return list;
        }
        /// <summary>
        /// 得到枚举元素总个数
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int GetEnumElementCount(Type type)
        {
            if (type == null || !type.IsEnum) return 0;
            return Enum.GetValues(type).Length;
        }
        public static Dictionary<int, string> GetEnumDescriptionDicList(Type type)
        {
            if (type == null || !type.IsEnum) return null;
            Dictionary<int, string> list = new Dictionary<int, string>();
            Array array = Enum.GetValues(type);
            foreach (Enum value in array)
            {
                list.Add(Convert.ToInt32(value), GetEnumDescription(value));
            }
            return list;
        }

        /// <summary>
        /// 根据枚举的内容编号获取对应的描述信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescriptionByValue(Type type, int value)
        {
            Dictionary<int, string> list = GetEnumDescriptionDicList(type);
            string des = string.Empty;
            foreach (KeyValuePair<int, string> item in list)
            {
                if (item.Key == value)
                {
                    des = item.Value;
                }
            }
            return des;
        }
    }
}
