using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Sticky.Application.Script.Controllers
{
    [Produces("application/json")]
    [Route(".well-known")]
    public class FreeSSLController : Controller
    {
        private readonly IWebHostEnvironment _env;
        public FreeSSLController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet]
        [Route("acme-challenge/{id}")]
        public ActionResult LetsEncrypt(string id)
        {
            var file = Path.Combine(_env.ContentRootPath, ".well-known", "acme-challenge", id);
            return PhysicalFile(file, "text/plain");
        }
    }
}