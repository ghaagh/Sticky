using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sticky.Models.Context;

namespace Sticky.API.Dashboard.Controllers
{
    [Route("Home")]
    public class HomeController : Controller
    {
        public HomeController(UserManager<User> userManager)
        {
        }
        [Route("Index")]
        [HttpGet]
        public string Index()
        {
            return "hello to you too ;)";
        }
    }
}