using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Sticky.API.Advertisement.Controller
{
    public class FreeSSLController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        public FreeSSLController(IWebHostEnvironment env)
        {
            _env = env;
        }
        public string Index()
        {
            return "hello to you too ;)";
        }
        [HttpGet]
        [Route(".well-known/acme-challenge/{id}")]
        public ActionResult LetsEncrypt(string id)
        {
            var file = Path.Combine(_env.ContentRootPath, ".well-known", "acme-challenge", id);
            return PhysicalFile(file, "text/plain");
        }
    }
}