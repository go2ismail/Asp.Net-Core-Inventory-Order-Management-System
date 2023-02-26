using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace coderush.Controllers
{
    [Authorize(Roles = Pages.MainMenu.CashBank.RoleName)]
    public class CashBankController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}