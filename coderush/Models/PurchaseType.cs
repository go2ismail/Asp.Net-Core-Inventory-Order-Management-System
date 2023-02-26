using System.ComponentModel.DataAnnotations;

namespace coderush.Models
{
    public class PurchaseType
    {
        public int PurchaseTypeId { get; set; }
        [Required]
        public string PurchaseTypeName { get; set; }
        public string Description { get; set; }
    }
}
