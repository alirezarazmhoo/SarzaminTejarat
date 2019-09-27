using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class MarketerFactor
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="نام خریدار را وارد کنید")]
        [MaxLength(50, ErrorMessage = "نام بسیار طولانی است")]
        [MinLength(3, ErrorMessage = "نام بسیار کوتاه است")]
        public string Buyer { get; set; }

        [Required(ErrorMessage = "آدرس خریدار را وارد کنید")]
        [MaxLength(1000, ErrorMessage = "آدرس بسیار طولانی است")]
        [MinLength(5, ErrorMessage = "آدرس بسیار کوتاه است")]
        public string BuyerAddress { get; set; }

        [Required(ErrorMessage = "موبایل خریدار را وارد کنید")]
        [MaxLength(15, ErrorMessage = "موبایل بسیار طولانی است")]
        public string BuyerMobile { get; set; }

        [Required(ErrorMessage = "تلفن خریدار را وارد کنید")]
        [MaxLength(15, ErrorMessage = "تلفن بسیار طولانی است")]
        public string BuyerPhoneNumber { get; set; }

        public DateTime Date { get; set; }
        public List<MarketerFactorItem> MarketerFactorItems { get; set; }
        public MarketerUser MarketerUser { get; set; }

        public long TotalPrice { get; set; }
        
        //0->paid
        //1->not paid
        //2->returned
        public int Status { get; set; }

        public bool IsAdminShow { get; set; }
        public bool IsAdminCheck{ get; set; }

        [Required(ErrorMessage ="کدپستی را وارد کنید")]
        [MaxLength(10,ErrorMessage ="طول کدپستی باید ده رقم باشد")]
        [MinLength(10, ErrorMessage = "طول کدپستی باید ده رقم باشد")]
        public string BuyerPostalCode { get; set; }
        

        public long ComputeTotalPrice()
        {
            long sum = 0;
            foreach (var item in MarketerFactorItems)
            {
                sum += (item.Product.Price - item.Product.Discount) * item.Qty;
            }
            return sum;
        }
        public MarketerFactor()
        {
            MarketerFactorItems = new List<MarketerFactorItem>();
            this.MarketerUser = new MarketerUser();
        }
    }
}