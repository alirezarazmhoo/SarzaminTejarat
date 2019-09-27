using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class UserRecover
    {
        public int Id { get; set; }
        public User User { get; set; }
        public DateTime Time{ get; set; }
        public string Key { get; set; }
        public bool Status { get; set; }
    }
}