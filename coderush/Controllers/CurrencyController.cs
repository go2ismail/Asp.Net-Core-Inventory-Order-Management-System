using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace coderush.Controllers
{
    [Authorize(Roles = Pages.MainMenu.Currency.RoleName)]
    public class CurrencyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}