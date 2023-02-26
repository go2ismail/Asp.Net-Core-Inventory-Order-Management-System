using System;
using System.ComponentModel.DataAnnotations;

namespace coderush.Models
{
    public class PaymentReceive
    {
        public int PaymentReceiveId { get; set; }
        [Display(Name = "Payment Number")]
        public string PaymentReceiveName { get; set; }
        [Display(Name = "Invoice")]
        public int InvoiceId { get; set; }
        public DateTimeOffset PaymentDate { get; set; }
        [Display(Name = "Payment Type")]
        public int PaymentTypeId { get; set; }
        public double PaymentAmount { get; set; }
        [Display(Name = "Full Payment")]
        public bool IsFullPayment { get; set; } = true;
    }
}
