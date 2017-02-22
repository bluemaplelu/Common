using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace QHW.Common
{
    public class InMemoryCache
    {
        public static TValue Get<TValue>(string cacheKey) where TValue : class
        {
            TValue item = MemoryCache.Default.Get(cacheKey) as TValue;
            return item;
        }

        public static bool Add(string cacheKey, object cacheValue, int durationInMinutes = 5)
        {
            return MemoryCache.Default.Add(cacheKey, cacheValue, DateTime.Now.AddMinutes(durationInMinutes));
        }

        public static void Remove(string cacheKey)
        {
            MemoryCache.Default.Remove(cacheKey);
        }
    }
}