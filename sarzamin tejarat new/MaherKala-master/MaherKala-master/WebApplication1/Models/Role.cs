using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string RoleNameFa { get; set; }
        [Required]
        public string RoleNameEn{ get; set; }
        public List<User> Users{ get; set; }
    }
}