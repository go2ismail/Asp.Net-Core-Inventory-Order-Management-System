using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace coderush.Controllers
{
    [Authorize(Roles = Pages.MainMenu.GoodsReceivedNote.RoleName)]
    public class GoodsReceivedNoteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}