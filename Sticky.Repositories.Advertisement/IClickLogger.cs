
using Sticky.Models.Etc;
using Sticky.Repositories.Common;
using System.Threading.Tasks;

namespace Sticky.Repositories.Advertisement
{
    public interface IClickLogger
    {
        Task IncreaseClick(string fullloghash, string uniqueId);
        Task IncreaseImpression(string winNoticeString);
    }

}
