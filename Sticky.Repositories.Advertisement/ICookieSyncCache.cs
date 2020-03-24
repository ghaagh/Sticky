using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sticky.Repositories.Advertisement
{
    public interface ICookieSyncCache
    {
        Task<long?> FindStickyUserIdAsync(int partnerId,string userId);
    }
}
