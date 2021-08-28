using System.Threading.Tasks;

namespace Sticky.Domain.UserAggrigate
{
    public interface IUserRepository
    {
        Task<bool> RegisterAsync(string email, string password);
        Task<IIdentity> FindByIdAsync(string userId);
        Task<IIdentity> FindByEmailAsync(string email);

    }
}
