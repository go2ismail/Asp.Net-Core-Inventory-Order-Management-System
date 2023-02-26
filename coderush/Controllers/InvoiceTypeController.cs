using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace coderush.Controllers
{
    [Authorize(Roles = Pages.MainMenu.InvoiceType.RoleName)]
    public class InvoiceTypeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}