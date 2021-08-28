
using System.Threading.Tasks;

namespace Sticky.Domain.Shared
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
    }
}
