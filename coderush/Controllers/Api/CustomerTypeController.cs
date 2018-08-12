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
    [Route("api/CustomerType")]
    public class CustomerTypeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerTypeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CustomerType
        [HttpGet]
        public async Task<IActionResult> GetCustomerType()
        {
            List<CustomerType> Items = await _context.CustomerType.ToListAsync();
            int Count = Items.Count();
            return Ok(new { Items, Count });
        }



        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<CustomerType> payload)
        {
            CustomerType customerType = payload.value;
            _context.CustomerType.Add(customerType);
            _context.SaveChanges();
            return Ok(customerType);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<CustomerType> payload)
        {
            CustomerType customerType = payload.value;
            _context.CustomerType.Update(customerType);
            _context.SaveChanges();
            return Ok(customerType);
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<CustomerType> payload)
        {
            CustomerType customerType = _context.CustomerType
                .Where(x => x.CustomerTypeId == (int)payload.key)
                .FirstOrDefault();
            _context.CustomerType.Remove(customerType);
            _context.SaveChanges();
            return Ok(customerType);

        }
    }
}