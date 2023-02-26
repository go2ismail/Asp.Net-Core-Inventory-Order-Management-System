using System;
using System.ComponentModel.DataAnnotations;

namespace coderush.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        [Display(Name = "Invoice Number")]
        public string InvoiceName { get; set; }
        [Display(Name = "Shipment")]
        public int ShipmentId { get; set; }
        [Display(Name = "Invoice Date")]
        public DateTimeOffset InvoiceDate { get; set; }
        [Display(Name = "Invoice Due Date")]
        public DateTimeOffset InvoiceDueDate { get; set; }
        [Display(Name = "Invoice Type")]
        public int InvoiceTypeId { get; set; }
    }
}
