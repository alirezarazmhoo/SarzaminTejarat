using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Members
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "نام  کاربری را وارد نمایید")]
        [MaxLength(100)]
        public string UserName { get; set; }
        [Required(ErrorMessage = "پسور کاربری را وارد نمایید")]
        [MaxLength(1000)]
        public string Password { get; set; }
        [Required(ErrorMessage = "تلفن همراه کاربری را وارد نمایید")]
        [MaxLength(100)]
        public string Mobile { get; set; }
        [Required(ErrorMessage = "آدرس کاربری را وارد نمایید")]
        [MaxLength(2000)]
        public string Address { get; set; }
    }
}