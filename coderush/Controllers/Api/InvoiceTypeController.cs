using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using coderush.Data;
using coderush.Models;
using coderush.Models.SyncfusionViewModels;
using Microsoft.AspNetCore.Authorization;

namespace coderush.Controllers.Api
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/InvoiceType")]
    public class InvoiceTypeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InvoiceTypeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/InvoiceType
        [HttpGet]
        public async Task<IActionResult> GetInvoiceType()
        {
            List<InvoiceType> Items = await _context.InvoiceType.ToListAsync();
            int Count = Items.Count();
            return Ok(new { Items, Count });
        }


        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<InvoiceType> payload)
        {
            InvoiceType invoiceType = payload.value;
            _context.InvoiceType.Add(invoiceType);
            _context.SaveChanges();
            return Ok(invoiceType);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<InvoiceType> payload)
        {
            InvoiceType invoiceType = payload.value;
            _context.InvoiceType.Update(invoiceType);
            _context.SaveChanges();
            return Ok(invoiceType);
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<InvoiceType> payload)
        {
            InvoiceType invoiceType = _context.InvoiceType
                .Where(x => x.InvoiceTypeId == (int)payload.key)
                .FirstOrDefault();
            _context.InvoiceType.Remove(invoiceType);
            _context.SaveChanges();
            return Ok(invoiceType);

        }
    }
}