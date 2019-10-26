using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class SendMessage
    {
        public int Id { get; set; }
        public string MessageContent { get; set; }
        public byte UserType { get; set; }
    }
}