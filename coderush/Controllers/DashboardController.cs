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