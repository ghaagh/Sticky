using System.Threading.Tasks;

namespace Sticky.Domain.UserAggrigate
{
    public interface IIdentity
    {
        string GetUserId();
    }
}
