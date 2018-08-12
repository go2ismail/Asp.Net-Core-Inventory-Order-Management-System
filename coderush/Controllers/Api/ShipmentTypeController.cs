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
    [Route("api/ShipmentType")]
    public class ShipmentTypeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShipmentTypeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ShipmentType
        [HttpGet]
        public async Task<IActionResult> GetShipmentType()
        {
            List<ShipmentType> Items = await _context.ShipmentType.ToListAsync();
            int Count = Items.Count();
            return Ok(new { Items, Count });
        }


        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<ShipmentType> payload)
        {
            ShipmentType shipmentType = payload.value;
            _context.ShipmentType.Add(shipmentType);
            _context.SaveChanges();
            return Ok(shipmentType);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<ShipmentType> payload)
        {
            ShipmentType shipmentType = payload.value;
            _context.ShipmentType.Update(shipmentType);
            _context.SaveChanges();
            return Ok(shipmentType);
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<ShipmentType> payload)
        {
            ShipmentType shipmentType = _context.ShipmentType
                .Where(x => x.ShipmentTypeId == (int)payload.key)
                .FirstOrDefault();
            _context.ShipmentType.Remove(shipmentType);
            _context.SaveChanges();
            return Ok(shipmentType);

        }
    }
}