using Microsoft.AspNetCore.Mvc;
using Sticky.Application.Advertising.Services;
using System.Threading.Tasks;

namespace Sticky.Application.Advertising.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipController : ControllerBase
    {
        private IMembershipFinder _membershipFinder;
        public MembershipController(IMembershipFinder membershipFinder)
        {
            _membershipFinder = membershipFinder ;
        }

        [HttpGet("by-sticky-id/{stickyId}")]
        public async Task<IActionResult> GET([FromRoute]long stickyId) 
            => Ok(await _membershipFinder.GetByStickyUserIdAsync(stickyId));


        [HttpGet("by-partner-id/{partnerHash}/{partnerUserId}/")]
        public IActionResult GetForPartner([FromRoute] string partnerHash, string partnerUserId) 
            => Ok(_membershipFinder.GetByPartnerUserIdAsync(partnerHash, partnerUserId));
    }
}
