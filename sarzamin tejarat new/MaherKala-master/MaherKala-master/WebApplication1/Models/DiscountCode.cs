using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class DiscountCode
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Code { get; set; }
        public int Price { get; set; }
        public DateTime ExpireDate { get; set; }
        public bool Status { get; set; }

        public string getDate()
        {
            PersianCalendar p = new PersianCalendar();
            return p.GetYear(ExpireDate) + "/" + p.GetMonth(ExpireDate) + "/" + p.GetDayOfMonth(ExpireDate);
        }

    }
}