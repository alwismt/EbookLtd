using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ecom.Controllers
{
    [Route("admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        [Route("dashboard", Name = "dashboard")]
        public IActionResult Index()
        {
            return View();
        }
    }
}