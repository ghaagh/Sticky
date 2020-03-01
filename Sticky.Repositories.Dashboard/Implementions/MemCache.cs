using Microsoft.Extensions.Caching.Memory;
using System;


namespace Sticky.Repositories.Dashboard.Implementions
{
    public class MemCache : IMemCache
    {


        private readonly IMemoryCache _cache;
        private readonly MemoryCacheEntryOptions _cacheOption;

        /// <summary>
        /// MemCahce Constructor
        /// </summary>
        /// <param name="expireAfter">Entry will expire after this parameter .(in secconds)</param>
        public MemCache(int expireAfter)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions();
            cacheEntryOptions.SetPriority(CacheItemPriority.High);
            cacheEntryOptions.SetAbsoluteExpiration(TimeSpan.FromSeconds(expireAfter));
            _cacheOption = cacheEntryOptions;

            MemoryCacheOptions cacheOption = new MemoryCacheOptions();
            _cache = new MemoryCache(cacheOption);
        }

        /// <summary>
        /// Get Value from cache 
        /// </summary>
        /// <typeparam name="T">Type of value</typeparam>
        /// <param name="key">Key </param>
        /// <returns></returns>
        public T GetValue<T>(string key)
        {
#pragma warning disable IDE0059 // Unnecessary assignment of a value
            var data = new object();
#pragma warning restore IDE0059 // Unnecessary assignment of a value
            bool isExist = _cache.TryGetValue(key, out data);
            if (isExist)
                return (T)data;
            else
                return (T)data;
        }

        /// <summary>
        /// Store value in cache 
        /// </summary>
        /// <typeparam name="T">Type of value</typeparam>
        /// <param name="key">Key</param>
        /// <param name="value">Value you want to store</param>
        public bool StoreValue<T>(string key, T value)
        {
            try
            {
                _cache.Set(key, value, _cacheOption);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
