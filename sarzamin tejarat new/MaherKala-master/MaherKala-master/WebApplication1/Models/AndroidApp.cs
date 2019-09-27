using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class AndroidApp
    {
        [Key]
        public int ID { get; set; }
        [MaxLength(70)]
        [Display(Name = "فایل apk")]
        public string AppAndroidUrl { get; set; }
        [MaxLength(70)]
        [Display(Name = "ورژن")]
        public string Version { get; set; }
        [NotMapped]
        [DataType(DataType.Upload)]
        [Display(Name = " apk انتخاب فایل")]
        public HttpPostedFileBase files { get; set; }
    }
}