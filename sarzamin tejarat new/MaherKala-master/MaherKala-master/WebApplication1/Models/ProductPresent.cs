using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class ProductPresent
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="مقدار مینیمم بازه را وارد کنید")]
        public int Min { get; set; }
        [Required(ErrorMessage = "مقدار ماکسیمم بازه را وارد کنید")]
        public int Max { get; set; }
        [Required(ErrorMessage = "درصد کمسیون را وارد کنید")]
        public double Percent { get; set; }
        public Product Product { get; set; }
    }
}