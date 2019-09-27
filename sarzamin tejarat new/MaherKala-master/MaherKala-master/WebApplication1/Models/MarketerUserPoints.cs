using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class MarketerUserPoints
    {
        public int Id { get; set; }
        public double UserPoits { get; set; }
        public DateTime SavedDate { get; set; }

        public int MarketerUserId { get; set; }

        public virtual MarketerUser MarketerUser { get; set; }

    }
}