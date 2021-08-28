using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sticky.Infrastructure.Cache
{
    public interface IMultipleCache<T> where T : class
    {
        Task<T> GetAsync(string key);
        Task<IEnumerable<T>> GetListAsync();
    }
}
