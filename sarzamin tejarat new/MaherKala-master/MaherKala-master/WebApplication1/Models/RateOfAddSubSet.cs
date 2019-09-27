using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class RateOfAddSubSet
    {
        public int Id { get; set; }
        [DisplayName("قیمت تیکت")]
        [Required(ErrorMessage ="قیمت تیکت خالی است")]
        public int Price { get; set; }
        [DisplayName("تعداد زیر مجموعه ای که اضافه میکند")]
        [Required(ErrorMessage = "تعداد زیر مجموعه را مشخص کنید")]
        public int AddSubsetCounts { get; set; }
    }
}