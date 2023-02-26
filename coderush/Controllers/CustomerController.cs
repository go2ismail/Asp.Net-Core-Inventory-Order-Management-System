using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace coderush.Controllers
{
    [Authorize(Roles = Pages.MainMenu.Customer.RoleName)]
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}