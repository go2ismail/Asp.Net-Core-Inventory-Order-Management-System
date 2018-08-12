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
    [Route("api/Shipment")]
    public class ShipmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INumberSequence _numberSequence;

        public ShipmentController(ApplicationDbContext context,
                        INumberSequence numberSequence)
        {
            _context = context;
            _numberSequence = numberSequence;
        }

        // GET: api/Shipment
        [HttpGet]
        public async Task<IActionResult> GetShipment()
        {
            List<Shipment> Items = await _context.Shipment.ToListAsync();
            int Count = Items.Count();
            return Ok(new { Items, Count });
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetNotInvoicedYet()
        {
            List<Shipment> shipments = new List<Shipment>();
            try
            {
                List<Invoice> invoices = new List<Invoice>();
                invoices = await _context.Invoice.ToListAsync();
                List<int> ids = new List<int>();

                foreach (var item in invoices)
                {
                    ids.Add(item.ShipmentId);
                }

                shipments = await _context.Shipment
                    .Where(x => !ids.Contains(x.ShipmentId))
                    .ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
            return Ok(shipments);
        }

        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<Shipment> payload)
        {
            Shipment shipment = payload.value;
            shipment.ShipmentName = _numberSequence.GetNumberSequence("DO");
            _context.Shipment.Add(shipment);
            _context.SaveChanges();
            return Ok(shipment);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<Shipment> payload)
        {
            Shipment shipment = payload.value;
            _context.Shipment.Update(shipment);
            _context.SaveChanges();
            return Ok(shipment);
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<Shipment> payload)
        {
            Shipment shipment = _context.Shipment
                .Where(x => x.ShipmentId == (int)payload.key)
                .FirstOrDefault();
            _context.Shipment.Remove(shipment);
            _context.SaveChanges();
            return Ok(shipment);

        }
    }
}