using System.Threading.Tasks;

namespace Sticky.Domain.ClientUsers
{
    public interface IClientUserRepository
    {
        Task<long> CreateAsync();
    }
}
