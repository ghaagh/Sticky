using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sticky.Repositories.Advertisement
{
    /// <summary>
    /// a class to find crawler users based on products or page count.
    /// </summary>
   public interface ICrowlerCache
    {
        Task AddUserToCrowlers(long userId);
        Task<bool> IsCrowler(long userId);
    }
}
