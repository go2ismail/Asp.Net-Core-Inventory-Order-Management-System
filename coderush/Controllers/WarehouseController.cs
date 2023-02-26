using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace coderush.Controllers
{
    [Authorize(Roles = Pages.MainMenu.Warehouse.RoleName)]
    public class WarehouseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}