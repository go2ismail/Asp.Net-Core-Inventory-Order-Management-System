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
    [Route("api/CashBank")]
    public class CashBankController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CashBankController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CashBank
        [HttpGet]
        public async Task<IActionResult> GetCashBank()
        {
            List<CashBank> Items = await _context.CashBank.ToListAsync();
            int Count = Items.Count();
            return Ok(new { Items, Count });
        }

        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<CashBank> payload)
        {
            CashBank cashBank = payload.value;
            _context.CashBank.Add(cashBank);
            _context.SaveChanges();
            return Ok(cashBank);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<CashBank> payload)
        {
            CashBank cashBank = payload.value;
            _context.CashBank.Update(cashBank);
            _context.SaveChanges();
            return Ok(cashBank);
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<CashBank> payload)
        {
            CashBank cashBank = _context.CashBank
                .Where(x => x.CashBankId == (int)payload.key)
                .FirstOrDefault();
            _context.CashBank.Remove(cashBank);
            _context.SaveChanges();
            return Ok(cashBank);

        }
    }
}