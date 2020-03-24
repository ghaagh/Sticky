using Microsoft.AspNetCore.Mvc;
using Sticky.Dto.Advertisement.Request;
using Sticky.Dto.Advertisement.Response;
using Sticky.Models.Etc;
using Sticky.Models.Redis;
using Sticky.Repositories.Advertisement;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Sticky.API.Advertisement.Controller
{
    [Produces("application/json")]
    [Route("UserMemberships")]
    public class UserMembershipsController : ControllerBase
    { 
        private readonly IUserMembershipFinder _userMembershipFinder;
        private readonly IResponseGenerator _responseGenerator;
        private readonly IPartnerCache _partnersManager;
        private readonly IResponseTimeLogger _responseTimeLogger;
        public UserMembershipsController(IUserMembershipFinder userMembershipFinder,IResponseTimeLogger responseTimeLogger, IPartnerCache partnersManager, IResponseGenerator responseGenerator)
        {
            _responseTimeLogger = responseTimeLogger;
            _responseGenerator = responseGenerator;
            _partnersManager = partnersManager;
            _userMembershipFinder = userMembershipFinder;
        }
        /// <summary>
        /// returns cached membership data for requested user base on userId(or partner userId) 
        /// </summary>
        /// <param name="request">
        /// something like this :
        /// {
	    ///"StickyUserId": "68747321",
	    ///"PartnerUserId": null,
	    ///"PartnerHashCode": "12fe9740-1145-4adc-906d-cf2592afa5c4",
        /// }
        /// </param>
        /// <returns>
        /// a response  empty or full with products 
        /// </returns>
        [HttpPost]
        public async Task<MembershipResponse> GetUserSegment([FromBody] UserMembershipsRequest request)
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                if (request == null)
                    return _responseGenerator.EmptyResponse(ResponseStatus.Error, ResponseMessage.InvalidData);
                if (request.PartnerHashCode == null || (request.PartnerUserId == null && request.StickyUserId == null))
                    return _responseGenerator.EmptyResponse(ResponseStatus.Error, ResponseMessage.RequiredUserIdAndPartnerHashCode);
                var partner = await _partnersManager.FindPartner(request.PartnerHashCode);
                if (partner.Id == 0)
                    return _responseGenerator.EmptyResponse(ResponseStatus.Error, ResponseMessage.NotRegisteredPartner);

                MembershipData userMembershipResult = new MembershipData();
                if (request.StickyUserId == null)
                {
                    userMembershipResult = await _userMembershipFinder.FindMembershipByPartnerUserIdAsync(partner.Id, request.PartnerUserId);
                }
                else if (request.PartnerUserId == null)
                {
                    userMembershipResult = await _userMembershipFinder.FindMembershipByStickyIdAsync(request.StickyUserId ?? 0);
                }
                var response =  await _responseGenerator.CreateResponseAsync(userMembershipResult.StickyUserId, partner.Id, userMembershipResult.Segments);
                stopwatch.Stop();
               await  _responseTimeLogger.LogResponseTime(stopwatch.ElapsedMilliseconds, 1);
                return response;
            }
            catch (Exception ex)
            {
                return _responseGenerator.EmptyResponse(ResponseStatus.Error, ex.Message);

            }


        }
    }
}