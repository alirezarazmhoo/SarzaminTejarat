using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Utility
{
    public class ProductQutyChanger
    {
        DBContext db = new DBContext();

        public void QutyChanger()
        {
            var isExist = db.CompanyAjentProducts.Any();
            if (isExist)
            {
                var listItems = db.CompanyAjentProducts.ToList();
             
                foreach (var item in listItems)
                {
                    var Productitem = db.Products.Where(s => s.Id == item.ProductID).FirstOrDefault();
                    if(Productitem != null)
                    {
                  
                        Productitem.Qty = item.Quty;
                    }
                }
                db.SaveChanges();
            }
  

        }
    }
}