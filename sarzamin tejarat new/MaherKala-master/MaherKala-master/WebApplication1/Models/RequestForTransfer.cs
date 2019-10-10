using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class RequestForTransfer
    {
        public int  Id { get; set; }

        public int MarketerUserID { get; set; }
        public MarketerUser MarketerUser { get; set; }
    }
}