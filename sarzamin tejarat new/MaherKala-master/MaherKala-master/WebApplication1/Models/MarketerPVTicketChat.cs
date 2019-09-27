using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public enum TicketStateEnum
    {
        //CreateByAdmin = 1,
        //CreateByUser = 2,
        //ResponceByAdmin = 3,
        //ResponceByUser = 4,
        //Watinig = 5,
        //Close = 6,
        Admin=1,
        User=2
        
    }
    public class MarketerPVTicketChat
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual MarketerUser User { get; set; }
        public DateTime CreateDate { get; set; }
       
        //public DateTime LastUpDate { get; set; }
        
        [MaxLength(5000)]
        public string LastMessage { get; set; }
        //[MaxLength(200)]
        //public string Title { get; set; }
        public TicketStateEnum State { get; set; }
        public bool IsRead { get; set;}
    }
   
}