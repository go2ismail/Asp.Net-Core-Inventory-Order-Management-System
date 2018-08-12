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
    [Route("api/PaymentVoucher")]
    public class PaymentVoucherController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INumberSequence _numberSequence;

        public PaymentVoucherController(ApplicationDbContext context,
                        INumberSequence numberSequence)
        {
            _context = context;
            _numberSequence = numberSequence;
        }

        // GET: api/PaymentVoucher
        [HttpGet]
        public async Task<IActionResult> GetPaymentVoucher()
        {
            List<PaymentVoucher> Items = await _context.PaymentVoucher.ToListAsync();
            int Count = Items.Count();
            return Ok(new { Items, Count });
        }



        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<PaymentVoucher> payload)
        {
            PaymentVoucher paymentVoucher = payload.value;
            paymentVoucher.PaymentVoucherName = _numberSequence.GetNumberSequence("PAYVCH");
            _context.PaymentVoucher.Add(paymentVoucher);
            _context.SaveChanges();
            return Ok(paymentVoucher);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<PaymentVoucher> payload)
        {
            PaymentVoucher paymentVoucher = payload.value;
            _context.PaymentVoucher.Update(paymentVoucher);
            _context.SaveChanges();
            return Ok(paymentVoucher);
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<PaymentVoucher> payload)
        {
            PaymentVoucher paymentVoucher = _context.PaymentVoucher
                .Where(x => x.PaymentvoucherId == (int)payload.key)
                .FirstOrDefault();
            _context.PaymentVoucher.Remove(paymentVoucher);
            _context.SaveChanges();
            return Ok(paymentVoucher);

        }
    }
}