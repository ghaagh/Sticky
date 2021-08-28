using Sticky.Infrastructure.Cache.Models;
using System.Threading.Tasks;

namespace Sticky.Application.Advertising.Services
{
    public interface IAdTextGenerator
    {
        string Clean(string name);
        Task<(string finalName, long templateId)> CreateAdvertisingText(long segmentId, string name, int? price);
    }
}
