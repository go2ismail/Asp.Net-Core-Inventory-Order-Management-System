﻿using System.ComponentModel.DataAnnotations;

namespace coderush.Models
{
    public class Branch
    {
        public int BranchId { get; set; }
        [Required]
        public string BranchName { get; set; }
        public string Description { get; set; }
        [Display(Name = "Currency")]
        public int CurrencyId { get; set; }
        [Display(Name = "Street Address")]
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }
    }
}
