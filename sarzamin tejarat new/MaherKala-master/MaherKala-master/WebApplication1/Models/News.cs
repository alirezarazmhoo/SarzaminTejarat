using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Models
{
    public class News
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="عنوان خبر را وارد کنید")]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required(ErrorMessage = "متن خبر را وارد کنید")]
        //[MaxLength(1000)]
        public string Desc{ get; set; }
        [AllowHtml]
        [MaxLength(2000)]
        public string Text { get; set; }
    }
}