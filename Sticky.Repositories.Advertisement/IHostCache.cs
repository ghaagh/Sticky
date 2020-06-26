using Sticky.Models.Context;
using Sticky.Models.Redis;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sticky.Repositories.Advertisement
{
    /// <summary>
    /// a redis wrapper for finding host basic data, initial setups for hosts , get list of all or active hosts and a host property like html tags or expiration dates.
    /// </summary>
    public interface IHostCache
    {
        /// <summary>
        /// Returns list or all hosts in Redis cache. ( it will be updated 1 time per hour.)
        /// </summary>
        /// <returns>
        /// list of host with their basic info.
        /// </returns>
        Task<List<Host>> GetListOfHostAsync();
        /// <summary>
        /// returns if given host is a valid host or not
        /// </summary>
        /// <param name="host">address of the host you want to search in cache</param>
        /// <returns>true for valid and false for invalid</returns>
        bool HostExists(string host);
        /// <summary>
        /// return host basic info based on host address. if returns null means the host is not valid or active
        /// </summary>
        /// <param name="host">address of the host you want to search.</param>
        /// <returns>
        /// host basic data.
        /// </returns>
        Task<HostWithoutRelation> GetHostAsync(string host);
        /// <summary>
        /// returns host basic infos based on Host Id.
        /// </summary>
        /// <param name="hostId"></param>
        /// <returns></returns>
        Task<HostWithoutRelation> GetHostByIdAsync(int hostId);


    }
}
