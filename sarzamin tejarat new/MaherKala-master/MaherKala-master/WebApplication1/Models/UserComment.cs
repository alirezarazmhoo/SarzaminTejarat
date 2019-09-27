using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class UserComment
    {
        [Key]
        public int Id { get; set; }
        public Comment Comment { get; set; }
        public User User { get; set; }
    }
}