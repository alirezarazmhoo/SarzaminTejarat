using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class SendMessage
    {
        public int id { get; set; }
        public string LastMessage { get; set; }
        public DateTime createDate { get; set; } 
        public int state { get; set; }
        public bool isRead { get; set; }
        public byte UserType { get; set; }

    }
}