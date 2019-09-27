using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Models
{
    public class WholeSaler
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Fullname { get; set; }
        [MaxLength(1000)]
        public string Address { get; set; }
        [MaxLength(10)]
        public string PostalCode { get; set; }
        [MaxLength(100)]
        public string Phone { get; set; }
        [MaxLength(100)]
        public string Mobile { get; set; }
        [AllowHtml]
        [MaxLength(1000)]
        public string Desc { get; set; }
        public int Type { get; set; }
        public bool IsShow { get; set; }

        public virtual string GetType()
        {
            switch (Type)
            {
                case 1: return "تولید کننده";
                    break;
                case 2:
                    return "وارد کننده";
                    break;
                case 3:
                    return "خریدار عمده";
                    break;
                case 4:
                    return "فروشنده عمده";
                    break;

            }
            return "";
        }

    }
}