using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class SaledProducts
    {
        public int Id { get; set; }
        public int SaledCount { get; set; }
        public int? ProductID { get; set; }
        public Product Product { get; set; }
        public int? CompanyID { get; set; }
        public Company Company { get; set; } 

    }
}