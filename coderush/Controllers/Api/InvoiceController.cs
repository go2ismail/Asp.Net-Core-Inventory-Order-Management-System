using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using coderush.Data;
using coderush.Models;
using coderush.Services;
using coderush.Models.SyncfusionViewModels;
using Microsoft.AspNetCore.Authorization;

namespace coderush.Controllers.Api
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/Invoice")]
    public class InvoiceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INumberSequence _numberSequence;

        public InvoiceController(ApplicationDbContext context,
                        INumberSequence numberSequence)
        {
            _context = context;
            _numberSequence = numberSequence;
        }

        // GET: api/Invoice
        [HttpGet]
        public async Task<IActionResult> GetInvoice()
        {
            List<Invoice> Items = await _context.Invoice.ToListAsync();
            int Count = Items.Count();
            return Ok(new { Items, Count });
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetNotPaidYet()
        {
            List<Invoice> invoices = new List<Invoice>();
            try
            {
                List<PaymentReceive> receives = new List<PaymentReceive>();
                receives = await _context.PaymentReceive.ToListAsync();
                List<int> ids = new List<int>();

                foreach (var item in receives)
                {
                    ids.Add(item.InvoiceId);
                }

                invoices = await _context.Invoice
                    .Where(x => !ids.Contains(x.InvoiceId))
                    .ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
            return Ok(invoices);
        }

        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<Invoice> payload)
        {
            Invoice invoice = payload.value;
            invoice.InvoiceName = _numberSequence.GetNumberSequence("INV");
            _context.Invoice.Add(invoice);
            _context.SaveChanges();
            return Ok(invoice);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<Invoice> payload)
        {
            Invoice invoice = payload.value;
            _context.Invoice.Update(invoice);
            _context.SaveChanges();
            return Ok(invoice);
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<Invoice> payload)
        {
            Invoice invoice = _context.Invoice
                .Where(x => x.InvoiceId == (int)payload.key)
                .FirstOrDefault();
            _context.Invoice.Remove(invoice);
            _context.SaveChanges();
            return Ok(invoice);

        }
    }
}