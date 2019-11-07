using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class UserAnswer
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string UserMessage { get; set; }
        public int MarketerUserID { get; set; }
        public MarketerUser MarketerUser { get; set; }
    }
}