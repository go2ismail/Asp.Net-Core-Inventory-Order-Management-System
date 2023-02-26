using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace coderush.Controllers
{
    [Authorize(Roles = Pages.MainMenu.SalesType.RoleName)]
    public class SalesTypeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}