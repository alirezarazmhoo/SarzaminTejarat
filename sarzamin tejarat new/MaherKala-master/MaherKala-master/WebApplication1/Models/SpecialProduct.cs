using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class SpecialProduct
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public DateTime ExpireDate { get; set; }
        public string GetDays()
        {
            decimal day = (decimal)((this.ExpireDate.Date - DateTime.Now.Date).TotalDays);
            return Math.Ceiling(day).ToString();
        }
        public string GetDate()
        {

            string res = "";
            PersianCalendar p = new PersianCalendar();
            res+= p.GetYear(ExpireDate);
            res += "/";
            res += p.GetMonth(ExpireDate);
            res += "/";
            res += p.GetDayOfMonth(ExpireDate);
            return res;
        }
    }
}