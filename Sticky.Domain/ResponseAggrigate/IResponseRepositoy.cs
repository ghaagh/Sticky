using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sticky.Domain.ResponseAggrigate
{
    public interface IResponseRepositoy
    {
        Task<bool> ExistAsync(ResponseUpdaterTypeEnum responseType, long stickyId, string excludedId="");
        Task<IEnumerable<Membership>> GetMembership(ResponseUpdaterTypeEnum responseType, long stickyId);
        Task SetMembership(ResponseUpdaterTypeEnum responseType, long stickyId, List<Membership> memberships, int emptyResponseExpireInMunites, int fullResponseExpireInMinute, string excludedId="");
    }
}
