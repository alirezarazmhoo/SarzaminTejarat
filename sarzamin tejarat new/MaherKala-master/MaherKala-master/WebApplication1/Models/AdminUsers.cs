using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class AdminUsers
    {
        public int Id { get; set; }
        [DisplayName("نام ")]
        public string FirstName { get; set; }
        [DisplayName("نام خانوادگی")]
        public string LastName { get; set; }
        [DisplayName("همراه")]
        public long Mobile { get; set; }
        [DisplayName("نام کاربری")]
        public string UserName { get; set; }
        [DisplayName("رمزعبور")]
        public string Password { get; set; }

        public ICollection<AdminsInRoles> AdminsInRoles { get; set; }


    }
}