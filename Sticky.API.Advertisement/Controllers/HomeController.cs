using Microsoft.AspNetCore.Mvc;

namespace Sticky.API.Advertisement.Controller
{
    [Route("Home")]
    public class HomeController : ControllerBase
    {
        [HttpGet("Index")]
        public string Index()
        {
            return "hello to you too ;)";
        }

    }
}
