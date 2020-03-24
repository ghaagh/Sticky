using System.Threading.Tasks;

namespace Sticky.Repositories.Advertisement
{
    public interface IUserIdSetter
    {
        Task<long> GetNewUserIdAsync();
    }
}
