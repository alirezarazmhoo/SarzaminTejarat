using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Models
{
    public class MarketerPrize
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(40)]
        public string Title { get; set; }
        [AllowHtml]

        public string Text { get; set; }
        [MaxLength(300)]
        public string PicAddress { get; set; }
    }
}