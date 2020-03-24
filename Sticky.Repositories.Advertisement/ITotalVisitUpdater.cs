
using System.Threading.Tasks;

namespace Sticky.Repositories.Advertisement
{
    public interface ITotalVisitUpdater
    {
        Task UpdateTotalVisit(int hostId);
    }
}
