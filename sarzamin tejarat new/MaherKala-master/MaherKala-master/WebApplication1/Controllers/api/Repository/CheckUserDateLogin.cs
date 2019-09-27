using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Controllers.api.Repository
{
    public class CheckUserDateLogin
    {
        DBContext db = new DBContext();

        public bool ActiveForaYear(int userId)
        {
            var UserItem = db.MarketerUsers.Where(p => p.Id == userId).FirstOrDefault();
            var Today = DateTime.Now;
            var totalDays = (Today - UserItem.CreatedDate).TotalDays;
            if (totalDays > 365)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public bool CheckLazyMarketerUser(int UserId)
        {
            var item = db.MarketerLimitSale.FirstOrDefault();
           
            var userlastFactor = db.MarketerFactor.Where(p => p.MarketerUser.Id == UserId).ToList();
            var max = 0;
            foreach (var last in userlastFactor)
            {
                max = last.Id;
               if(last.Id >= max)
                {
                    max = last.Id;

                }

            }
            var findlastFactor = db.MarketerFactor.Where(p => p.Id == max).FirstOrDefault();

        

            if(userlastFactor == null)
            {
                return false;
            }


            DateTime ActivedDateByAdmin = item.ActiveDate;

            var totalDays = (ActivedDateByAdmin - findlastFactor.Date ).TotalDays;
            if (totalDays > item.Days)
            {
                return false;

            }
            else
            {
                return true;
            }

        }
    }
}