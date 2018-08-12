using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace coderush.Controllers
{
    
    public class DashboardController : Controller
    {
        [Authorize(Roles = Pages.MainMenu.Dashboard.RoleName)]
        public IActionResult Index()
        {
            return View();
        }
    }
}