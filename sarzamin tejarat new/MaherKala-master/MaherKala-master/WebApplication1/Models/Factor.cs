using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Factor
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50, ErrorMessage = "نام بسیار طولانی است")]
        [MinLength(3, ErrorMessage = "نام بسیار کوتاه است")]
        public string Buyer { get; set; }
        [MaxLength(1000, ErrorMessage = "آدرس بسیار طولانی است")]
        [MinLength(5, ErrorMessage = "آدرس بسیار کوتاه است")]
        public string Address { get; set; }
        [MaxLength(15, ErrorMessage = "موبایل بسیار طولانی است")]
        public string Mobile { get; set; }
        [MaxLength(15, ErrorMessage = "تلفن بسیار طولانی است")]
        public string PhoneNumber { get; set; }
        public DateTime Date { get; set; }
        public List<FactorItem> FactorItems { get; set; }
        public User User { get; set; }
        public long TotalPrice { get; set; }
        public bool Status { get; set; }

        public bool IsAdminShow { get; set; }
        [MaxLength(20)]
        public string PostalCode { get; set; }

        //esf:1 , najaf:2 , other: 3
        public int City_Id { get; set; }
        public int TransportationFee { get; set; }
        public string Discount_Code { get; set; }
        public int Discount_Amount { get; set; }

        public long ComputeTotalPrice()
        {
            long sum = 0;
            foreach (var item in FactorItems)
            {
                sum += (item.Product.Price - item.Product.Discount) * item.Qty;
            }
            return sum + this.TransportationFee - this.Discount_Amount;
        }
        public long ComputeTotalPrice2()
        {
            long sum = 0;
            foreach (var item in FactorItems)
            {
                sum += (item.Product.Price - item.Product.Discount) * item.Qty;
            }
            return sum  - this.Discount_Amount;
        }
        public Factor()
        {
            FactorItems = new List<FactorItem>();
            User = new User();
        }
    }
}