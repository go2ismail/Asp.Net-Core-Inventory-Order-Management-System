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
    [Route("api/SalesOrder")]
    public class SalesOrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INumberSequence _numberSequence;

        public SalesOrderController(ApplicationDbContext context,
                        INumberSequence numberSequence)
        {
            _context = context;
            _numberSequence = numberSequence;
        }

        // GET: api/SalesOrder
        [HttpGet]
        public async Task<IActionResult> GetSalesOrder()
        {
            List<SalesOrder> Items = await _context.SalesOrder.ToListAsync();
            int Count = Items.Count();
            return Ok(new { Items, Count });
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetNotShippedYet()
        {
            List<SalesOrder> salesOrders = new List<SalesOrder>();
            try
            {
                List<Shipment> shipments = new List<Shipment>();
                shipments = await _context.Shipment.ToListAsync();
                List<int> ids = new List<int>();

                foreach (var item in shipments)
                {
                    ids.Add(item.SalesOrderId);
                }

                salesOrders = await _context.SalesOrder
                    .Where(x => !ids.Contains(x.SalesOrderId))
                    .ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
            return Ok(salesOrders);
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            SalesOrder result = await _context.SalesOrder
                .Where(x => x.SalesOrderId.Equals(id))
                .Include(x => x.SalesOrderLines)
                .FirstOrDefaultAsync();

            return Ok(result);
        }

        private void UpdateSalesOrder(int salesOrderId)
        {
            try
            {
                SalesOrder salesOrder = new SalesOrder();
                salesOrder = _context.SalesOrder
                    .Where(x => x.SalesOrderId.Equals(salesOrderId))
                    .FirstOrDefault();

                if (salesOrder != null)
                {
                    List<SalesOrderLine> lines = new List<SalesOrderLine>();
                    lines = _context.SalesOrderLine.Where(x => x.SalesOrderId.Equals(salesOrderId)).ToList();

                    //update master data by its lines
                    salesOrder.Amount = lines.Sum(x => x.Amount);
                    salesOrder.SubTotal = lines.Sum(x => x.SubTotal);

                    salesOrder.Discount = lines.Sum(x => x.DiscountAmount);
                    salesOrder.Tax = lines.Sum(x => x.TaxAmount);

                    salesOrder.Total = salesOrder.Freight + lines.Sum(x => x.Total);

                    _context.Update(salesOrder);

                    _context.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<SalesOrder> payload)
        {
            SalesOrder salesOrder = payload.value;
            salesOrder.SalesOrderName = _numberSequence.GetNumberSequence("SO");
            _context.SalesOrder.Add(salesOrder);
            _context.SaveChanges();
            this.UpdateSalesOrder(salesOrder.SalesOrderId);
            return Ok(salesOrder);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<SalesOrder> payload)
        {
            SalesOrder salesOrder = payload.value;
            _context.SalesOrder.Update(salesOrder);
            _context.SaveChanges();
            return Ok(salesOrder);
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<SalesOrder> payload)
        {
            SalesOrder salesOrder = _context.SalesOrder
                .Where(x => x.SalesOrderId == (int)payload.key)
                .FirstOrDefault();
            _context.SalesOrder.Remove(salesOrder);
            _context.SaveChanges();
            return Ok(salesOrder);

        }
    }
}