using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "نام  کالا را وارد نمایید")]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required(ErrorMessage = "تعداد کالا را وارد نمایید")]
        public int Qty { get; set; }
        [Required(ErrorMessage = "توضیحات محصول را وارد نمایید")]
        //[MaxLength(1000)]
        public string Desc { get; set; }
        [Required(ErrorMessage = "تصویر کوچک وارد نمایید")]
        [MaxLength(1000)]
        public string Thumbnail { get; set; }
        [Required(ErrorMessage ="قیمت را وارد کنید")]
        //قیمت برای مشتری عادی
        public long Price { get; set; }
        //قیمت برای بازاریاب 
        public long MarketerPrice { get; set; }
        //قیمت برای خریدار عمده
        public long MultiplicationBuyerPrice { get; set; }
        //قیمت برای خرده فروش
        public long RetailerPrice { get; set; }

        public int? CompanyID { get; set; }
        public Company Company { get; set; }


        public string RealPrice
        {
            get {
                return string.Format("{0:0,0}", this.Price - this.Discount);

                    }
            
        }
        public string SepratePrice
        {
            get
            {
                return string.Format("{0:0,0}", this.Price );

            }

        }

        [Required(ErrorMessage = "تخفیف را وارد کنید")]

        public int Discount { get; set; }
        [Required(ErrorMessage ="تصویر محصول الزامی است")]
        //[MaxLength(1000)]
        public string Main_Image { get; set; }
        [MaxLength(1000)]
        public string Images{ get; set; }
       [MaxLength(100)]
        public string Tags { get; set; }
        
        public int Like { get; set; }
        public int TotalVotes { get; set; }
        public bool Status { get; set; }
        public int TotalComment { get; set; }
        public string Color { get; set; }
        public int CategoryID { get; set; }
        public Category Category { get; set; }
        public List<Comment> Comments{ get; set; }
        public List<ProductPresent> ProductPercents { get; set; }
        public bool IsOnlyForMarketer { get; set; }

        public bool IsOnlyForMultipation { get; set; }
        public bool IsOnlyForRetailer { get; set; }
        public int Percent()
        {
            
            int f = (int)(Discount * 100.00 / Price) ;
            
            return f;
        }
        public string GetLike()
        {
            float res = 0;
            if (this.TotalVotes > 0)
            {
                res = (float)((float)this.Like / (float)this.TotalVotes);
                res = res * 5;
                return res.ToString().Replace('/', '.');
            }
            return res.ToString();
        }
        public Product()
        {
            //Category = new Category();
            Comments = new List<Models.Comment>();
        }

    }
    
}