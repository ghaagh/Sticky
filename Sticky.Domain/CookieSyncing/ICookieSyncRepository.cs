using System.Threading.Tasks;

namespace Sticky.Domain.CookieSyncing
{
    public interface ICookieSyncRepository
    {
        Task SyncCookieAsync(CookieMatch cookieMatch);
        Task<long> GetStickyCookie(string partnerHash, string partnerUserId);
    }
}
