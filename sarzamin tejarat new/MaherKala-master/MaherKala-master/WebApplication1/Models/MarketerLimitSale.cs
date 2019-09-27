using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class MarketerLimitSale
    {
        public int Id { get; set; }
        [DisplayName("تعداد روز")]
 
        public int Days { get; set; }
        [DisplayName("قیمت فعال سازی مجدد")]
    
        public long Price { get; set; }
        public DateTime ActiveDate { get; set; } = DateTime.Now;
        public bool Enable { get; set; }
    }
}