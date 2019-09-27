using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class MarketerActiveAccountTicket
    {
        public int Id { get; set; }
        [DisplayName("قیمت")]
        [Required(ErrorMessage = "قیمت را مشخص کنید")]
        public long Price { get; set; }
    }
}