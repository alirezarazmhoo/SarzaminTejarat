using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Complaint
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="عنوان را وارد کنید")]
        [MaxLength(50,ErrorMessage ="عنوان بزرگ  تر از حد مجاز است")]
        public string Title { get; set; }
        [Required(ErrorMessage = "متن شکایت را وارد کنید")]
        [MaxLength(100,ErrorMessage = "متن شکایت بزرگ  تر از حد مجاز است")]
        public string Text { get; set; }
        [Index]
        public bool IsAdminShow { get; set; }
        [MaxLength(50, ErrorMessage = "تلفن یا ایمیل بزرگ  تر از حد مجاز است")]
        [Required(ErrorMessage = "تلفن تماس یا ایمیل را وارد کنید")]
        public string Call { get; set; }

    }
}