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
    [Route("api/UnitOfMeasure")]
    public class UnitOfMeasureController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UnitOfMeasureController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/UnitOfMeasure
        [HttpGet]
        public async Task<IActionResult> GetUnitOfMeasure()
        {
            List<UnitOfMeasure> Items = await _context.UnitOfMeasure.ToListAsync();
            int Count = Items.Count();
            return Ok(new { Items, Count });
        }

        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<UnitOfMeasure> payload)
        {
            UnitOfMeasure unitOfMeasure = payload.value;
            _context.UnitOfMeasure.Add(unitOfMeasure);
            _context.SaveChanges();
            return Ok(unitOfMeasure);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<UnitOfMeasure> payload)
        {
            UnitOfMeasure unitOfMeasure = payload.value;
            _context.UnitOfMeasure.Update(unitOfMeasure);
            _context.SaveChanges();
            return Ok(unitOfMeasure);
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<UnitOfMeasure> payload)
        {
            UnitOfMeasure unitOfMeasure = _context.UnitOfMeasure
                .Where(x => x.UnitOfMeasureId == (int)payload.key)
                .FirstOrDefault();
            _context.UnitOfMeasure.Remove(unitOfMeasure);
            _context.SaveChanges();
            return Ok(unitOfMeasure);

        }
    }
}