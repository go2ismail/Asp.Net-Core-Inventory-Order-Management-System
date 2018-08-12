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
    [Route("api/Currency")]
    public class CurrencyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CurrencyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Currency
        [HttpGet]
        public async Task<IActionResult> GetCurrency()
        {
            List<Currency> Items = await _context.Currency.ToListAsync();
            int Count = Items.Count();
            return Ok(new { Items, Count });
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetByBranchId([FromRoute]int id)
        {
            Branch branch = new Branch();
            Currency currency = new Currency();
            branch = await _context.Branch.SingleOrDefaultAsync(x => x.BranchId.Equals(id));
            if (branch != null && branch.CurrencyId != 0)
            {
                currency = await _context.Currency.SingleOrDefaultAsync(x => x.CurrencyId.Equals(branch.CurrencyId));
                
            }
            return Ok(currency);
        }

        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<Currency> payload)
        {
            Currency currency = payload.value;
            _context.Currency.Add(currency);
            _context.SaveChanges();
            return Ok(currency);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<Currency> payload)
        {
            Currency currency = payload.value;
            _context.Currency.Update(currency);
            _context.SaveChanges();
            return Ok(currency);
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<Currency> payload)
        {
            Currency currency = _context.Currency
                .Where(x => x.CurrencyId == (int)payload.key)
                .FirstOrDefault();
            _context.Currency.Remove(currency);
            _context.SaveChanges();
            return Ok(currency);

        }
    }
}