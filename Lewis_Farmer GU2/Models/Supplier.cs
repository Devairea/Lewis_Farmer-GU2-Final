using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Lewis_Farmer_GU2.Models
{
    public class Supplier
    {
        [Key]
        public string SupplierId { get; set; }

        [Display(Name = "Supplier")]
        public string SupplierName { get; set; }

        [Display(Name = "Email")]
        public string SupplierEmail { get; set; }

        [Display(Name = "Telephone No.")]
        public string SupplierTelephoneNo { get; set; }

        [Display(Name = "Parts We Use")]
        public List<Part> ListOfParts { get; set; }

        public Supplier()
        {
            ListOfParts = new List<Part>();
        }
    }
}