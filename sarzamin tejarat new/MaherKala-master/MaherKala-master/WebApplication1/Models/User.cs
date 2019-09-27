using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]

        public string Email { get; set; }
        [Required(ErrorMessage = "پسور کاربری را وارد نمایید")]
        [MaxLength(1000)]
        public string Password { get; set; }
        [Required(ErrorMessage ="نام و نام خانوادگی را وارد کنید")]
        //[MaxLength(100)]
        public string Fullname { get; set; }
        public bool Status { get; set; }
        [MaxLength(100)]
        public string PhoneNumber { get; set; }
        [MaxLength(100)]
        public string Mobile { get; set; }
        [MaxLength(10)]
        public string PostalCode { get; set; }
        [MaxLength(1000)]
        public string Address { get; set; }
        public bool LinkStatus { get; set; }
        public string Api_Token { get; set; }

        public Role Role { get; set; }


    }
}