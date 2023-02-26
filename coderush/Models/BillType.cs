using System.ComponentModel.DataAnnotations;

namespace coderush.Models
{
    public class BillType
    {
        public int BillTypeId { get; set; }
        [Required]
        public string BillTypeName { get; set; }
        public string Description { get; set; }
    }
}
