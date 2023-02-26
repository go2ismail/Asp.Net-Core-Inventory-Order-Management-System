using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace coderush.Controllers
{
    [Authorize(Roles = Pages.MainMenu.CustomerType.RoleName)]
    public class CustomerTypeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}