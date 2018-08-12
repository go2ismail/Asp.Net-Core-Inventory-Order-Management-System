using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using coderush.Data;
using coderush.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace coderush.Controllers
{
    [Authorize(Roles = Pages.MainMenu.PurchaseOrder.RoleName)]
    public class PurchaseOrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PurchaseOrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(int id)
        {
            PurchaseOrder purchaseOrder = _context.PurchaseOrder.SingleOrDefault(x => x.PurchaseOrderId.Equals(id));

            if (purchaseOrder == null)
            {
                return NotFound();
            }

            return View(purchaseOrder);
        }
    }
}