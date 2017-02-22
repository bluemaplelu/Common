using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Text.RegularExpressions;
using System.IO;
using System.Web;

namespace QHW.Common
{
    public class FileHelper
    {

        #region 获取模版
        /// <summary>
        /// 获取模版
        /// </summary>
        /// <param name="path">文件的路径</param>
        /// <param name="encoding">文件所使用的编码(ex:gbk|utf-8)</param>
        /// <returns></returns>
        public static string GetContentByFile(string path, string encoding)
        {
            if (File.Exists(path))
            {
                FileStream fs = File.Open(path, FileMode.Open);
                StreamReader sr = new StreamReader(fs, Encoding.GetEncoding(encoding));
                string result = sr.ReadToEnd();
                sr.Close();
                fs.Close();
                return result;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 获取模版
        /// </summary>
        /// <param name="path">文件的路径</param>
        /// <returns></returns>
        public static string GetContentByFile(string path)
        {
            if (File.Exists(path))
            {
                FileStream fs = File.Open(path, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                string result = sr.ReadToEnd();
                sr.Close();
                fs.Close();
                return result;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 获取模版
        /// </summary>
        /// <param name="path">文件的路径</param>
        /// <returns></returns>
        public static string[] GetContentLinesByFile(string path)
        {
            if (File.Exists(path))
            {
                return File.ReadAllLines(path);
            }
            else
            {
                return new string[] { "" };
            }
        }

        #endregion
        #region 生成文件
        /// <summary>
        /// 根据模板文件生成新的文件
        /// </summary>
        /// <param name="templatePath">模板文件路径</param>
        /// <param name="savePath">生成的文件路径</param>
        /// <param name="loopData">要循环列表的DataTable</param>
        /// <param name="otherData">要替换字符串的DataTable组合,一般为1行</param>
        public static void FileSaveByTemplateFile(string templatePath, string savePath, DataTable loopData, DataTable otherData, string coding)
        {
            string fileString = GetContentByFile(templatePath);
            FileSave(fileString, savePath, loopData, otherData, coding);
        }
        /// <summary>
        /// 根据模板文件生成新的文件
        /// </summary>
        /// <param name="templatePath">模板文件路径</param>
        /// <param name="savePath">生成的文件路径</param>
        /// <param name="loopData">要循环列表的DataTable</param>
        /// <param name="otherData">要替换字符串的DataTable组合,一般为1行</param>
        /// <param name="coding">编码格式</param>
        public static string FileSave(string templateString, string savePath, DataTable loopData, DataTable otherData, string coding)
        {
            if (otherData != null && otherData.Rows.Count > 0)
            {
                foreach (DataColumn otherKey in otherData.Columns)
                {
                    string key = otherKey.ColumnName;
                    templateString = templateString.Replace(string.Format("${0}$", key), otherData.Rows[0][key].ToString());
                }
            }
            RegexOptions myOption = RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline;
            Regex reg = new Regex("#loop(.+)#loop", myOption);
            string item = reg.Match(templateString).Groups[1].Value;
            if (!string.IsNullOrEmpty(item) && loopData != null && loopData.Rows.Count > 0)
            {
                StringBuilder sbBody = new StringBuilder();
                foreach (DataRow dr in loopData.Rows)
                {
                    string itemString = item;
                    foreach (DataColumn column in loopData.Columns)
                    {
                        Regex regColumn = new Regex(string.Format(@"\${0}\$", column.ColumnName), myOption);
                        itemString = regColumn.Replace(itemString, dr[column.ColumnName].ToString());
                    }
                    sbBody.Append(itemString);
                }
                templateString = reg.Replace(templateString, sbBody.ToString()).Replace("#loop", "");
            }

            CreateHTML(savePath, templateString, coding);
            return templateString;
        }
        #endregion
        #region 创建HTML文件
        /// <summary>
        /// 创建HTML文件
        /// </summary>
        /// <param name="path">文件所在路径</param>
        /// <param name="content">文件中的内容</param>
        /// <param name="coding">文件所使用的编码(ex:gbk|utf-8)</param>
        /// <returns></returns>
        public static void CreateHTML(string path, string content, string coding)
        {
            if (!File.Exists(path))
            {
                FileInfo fi = new FileInfo(path);
                if (!Directory.Exists(fi.DirectoryName))
                {
                    Directory.CreateDirectory(fi.DirectoryName);
                }
                FileStream rs = File.Create(path);
                System.IO.StreamWriter sw = new System.IO.StreamWriter(rs, System.Text.Encoding.GetEncoding(coding));
                sw.Write(content);
                sw.Close();
            }
            else
            {
                FileStream rs = File.Open(path, FileMode.Create);
                System.IO.StreamWriter sw = new System.IO.StreamWriter(rs, System.Text.Encoding.GetEncoding(coding));
                sw.Write(content);
                sw.Close();
            }
        }
        /// <summary>
        /// 创建HTML文件
        /// </summary>
        /// <param name="paths">文件所在路径(多路径的)</param>
        /// <param name="content">文件中的内容</param>
        /// <param name="coding">文件所使用的编码(ex:gbk|utf-8)</param>
        /// <returns></returns>
        public static void CreateHTML(string multiRoot, string subPath, string content, string coding)
        {
            string[] paths = BuildMultiPath(multiRoot, subPath);
            foreach (string path in paths)
            {
                if (!File.Exists(path))
                {
                    FileInfo fi = new FileInfo(path);
                    if (!Directory.Exists(fi.DirectoryName))
                    {
                        Directory.CreateDirectory(fi.DirectoryName);
                    }
                    FileStream rs = File.Create(path);
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(rs, System.Text.Encoding.GetEncoding(coding));
                    sw.Write(content);
                    sw.Close();
                }
                else
                {
                    FileStream rs = File.Open(path, FileMode.Create);
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(rs, System.Text.Encoding.GetEncoding(coding));
                    sw.Write(content);
                    sw.Close();
                }
            }
        }

        /// <summary>
        /// 构建多个路径
        /// </summary>
        /// <param name="multiRoot">多个根目录以,号分隔</param>
        /// <param name="subPath">子路径</param>
        /// <returns></returns>
        static string[] BuildMultiPath(string multiRoot, string subPath)
        {
            string[] multiPath = multiRoot.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < multiPath.Length; i++)
            {
                multiPath[i] = new StringBuilder().Append(multiPath[i]).Append(subPath).ToString();
            }
            return multiPath;
        }
        #endregion
        public static HtmlString GetINCContent(string virtualPath, bool isCached)
        {
            string filePath = HttpContext.Current.Server.MapPath(virtualPath);
            string key = filePath;
            if (isCached)
            {
                HtmlString content = Common.DotNetCacheManager.Get(key) as HtmlString;
                if (content == null)
                {
                    if (File.Exists(filePath))
                    {
                        content = new HtmlString(File.ReadAllText(filePath, Encoding.UTF8));
                        DotNetCacheManager.Add(key, content, 15);
                        return content;
                    }

                }

            }
            if (File.Exists(filePath))
            {
                string content = File.ReadAllText(filePath, Encoding.UTF8);
                return new HtmlString(content);
            }
            return null;
        }
        public static HtmlString GetINCContent(string virtualPath)
        {
            return GetINCContent(virtualPath, true);
        }
    }
}
