using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace coderush.Controllers
{
    [Authorize(Roles = Pages.MainMenu.Invoice.RoleName)]
    public class InvoiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}