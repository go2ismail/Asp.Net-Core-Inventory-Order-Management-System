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