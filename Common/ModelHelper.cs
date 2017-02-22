using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace QHW.Common
{
    /// <summary>
    /// 数据库表-Model实体映射功能
    /// <remarks>注意！！！实体中的属性名称必须和表列名相同，必须相同，不同你就2逼了</remarks>
    /// </summary>
    public sealed class ModelHelper
    {
        private ModelHelper() { ;}

        private const string exception_msg0 = "Model实体类中不包含任何属性成员";
        private const string exception_msg1 = "参数为空";
        private const string exception_msg2 = "为空或列集合中列个数为0";

        /// <summary>
        /// 填充实体
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <param name="row">行</param>
        /// <param name="columns">列集</param>
        /// <returns></returns>
        public static T FillModel<T>(DataRow row, DataColumnCollection columns) where T : new()
        {
            if (row == null) throw new ArgumentNullException();
            if (columns == null || columns.Count == 0) throw new ArgumentNullException(exception_msg2);
            PropertyInfo[] propertys = (typeof(T)).GetProperties();
            int len = propertys.Length;
            if (len == 0) throw new Exception(exception_msg0);
            T obj = new T();
            for (int i = 0; i < len; i++)
            {
                string columnName = propertys[i].Name;
                if (columns.Contains(columnName))
                {
                    if (row[columnName] != DBNull.Value)
                    {
                        propertys[i].SetValue(obj
                            , Convert.ChangeType(row[columnName], propertys[i].PropertyType)
                            , null);
                    }
                }
            }
            return obj;
        }

        /// <summary>
        /// 填充实体集合
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <param name="dt">表集</param>
        /// <returns></returns>
        public static List<T> FillModelsFromDataTable<T>(DataTable dt) where T : new()
        {
            List<T> models = null;
            if (dt == null) { models = new List<T>(); models.Add(new T()); return models; }
            DataRowCollection rows = dt.Rows;
            DataColumnCollection dcc = dt.Columns;
            models = new List<T>(rows.Count);
            foreach (DataRow dr in rows)
            {
                models.Add(FillModel<T>(dr, dcc));
            }

            return models;
        }

        /// <summary>
        /// DataTable 2 List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static IList<T> Table2List<T>(DataTable table)
        {
            IList<T> list = new List<T>();
            T t = default(T);
            PropertyInfo[] propertypes = null;
            string tempName = string.Empty;
            foreach (DataRow row in table.Rows)
            {
                t = Activator.CreateInstance<T>();
                propertypes = t.GetType().GetProperties();
                foreach (PropertyInfo pro in propertypes)
                {
                    tempName = pro.Name;
                    if (table.Columns.Contains(tempName.ToUpper()))
                    {
                        if (!pro.CanWrite)
                            continue;
                        object value = row[tempName];
                        if (value != DBNull.Value)
                            pro.SetValue(t, value, null);
                    }
                }
                list.Add(t);
            }
            return list;
        }


        /// <summary>
        /// 防止数据绑定字段值为空的情况
        /// </summary>
        /// <param name="value">绑定值</param>
        /// <returns></returns>
        public static string DefendDBNull(object value, string insteadStr)
        {
            return (value == null || value == DBNull.Value)
                ? (string.IsNullOrEmpty(insteadStr) ? string.Empty : insteadStr)
                : value.ToString();
        }

        /// <summary>
        /// 根据表单/url 请求参数 自动填充实体类,
        /// 注意:参数名去掉前缀(prefix)必须和实体类的属性对应,例如 对象属性名字ID,如果调用方法传递的参数是"qhw_"那么传递的参数名必须是 qhw_ID
        /// 目前实体类的属性能自动转换[int,string,float,datetime,decimal]类型的值
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <returns></returns>
        public static T FillModelFromRequst<T>(string prefix) where T : new()
        {
            PropertyInfo[] propertys = (typeof(T)).GetProperties();
            int len = propertys.Length;
            if (len == 0) throw new Exception(exception_msg0);
            T obj = new T();
            for (int i = 0; i < len; i++)
            {
                string columnName = prefix + propertys[i].Name;
                string typeString = propertys[i].PropertyType.Name.ToLower();
                object value = null;
                switch (typeString)
                {
                    case "int32":
                        value = RequestHelper.GetInt(columnName, 0);
                        break;
                    case "string":
                        value = RequestHelper.GetString(columnName);
                        break;
                    case "float":
                        value = RequestHelper.GetFloat(columnName, 0);
                        break;
                    case "datetime":
                        DateTime dt = DateTime.MinValue;
                        DateTime.TryParse(RequestHelper.GetString(columnName), out dt);
                        value = dt;
                        break;
                    case "decimal":
                        decimal d = 0;
                        decimal.TryParse(RequestHelper.GetString(columnName), out d);
                        value = d;
                        break;
                    default:
                        break;
                }
                propertys[i].SetValue(obj
                    , Convert.ChangeType(value, propertys[i].PropertyType)
                    , null);

            }
            return obj;
        }

        /// <summary>
        /// 填充实体
        /// 为实体的每个属性付默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static T FillModel<T>(T t)
        {
            Type type = t.GetType();
            PropertyInfo[] proes = type.GetProperties();

            object value = null;
            foreach (var item in proes)
            {
                switch (item.PropertyType.Name.ToLower())
                {
                    case "int32":
                        value = TypeParse.ObjectToInt(item.GetValue(t, null), 0);
                        break;
                    case "string":
                        value = item.GetValue(t, null);
                        if (value == null)
                            value = string.Empty;
                        break;
                    case "float":
                        value = TypeParse.ObjectToFloat(item.GetValue(t, null), 0);
                        break;
                    case "datetime":
                        value = TypeParse.ObjectToDateTime(item.GetValue(t, null), DateTime.Now);
                        break;
                    case "decimal":
                        value = TypeParse.ObjectToDecimal(item.GetValue(t, null), 0);
                        break;
                    case "boolean":
                        value = item.GetValue(t, null);
                        if (value == null)
                        {
                            value = false;
                        }
                        break;
                    default:
                        break;
                }

                item.SetValue(t, value, null);
            }

            return t;
        }
    }
}
