using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class PriceForTranslate
    {
        public int Id { get; set; }
      
        public float MarketerPriceTranslate { get; set; }
       
        public long Marketergratis { get; set; }
        public float BigBuyerPriceTranslate { get; set; }
   
        public long Buyergratis { get; set; }
        public float RetailerPriceTranslate { get; set; }
       
        public long Retailergratis { get; set; }


    }
}