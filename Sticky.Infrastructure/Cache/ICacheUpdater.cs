using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sticky.Infrastructure.Cache
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">is the cache model</typeparam>
    public interface ICacheUpdater<T>
    {
        Task UpdateBulkAsync(Dictionary<string, T> cacheList, bool deleteOld = true);
        Task UpdateOneAsync(string key, T cacheItem);
    }
}
