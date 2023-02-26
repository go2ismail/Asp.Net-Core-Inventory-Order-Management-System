using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace coderush.Controllers
{
    [Authorize(Roles = Pages.MainMenu.ShipmentType.RoleName)]
    public class ShipmentTypeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}