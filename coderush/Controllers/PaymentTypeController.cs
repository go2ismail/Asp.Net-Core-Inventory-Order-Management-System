using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace coderush.Controllers
{
    [Authorize(Roles = Pages.MainMenu.PaymentType.RoleName)]
    public class PaymentTypeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}