using Sticky.Dto.Dashboard.Request;
using Sticky.Dto.Dashboard.Response;
using Sticky.Models.Context;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Sticky.Repositories.Dashboard
{
    public interface IHostManager
    {
        Task<HostResult> CreateAsync(CreateHostRequest host);
        Task<bool> ModifyHostAsync(int id,ModifyHostRequest validity);
        Task<IEnumerable<Host>> GetUserHostsAsync(string email,int? id);
        Task<IEnumerable<Host>> GetAllHostsAsync(string email);
        Task<bool> GrantAccessToHostAsync(string currentEmail, string accessEmail, int hostId);

    }
}
