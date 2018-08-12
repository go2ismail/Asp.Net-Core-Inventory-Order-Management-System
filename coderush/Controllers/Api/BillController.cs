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
    [Route("api/Bill")]
    public class BillController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INumberSequence _numberSequence;

        public BillController(ApplicationDbContext context,
                        INumberSequence numberSequence)
        {
            _context = context;
            _numberSequence = numberSequence;
        }

        // GET: api/Bill
        [HttpGet]
        public async Task<IActionResult> GetBill()
        {
            List<Bill> Items = await _context.Bill.ToListAsync();
            int Count = Items.Count();
            return Ok(new { Items, Count });
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetNotPaidYet()
        {
            List<Bill> bills = new List<Bill>();
            try
            {
                List<PaymentVoucher> vouchers = new List<PaymentVoucher>();
                vouchers = await _context.PaymentVoucher.ToListAsync();
                List<int> ids = new List<int>();

                foreach (var item in vouchers)
                {
                    ids.Add(item.BillId);
                }

                bills = await _context.Bill
                    .Where(x => !ids.Contains(x.BillId))
                    .ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
            return Ok(bills);
        }

        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<Bill> payload)
        {
            Bill bill = payload.value;
            bill.BillName = _numberSequence.GetNumberSequence("BILL");
            _context.Bill.Add(bill);
            _context.SaveChanges();
            return Ok(bill);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<Bill> payload)
        {
            Bill bill = payload.value;
            _context.Bill.Update(bill);
            _context.SaveChanges();
            return Ok(bill);
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<Bill> payload)
        {
            Bill bill = _context.Bill
                .Where(x => x.BillId == (int)payload.key)
                .FirstOrDefault();
            _context.Bill.Remove(bill);
            _context.SaveChanges();
            return Ok(bill);

        }
    }
}