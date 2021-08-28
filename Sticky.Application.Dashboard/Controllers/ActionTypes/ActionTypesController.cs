using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sticky.Domain.SegmentAggrigate;
using System;
using System.Linq;

namespace Sticky.Application.Dashboard.Controllers.ActionTypes
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ActionTypesController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetList()
        {
            return Ok(Enum.GetValues(typeof(ActionTypeEnum)).Cast<ActionTypeEnum>());
        }
    }
}
