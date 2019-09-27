using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Commission
    {
        [Key]
        public int Id { get; set; }
        public int Money { get; set; }
        public string Detail { get; set; }
        public string Formula { get; set; }
        public DateTime Date { get; set; }
        public MarketerUser MarketerUser { get; set; }
        public MarketerFactor MarketerFactor { get; set; }
    }
}