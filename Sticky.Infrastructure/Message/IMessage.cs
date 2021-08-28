using Sticky.Domain.SegmentAggrigate;
using System.Threading.Tasks;

namespace Sticky.Infrastructure.Message
{
    public interface IMessage
    {
        Task SendPageViewMessageAsync(string host, long user, string address);
        Task SendProductMessageAsync(string host, long user, string productId, string productname, string imageAddress, string category, string url, int price);
        Task SendActionMessageAsync(string host, long user, string productId, ActivityTypeEnum statType);
    }

}

