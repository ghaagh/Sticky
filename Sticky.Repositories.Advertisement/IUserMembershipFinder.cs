
using Sticky.Models.Redis;
using System.Threading.Tasks;

namespace Sticky.Repositories.Advertisement
{
    /// <summary>
    /// a Redis Wrapper For Finding User Membership From User Id Or Partner UserId.
    /// </summary>
    public interface IUserMembershipFinder
    {
        /// <summary>
        /// returns Segment Membership from PartnerId.
        /// </summary>
        /// <param name="partnerId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<MembershipData> FindMembershipByPartnerUserIdAsync(int partnerId,string userId);
        /// <summary>
        /// returns UserMembership from StickyId
        /// </summary>
        /// <param name="stickyId">
        /// Id of User In This System
        /// </param>
        /// <returns></returns>
       Task< MembershipData> FindMembershipByStickyIdAsync(long stickyId);
    }
}
