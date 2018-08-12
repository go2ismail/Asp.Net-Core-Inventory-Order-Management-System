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
    [Route("api/ProductType")]
    public class ProductTypeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductTypeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ProductType
        [HttpGet]
        public async Task<IActionResult> GetProductType()
        {
            List<ProductType> Items = await _context.ProductType.ToListAsync();
            int Count = Items.Count();
            return Ok(new { Items, Count });
        }



        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<ProductType> payload)
        {
            ProductType productType = payload.value;
            _context.ProductType.Add(productType);
            _context.SaveChanges();
            return Ok(productType);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<ProductType> payload)
        {
            ProductType productType = payload.value;
            _context.ProductType.Update(productType);
            _context.SaveChanges();
            return Ok(productType);
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<ProductType> payload)
        {
            ProductType productType = _context.ProductType
                .Where(x => x.ProductTypeId == (int)payload.key)
                .FirstOrDefault();
            _context.ProductType.Remove(productType);
            _context.SaveChanges();
            return Ok(productType);

        }
    }
}