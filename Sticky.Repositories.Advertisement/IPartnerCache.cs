using Sticky.Models.Context;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sticky.Repositories.Advertisement
{
    /// <summary>
    /// A Cache To Find a Partner in Redis or Return a list of active partners.
    /// </summary>
    public interface IPartnerCache
    {
        /// <summary>
        /// returns Partner info from partnerhashcode.if returned partner id is 0 , it means that partner is not valid,not activated(on sql server Partners table) or it's not in the cache. (Cache update is one time per hour job. just wait or restart the cache console manually)
        /// </summary>
        /// <param name="hashCode">
        /// a hashcode given to partners to include in their headers when sending requests.
        /// </param>
        /// <returns>
        /// returns partner data. 
        /// </returns>
        Task<Partner> FindPartner(string hashCode);
        /// <summary>
        /// returns list of active partners.
        /// </summary>
        /// <returns></returns>
        Task<List<Partner>> ListAsync();
    }
}
