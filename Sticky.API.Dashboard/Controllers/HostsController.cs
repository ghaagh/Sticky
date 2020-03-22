using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Sticky.Models.Etc;
using Sticky.Repositories.Dashboard;
using Sticky.Dto.Dashboard.Response;
using Sticky.Dto.Dashboard.Request;
using Microsoft.AspNetCore.Identity;
using Sticky.Models.Context;

namespace DashboardAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Hosts")]
    [Authorize(Roles = Roles.HostOwnerOrAdmin)]
    public class HostsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IHostManager _hostManager;
        public HostsController(IHostManager hostManager,UserManager<User> userManager)
        {
            _userManager = userManager;
            _hostManager = hostManager;
        }
        [HttpGet]
        public async Task<HostReponse> List([FromQuery]int? id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
                return new HostReponse() { Valid=false,Error="No User Access"};
            try
            {
                HostReponse response = new HostReponse
                {
                    Valid = true,
                    Result = (await _hostManager.GetUserHostsAsync(user.Email, id)).Select(c => new HostResult()
                    {
                        Owner = c.User.Email,
                        FinalizePage = c.FinalizePage,
                        AdToCartId = c.AddToCardId,
                        LogoOtherData = c.LogoOtherData,
                        LogoAddress = c.LogoAddress,
                        HostAddress = c.HostAddress,
                        Id = c.Id,
                        ProductValidityType = c.ProductValidityId,
                        UserValidityType = c.UserValidityId,
                        SegmentCreationAccess = new SegmentCreationAccess() { 
                        Page = c.HostValidated && c.PageValidated,
                        AddToCart = c.AddToCardValidated,
                        Fav = c.FavValidated==true,
                        Buy = c.FinalizeValidated,
                        ProductVisit = c.CategoryValidated
                        }
                    })
                };
                return response;
            }
            catch (Exception ex)
            {
                return new HostReponse() { Error = ex.Message, Valid = false };
            }

        }
        [HttpGet("All")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<HostReponse> ListAll([FromQuery]string email)
        {
            try
            {
                HostReponse response = new HostReponse
                {
                    Valid = true,
                    Result = (await _hostManager.GetAllHostsAsync(email)).Select(c => new HostResult()
                    {
                        Owner = c.User.Email,
                        FinalizePage = c.FinalizePage,
                        AdToCartId = c.AddToCardId,
                        LogoOtherData = c.LogoOtherData,
                        LogoAddress = c.LogoAddress,
                        HostAddress = c.HostAddress,
                        Id = c.Id,
                        ProductValidityType = c.ProductValidityId,
                        UserValidityType = c.UserValidityId,
                                            SegmentCreationAccess = new SegmentCreationAccess()
                                            {
                                                Page = c.HostValidated && c.PageValidated,
                                                AddToCart = c.AddToCardValidated,
                                                Buy = c.FinalizeValidated,
                                                ProductVisit = c.CategoryValidated
                                            }
                    })
                };
                return response;
            }
            catch (Exception ex)
            {
                return new HostReponse() { Error = ex.Message, Valid = false };
            }

        }
        [HttpPost]
        public async Task<HostReponse> Create([FromBody]CreateHostRequest model)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
                return new HostReponse() { Valid = false, Error = "No User Access" };
            var createdHost = (await _hostManager.CreateAsync(user.Email,model));
            return new HostReponse()
            {
                Error = "",
                Valid = !createdHost.AlreadyExists,
                Result = new List<HostResult>() { createdHost }
            };
        }
        [HttpPatch("{id}")]
        public async Task<EmptyResponse> Edit(int id, [FromBody]ModifyHostRequest request)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
                return new EmptyResponse() { Valid = false, Error = "No Access" };
               var isOk = await _hostManager.ModifyHostAsync(id,user.Id, request);
            return new EmptyResponse { Valid = isOk };
        }
    }
}