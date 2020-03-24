
using Sticky.Dto.Advertisement.Response;
using Sticky.Models.Redis;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sticky.Repositories.Advertisement
{
    /// <summary>
    /// Utility for making iframe or product list from user current segments.
    /// </summary>
    public interface IResponseGenerator
    {
        /// <summary>
        /// Creates a full response base on user segment membership and request types.
        /// </summary>
        /// <param name="userId">
        /// Sticky UserId
        /// </param>
        /// <param name="partnerId">
        /// Partner Unique Id
        /// </param>
        /// <param name="requestType">
        /// RequestType.Native or RequestType.Banner for native or banner types.
        /// </param>
        /// <param name="segments">
        /// list of user Segments
        /// </param>
        /// <returns>
        /// returns a MembershipRespose with Status=true and segment products or iframe linke(base on request type. if native=>returns product list , if banner returns iframe address)
        /// </returns>
        Task<MembershipResponse> CreateResponseAsync(long userId,int partnerId, List<UserSegment> segments);
        /// <summary>
        /// returns an empty response base on status and message given to it.
        /// </summary>
        /// <param name="status">
        /// ResponseStatus.Error or ResponseStatus.Success. returns "Error" or "True"
        /// </param>
        /// <param name="message">
        /// Descriptions and reasons for status.
        /// </param>
        /// <returns></returns>
        MembershipResponse EmptyResponse(string status, string message);
    }
}
