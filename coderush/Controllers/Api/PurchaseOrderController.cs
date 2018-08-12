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
    [Route("api/PurchaseOrder")]
    public class PurchaseOrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INumberSequence _numberSequence;

        public PurchaseOrderController(ApplicationDbContext context,
                        INumberSequence numberSequence)
        {
            _context = context;
            _numberSequence = numberSequence;
        }

        // GET: api/PurchaseOrder
        [HttpGet]
        public async Task<IActionResult> GetPurchaseOrder()
        {
            List<PurchaseOrder> Items = await _context.PurchaseOrder.ToListAsync();
            int Count = Items.Count();
            return Ok(new { Items, Count });
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetNotReceivedYet()
        {
            List<PurchaseOrder> purchaseOrders = new List<PurchaseOrder>();
            try
            {
                List<GoodsReceivedNote> grns = new List<GoodsReceivedNote>();
                grns = await _context.GoodsReceivedNote.ToListAsync();
                List<int> ids = new List<int>();

                foreach (var item in grns)
                {
                    ids.Add(item.PurchaseOrderId);
                }

                purchaseOrders = await _context.PurchaseOrder
                    .Where(x => !ids.Contains(x.PurchaseOrderId))
                    .ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
            return Ok(purchaseOrders);
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            PurchaseOrder result = await _context.PurchaseOrder
                .Where(x => x.PurchaseOrderId.Equals(id))
                .Include(x => x.PurchaseOrderLines)
                .FirstOrDefaultAsync();

            return Ok(result);
        }

        private void UpdatePurchaseOrder(int purchaseOrderId)
        {
            try
            {
                PurchaseOrder purchaseOrder = new PurchaseOrder();
                purchaseOrder = _context.PurchaseOrder
                    .Where(x => x.PurchaseOrderId.Equals(purchaseOrderId))
                    .FirstOrDefault();

                if (purchaseOrder != null)
                {
                    List<PurchaseOrderLine> lines = new List<PurchaseOrderLine>();
                    lines = _context.PurchaseOrderLine.Where(x => x.PurchaseOrderId.Equals(purchaseOrderId)).ToList();

                    //update master data by its lines
                    purchaseOrder.Amount = lines.Sum(x => x.Amount);
                    purchaseOrder.SubTotal = lines.Sum(x => x.SubTotal);

                    purchaseOrder.Discount = lines.Sum(x => x.DiscountAmount);
                    purchaseOrder.Tax = lines.Sum(x => x.TaxAmount);

                    purchaseOrder.Total = purchaseOrder.Freight + lines.Sum(x => x.Total);

                    _context.Update(purchaseOrder);

                    _context.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<PurchaseOrder> payload)
        {
            PurchaseOrder purchaseOrder = payload.value;
            purchaseOrder.PurchaseOrderName = _numberSequence.GetNumberSequence("PO");
            _context.PurchaseOrder.Add(purchaseOrder);
            _context.SaveChanges();
            this.UpdatePurchaseOrder(purchaseOrder.PurchaseOrderId);
            return Ok(purchaseOrder);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<PurchaseOrder> payload)
        {
            PurchaseOrder purchaseOrder = payload.value;
            _context.PurchaseOrder.Update(purchaseOrder);
            _context.SaveChanges();
            this.UpdatePurchaseOrder(purchaseOrder.PurchaseOrderId);
            return Ok(purchaseOrder);
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<PurchaseOrder> payload)
        {
            PurchaseOrder purchaseOrder = _context.PurchaseOrder
                .Where(x => x.PurchaseOrderId == (int)payload.key)
                .FirstOrDefault();
            _context.PurchaseOrder.Remove(purchaseOrder);
            _context.SaveChanges();
            this.UpdatePurchaseOrder(purchaseOrder.PurchaseOrderId);
            return Ok(purchaseOrder);

        }
    }
}