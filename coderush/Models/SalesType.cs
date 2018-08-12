using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.Models
{
    public class SalesType
    {
        public int SalesTypeId { get; set; }
        [Required]
        public string SalesTypeName { get; set; }
        public string Description { get; set; }
    }
}
