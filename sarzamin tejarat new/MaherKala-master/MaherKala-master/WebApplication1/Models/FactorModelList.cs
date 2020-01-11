using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class MyFactorModelList
    {
        public int Id { get; set; }
        public string Buyer { get; set; }
        public string BuyerAddress { get; set; }
        public string BuyerMobile { get; set; }
        public string BuyerPhoneNumber { get; set; }
        public string BuyerPostalCode { get; set; }
        public DateTime Date { get; set; }
        public int Product_Id { get; set; }
        public int Qty { get; set; }
        public long UnitPrice { get; set; }
        public string ProductName { get; set; }
        public string ProductImages { get; set; }
        public string Thumbnail { get; set; }
       
        public string ProductDesc { get; set; }
        public string CategoriName { get; set; }
        public long MarketerPrice { get; set; }
   
        public string Tags { get; set; }
        public int Like { get; set; }
        public string Main_Image { get; set; }

        public bool Status { get; set; }
        public int TotalVotes { get; set; }
        public int TotalComment { get; set; }
        public string Color { get; set; }
        public bool IsOnlyForMarketer { get; set; }
    }
}