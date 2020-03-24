
using Microsoft.AspNetCore.Mvc;


namespace Sticky.API.CookieSyncing.Controllers
{
    [Route("Home")]
    public class HomeController : Controller
    {
        [Route("index")]
        [HttpGet]
        public string Index()
        {
            return "Hello to you too ;). Also see the cookie syncing extension for more details";
        }


    }
}
