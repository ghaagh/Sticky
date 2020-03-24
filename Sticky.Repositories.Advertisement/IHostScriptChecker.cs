
using System.Threading.Tasks;

namespace Sticky.Repositories.Advertisement
{
    public interface IHostScriptChecker
    {

        Task UpdatePageValidation(int hostId);
        Task UpdateProductValidation(int hostId);
        Task UpdateCartValidation(int hostId);
        Task UpdateBuyValidation(int hostId);
        Task UpdateFavValidation(int hostId);

    }
}
