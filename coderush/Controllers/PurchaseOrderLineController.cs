using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace coderush.Controllers
{
    [Authorize(Roles = Pages.MainMenu.PurchaseOrder.RoleName)]
    public class PurchaseOrderLineController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}