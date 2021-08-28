using System.Threading.Tasks;

namespace Sticky.Infrastructure.Cache
{
    public interface ICache<T> where T : class
    {
        Task<T> GetAsync(string key);
    }

}
