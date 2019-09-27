using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class PlanType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public  ICollection<Plannn> Plans { get; set; }
        public ICollection<MarketerImprovePlan> MarketerImprovePlans { get; set; }
    }

}