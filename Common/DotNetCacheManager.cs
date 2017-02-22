using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;


namespace DotNet.Utilities
{
    public class DotNetCacheManager
    {
        static System.Web.Caching.Cache cache = HttpContext.Current == null ? System.Web.HttpRuntime.Cache : HttpContext.Current.Cache;

        /// <summary>
        /// 添加缓存项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expireMin">过期时长（分钟）</param>
        /// <returns></returns>
        public static bool Add(string key, object value, double expireMin)
        {
            return cache.Add(key, value, null, DateTime.Now.AddMinutes(expireMin), TimeSpan.Zero, CacheItemPriority.Normal, null) != null;
        }

        /// <summary>
        /// 添加缓存项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeSpan">过期时长</param>
        /// <returns></returns>
        public static bool Add(string key, object value, TimeSpan timeSpan)
        {
            return cache.Add(key, value, null, DateTime.MaxValue, timeSpan, CacheItemPriority.Normal, null) != null;
        }


        /// <summary>
        /// 获取缓存项值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object Get(string key)
        {
            return cache[key];
        }

        /// <summary>
        /// 获取缓存项值
        /// </summary>
        /// <typeparam name="T">缓存项类型</typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            if (cache[key] == null) return default(T);
            return (T)cache[key];
        }

        /// <summary>
        /// 获取多个缓存项
        /// </summary>
        /// <param name="keys">缓存键列表</param>
        /// <returns></returns>
        public static IDictionary<string, object> Get(params string[] keys)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            foreach (string key in keys)
            {
                dict.Add(key, cache[key]);
            }
            return dict;
        }

        /// <summary>
        /// 删除缓存项
        /// </summary>
        /// <param name="key"></param>
        /// <returns>缓存项值</returns>
        public static object Remove(string key)
        {
            return cache.Remove(key);
        }

        public static void Insert(string key, object value)
        {
            cache.Insert(key, value);
        }
    }
}
