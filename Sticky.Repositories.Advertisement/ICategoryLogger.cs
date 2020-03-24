
using System.Threading.Tasks;

namespace Sticky.Repositories.Advertisement
{
    /// <summary>
    /// Utility for logging new categories with visit counts into redis.
    /// </summary>
    public interface ICategoryLogger
    {
        /// <summary>
        /// logs the category into redis. it will be flushed to sql every hour.
        /// </summary>
        /// <param name="hostId"> Host Id</param>
        /// <param name="category">category strings</param>
        /// <param name="counter">visit counters</param>
        /// <returns></returns>
        Task LogCategory(int hostId, string category, double counter);
    }
}
