using Sticky.Domain.UserAggrigate;
using System.Threading.Tasks;

namespace Sticky.Domain.HostAggrigate
{
    public interface IHostRepository
    {
        Task<Host> CreateHostAsync(string hostAddress, string useriD, ValidityEnum userExpiration = ValidityEnum.Y1, ValidityEnum productExpiration = ValidityEnum.Y1);
        Task<Host> GetByIdAsync(long id);
    }
}
