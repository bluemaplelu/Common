using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Configuration;

namespace QHW.Common
{
    /// <summary>
    /// 异常日志记录工具类（通过该类可以记录系统中出现的异常信息，方便跟踪排查异常）
    /// </summary>
    public class ErrorHelper
    {
        /// <summary>
        /// 记录异常信息
        /// </summary>
        /// <param name="msg">异常信息内容</param>
        public static void RecordError(string msg)
        {
            string root = ConfigurationManager.AppSettings["errorLogPath"];
            string sPath = string.IsNullOrEmpty(root) ? System.Reflection.Assembly.GetExecutingAssembly().Location : root;
            string sDirectory = Path.Combine(Path.GetDirectoryName(sPath), "log");
            if (!Directory.Exists(sDirectory))
            {
                Directory.CreateDirectory(sDirectory);
            }
            string logPath = Path.Combine(sDirectory, new StringBuilder("log").Append(System.DateTime.Today.ToString("yyyyMMdd")).Append(".log").ToString());
            RecordError(msg, logPath);
        }

        /// <summary>
        /// 记录异常信息
        /// </summary>
        /// <param name="msg">异常信息内容</param>
        /// <param name="sPath">异常日志存储路径（绝对物理路径）</param>
        public static void RecordError(string msg, string sPath)
        {
            Mutex m_Mutex = new Mutex();
            try
            {
                m_Mutex.WaitOne();
            }
            catch (Exception)
            {
                return;
            }
            try
            {
                string sTime = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
                string dir = Path.GetDirectoryName(sPath);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                System.IO.TextWriter oWrite = System.IO.File.AppendText(sPath);
                oWrite.WriteLine(sTime + ": " + msg);
                oWrite.Close();

            }
            catch (Exception e)
            {
                EventLog myLog = new EventLog();
                myLog.Source = "tools";
                myLog.WriteEntry("Err:" + msg + "\t" + e.ToString());
            }
            finally
            {
                m_Mutex.ReleaseMutex();
            }
        }



        public static void WindowsEventLogWrite(string source, string error)
        {


        }
    }
}
