using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class MarketerSlider
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "عکس را ارسال کنید")]
        [MaxLength(1000)]
        public string Image { get; set; }
        [MaxLength(100)]
        public string Link { get; set; }
    }
}