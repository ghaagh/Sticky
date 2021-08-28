using Sticky.Domain.ResponseAggrigate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sticky.Application.Advertising.Services
{
    public interface IMembershipFinder
    {
        Task<IEnumerable<Membership>> GetByPartnerUserIdAsync(string partnerHash, string partnerUserId);
        Task<IEnumerable<Membership>> GetByStickyUserIdAsync(long stickyId);
    }
}
