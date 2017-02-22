using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using MongoDB.Driver;

namespace DotNet.Utilities
{
    public class MongoDBHelper
    {
        public static MongoCollection<T> GetMongoCollection<T>(string collectionName,  string connectionString)
        {
        
            MongoServer server = MongoServer.Create(connectionString);
            MongoDatabase db = server.GetDatabase(GetDataBaseName(connectionString));
            return db.GetCollection<T>(collectionName);
        }

        public static MongoCollection GetMongoCollection(string collectionName,  string connectionString)
        {
             

            MongoServer server = MongoServer.Create(connectionString);
            MongoDatabase db = server.GetDatabase(GetDataBaseName(connectionString));
            return db.GetCollection(collectionName);
        }

        /// <summary>
        /// 获取Mongodb数据库名称
        /// </summary>
        /// <param name="connectionString">mongodb://192.168.2.252/QHWMonitor</param>
        /// <returns></returns>
        public static string GetDataBaseName(string connectionString)
        {
            string[] array = connectionString.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            return array[array.Length - 1];

        }
    }
}
                              