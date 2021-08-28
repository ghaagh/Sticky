using Microsoft.AspNetCore.Mvc;
using Sticky.Domain.SegmentAggrigate;
using System;
using System.Linq;

namespace Sticky.Application.Dashboard.Controllers.ActivityTypes
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityTypesController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetList()
        {
            return Ok(Enum.GetValues(typeof(ActivityTypeEnum)).Cast<ActivityTypeEnum>());
        }
    }
}
