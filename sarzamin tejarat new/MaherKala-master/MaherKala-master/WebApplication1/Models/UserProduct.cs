using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class UserProduct
    {
        [Key]
        public int Id { get; set; }
        public Product Product { get; set; }
        public User User { get; set; }
    }
}