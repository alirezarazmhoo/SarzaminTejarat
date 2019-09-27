using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Email
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Address { get; set; }
    }
}