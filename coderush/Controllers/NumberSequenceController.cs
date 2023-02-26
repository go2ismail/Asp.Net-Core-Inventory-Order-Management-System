using Microsoft.AspNetCore.Mvc;

namespace coderush.Controllers
{
    public class NumberSequenceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}