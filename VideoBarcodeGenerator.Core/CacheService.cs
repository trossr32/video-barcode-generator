using System;
using System.Runtime.Caching;

namespace VideoBarcodeGenerator.Core
{
    public class InMemoryCache : ICacheService
    {
        public T GetOrSet<T>(string cacheKey, Func<T> getItemCallback) where T : class
        {
            if (MemoryCache.Default.Get(cacheKey) is T item)
                return item;

            item = getItemCallback();

            MemoryCache.Default.Add(cacheKey, item, DateTime.Now.AddDays(1));

            return item;
        }
    }

    interface ICacheService
    {
        T GetOrSet<T>(string cacheKey, Func<T> getItemCallback) where T : class;
    }
}