using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class MarketerFactorItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Qty { get; set; }

        [Required]
        [MaxLength(100)]
        public string ProductName { get; set; }
        [Required]
        public long UnitPrice { get; set; }

        public Product Product { get; set; }
        public MarketerFactor MarketerFactor { get; set; }
        public MarketerFactorItem()
        {
            Product = new Product();
        }
    }
}