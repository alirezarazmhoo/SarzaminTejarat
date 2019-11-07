using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class CompanyAjentProduct
    {
        public int Id { get; set; }
        public int Quty { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
    }
}