using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace coderush.Controllers
{
    [Authorize(Roles = Pages.MainMenu.PaymentVoucher.RoleName)]
    public class PaymentVoucherController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}