using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class ConfirmEmail
    {
        [Key]
        public int Id { get; set; }
        public string Key { get; set; }
        public User User { get; set; }
        public bool IsUsed { get; set; }
    }
}