using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace coderush.Controllers
{
    [Authorize(Roles = Pages.MainMenu.ProductType.RoleName)]
    public class ProductTypeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}