using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class PricePointForAddSubset
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "حداقل نرخ افزایش زیرمجموعه را تعیین کنید")]
        public int MinimumPrice { get; set; }
    }
}