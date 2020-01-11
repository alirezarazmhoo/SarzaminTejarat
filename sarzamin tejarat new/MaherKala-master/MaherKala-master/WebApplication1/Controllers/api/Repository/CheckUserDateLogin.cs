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

        

            if(findlastFactor == null)
            {
                DateTime Today = DateTime.Now;
                var days = (Today - item.ActiveDate).TotalDays;
                if (days > item.Days)
                {
                    return false;
                }
                else
                {
                    return true;
                }
                
            }


            DateTime ActivedDateByAdmin = item.ActiveDate;
            if(ActivedDateByAdmin > findlastFactor.Date)
            {
                var _totalDays = (ActivedDateByAdmin - findlastFactor.Date).TotalDays;
                if (_totalDays > item.Days)
                {
                    return false;

                }
                else
                {
                    return true;
                }
            }
            if(ActivedDateByAdmin < findlastFactor.Date)
            {
                var totalDays = (findlastFactor.Date - ActivedDateByAdmin).TotalDays;
                if (totalDays > item.Days)
                {
                    return false;

                }
                else
                {
                    return true;
                }
            }
            return true;

        }
    }
}