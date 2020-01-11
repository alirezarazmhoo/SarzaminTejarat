using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class UserConversions
    {
        public int Id { get; set; }
        public string LastMessage { get; set; }
        public DateTime CreateDate { get; set; }
        public int State { get; set; }
        //public int UserId { get; set; }
        public int TargetUserId { get; set; }
        public int? MarketerUserID { get; set; }
		public bool IsRead { get; set; }
        public MarketerUser MarketerUser { get; set; }
    }
}