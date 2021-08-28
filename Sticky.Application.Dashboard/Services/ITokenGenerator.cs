using System.Threading.Tasks;

namespace Sticky.Application.Dashboard.Services
{
    public interface ITokenGenerator
    {
        Task<string> GenerateAsync(string email, string password);
    }
}
