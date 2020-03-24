using System.Threading.Tasks;
using Sticky.Dto.Dashboard.Response;

namespace Sticky.Repositories.Dashboard
{
    public interface IHostDataExtractor
    {
        Task<HostDataResult> ExtractHostInfoAsync(int hostId);
    }
}
