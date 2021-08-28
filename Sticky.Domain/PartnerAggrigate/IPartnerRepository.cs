using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sticky.Domain.PartnerAggrigate
{
    public interface IPartnerRepository
    {
        Task<Partner> CreatePartnerAsync(Partner partner);
        Task<Partner> GetPartnerAsync(long id);
        Task<IEnumerable<Partner>> GetAllAsync();
    }
}
