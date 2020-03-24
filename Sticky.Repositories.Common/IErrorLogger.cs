using System.Threading.Tasks;

namespace Sticky.Repositories.Common
{
   public interface IErrorLogger
    {
        Task LogError(string error);
    }
}
