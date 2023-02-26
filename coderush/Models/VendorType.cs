using System.ComponentModel.DataAnnotations;

namespace coderush.Models
{
    public class VendorType
    {
        public int VendorTypeId { get; set; }
        [Required]
        public string VendorTypeName { get; set; }
        public string Description { get; set; }
    }
}
