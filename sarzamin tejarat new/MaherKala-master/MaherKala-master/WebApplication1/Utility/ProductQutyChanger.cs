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

                List<int> listPro = new List<int>();
                var listProes = db.CompanyAjentProducts.Where(s => s.hasDone).ToList();
                foreach (var item in listProes)
                {
                    listPro.Add(item.Id);

                }
                foreach (var _item in listPro)
                {
                    var t = db.CompanyAjentProducts.Find(_item);
                    if (t != null)
                    {
                        db.CompanyAjentProducts.Remove(t);
                    }

                }


                var listItems = db.CompanyAjentProducts.ToList();
                DateTime today = DateTime.Now;
                foreach (var item in listItems)
                {
                    int totalhour = 0;

                    totalhour = (today - item.CreatedDate).Hours;
                    if (!item.hasDone && totalhour >24)
                    {
                        var Productitem = db.Products.Where(s => s.Id == item.ProductID).FirstOrDefault();
                        if (Productitem != null)
                        {
                        item.hasDone = true;

                            Productitem.Qty = item.Quty;
                        }
                    }
                }
                db.SaveChanges();
            }
  

        }
    }
}