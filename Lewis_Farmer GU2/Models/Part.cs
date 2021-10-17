using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
namespace Lewis_Farmer_GU2.Models
{
    public class Part
    {
        [Key]
        public virtual string PartId { get; set; }

        public virtual string PartName { get; set; }

        public virtual string PartType { get; set; }

        [Range(0, int.MaxValue)]
        public virtual int StockLevel { get; set; }

        [ForeignKey("PartSupplier")]
        public virtual string SupplierId { get; set; }
        public virtual Supplier PartSupplier { get; set; }

        [Range(0.00, double.MaxValue)]
        //[DisplayFormat(DataFormatString = "###,###,##0.00")]
        [DataType(DataType.Currency)]
        public virtual double PartCost { get; set; }

        public string PartCompatibility { get; set; }
    }
}