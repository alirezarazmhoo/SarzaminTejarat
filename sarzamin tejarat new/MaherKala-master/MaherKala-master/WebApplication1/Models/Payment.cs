using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        //شماره پیگیری
        public string ReferenceNumber { get; set; }

        
        public string StatusPayment { get; set; }

        public bool PaymentFinished { get; set; }
        public long Amount { get; set; }
        public string BankName { get; set; }

        public User User { get; set; }
        public MarketerUser MarketerUser{ get; set; }
        public DateTime Date { get; set; }
        public bool IsUsed { get; set; }
        public Factor Factor { get; set; }
        public MarketerFactor MarketerFactor { get; set; }
        public bool IsForMarketer { get; set; }
        public Payment()
        {
            this.IsUsed = false;
        }

    }
}