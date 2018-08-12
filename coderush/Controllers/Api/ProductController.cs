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
    [Route("api/Product")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<IActionResult> GetProduct()
        {
            List<Product> Items = await _context.Product.ToListAsync();
            int Count = Items.Count();
            return Ok(new { Items, Count });
        }



        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<Product> payload)
        {
            Product product = payload.value;
            _context.Product.Add(product);
            _context.SaveChanges();
            return Ok(product);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<Product> payload)
        {
            Product product = payload.value;
            _context.Product.Update(product);
            _context.SaveChanges();
            return Ok(product);
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<Product> payload)
        {
            Product product = _context.Product
                .Where(x => x.ProductId == (int)payload.key)
                .FirstOrDefault();
            _context.Product.Remove(product);
            _context.SaveChanges();
            return Ok(product);

        }
    }
}