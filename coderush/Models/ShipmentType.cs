using System.ComponentModel.DataAnnotations;

namespace coderush.Models
{
    public class ShipmentType
    {
        public int ShipmentTypeId { get; set; }
        [Required]
        public string ShipmentTypeName { get; set; }
        public string Description { get; set; }
    }
}
