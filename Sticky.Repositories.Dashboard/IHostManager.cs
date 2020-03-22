using Sticky.Dto.Dashboard.Request;
using Sticky.Dto.Dashboard.Response;
using Sticky.Models.Context;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Sticky.Repositories.Dashboard
{
    public interface IHostManager
    {
        Task<HostResult> CreateAsync(string userId,CreateHostRequest host);
        Task<bool> UserHasAccessToHost(string userId, int host);
        Task<bool> ModifyHostAsync(int id, string userId,ModifyHostRequest validity);
        Task<IEnumerable<Host>> GetUserHostsAsync(string userId,int? id);
        Task<IEnumerable<Host>> GetAllHostsAsync(string userId);
        Task<bool> GrantAccessToHostAsync(string userId, string destinationUserId, int hostId);

    }
}
