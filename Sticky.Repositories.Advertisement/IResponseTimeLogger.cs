
using System.Threading.Tasks;

namespace Sticky.Repositories.Advertisement
{
    public interface IResponseTimeLogger
    {

        Task LogResponseTime(long timeperiod, double counter);
    }
}
