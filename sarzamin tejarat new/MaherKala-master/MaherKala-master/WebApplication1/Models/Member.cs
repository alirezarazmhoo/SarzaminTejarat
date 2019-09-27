using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Member
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "نام  کاربری را وارد نمایید")]
        [MaxLength(100)]
        public string UserName { get; set; }
        [MaxLength(1000)]
        [Required(ErrorMessage = "پسور کاربری را وارد نمایید")]
        public string Password { get; set; }
    }
}