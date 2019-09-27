using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class FactorItem
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
        public string Color { get; set; }
        public Product Product { get; set; }
        public Factor Factor { get; set; }
        public FactorItem()
        {
            Product = new Product();
        }
    }
}