using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace QHW.Common
{
    public class ConstConfig
    {
        public const string AppType_Web = "web";
        public const string AppType_Service = "service";
    }

    public class PubConstant
    {
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                string _connectionString = "";
                if (AppType == ConstConfig.AppType_Web)
                {
                    string theme = ConfigurationManager.AppSettings["Theme"];
                    _connectionString = ConfigurationManager.ConnectionStrings[theme + "ConnectionString"].ConnectionString;
                }
                else if (AppType == ConstConfig.AppType_Service)
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + "setting.ini";


                    INIFile settingfile = new INIFile(path);
                    _connectionString = settingfile.IniReadValue("Global", "connection");
                }
                return _connectionString;
            }
        }


        private static string _appType = ConstConfig.AppType_Web;
        public static string AppType
        {
            get { return _appType; }
            set { _appType = value; }
        }

        /// <summary>
        /// 得到web.config里配置项的数据库连接字符串。
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public static string GetConnectionString(string configName)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[configName].ConnectionString;

            return connectionString;
        }


    }
}
