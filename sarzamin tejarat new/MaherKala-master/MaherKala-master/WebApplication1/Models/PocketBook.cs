using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class PocketBook
    {
         [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string FullName { get; set; }
        [MaxLength(11)]
        [MinLength(11)]
        public string Mobile{ get; set; }
        [MaxLength(1000)]
        public string Description{ get; set; }
        [ForeignKey("MarketerUser")]
        public int MarketerUserId { get; set; }
        public virtual MarketerUser MarketerUser { get; set; }
    }
}