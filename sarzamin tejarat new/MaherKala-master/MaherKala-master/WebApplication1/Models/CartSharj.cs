using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class CartSharj
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int CartType { get; set; }
		public int Price { get; set; }
		public int SharjPrice { get; set; }
    }
}