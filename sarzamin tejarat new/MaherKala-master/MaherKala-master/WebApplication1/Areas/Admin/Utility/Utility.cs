using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Areas.Admin.Utility
{
    public   class Utility
    {
        private static double TotalSumPrice = 0;
      
        DBContext db = new DBContext();
        public int CheckMarketerPlan(int UserId,int factorId)
        {

            DateTime NowDateTime = DateTime.Now;
            var thisYear = NowDateTime.Year;
            var thisMonth = NowDateTime.Month;
            var TotaFactors = db.MarketerFactorItem.Where(p =>
            p.MarketerFactor.MarketerUser.Id == UserId && p.MarketerFactor.Date.Month
            == thisMonth && p.MarketerFactor.Date.Year == thisYear && p.MarketerFactor.Id == factorId).ToList();

            if (TotaFactors.Count != 0)
            {


                foreach (var item in TotaFactors)
                {
                    TotalSumPrice += (item.UnitPrice) * (item.Qty);
                }
                var MarketerUser = db.MarketerUsers.Where(s => s.Id == UserId).FirstOrDefault();
                var findedPlan = db.Plannns.Where(p => p.Id == MarketerUser.PlannnID).FirstOrDefault();
                var MarketerUserPlanType = findedPlan.PlanTypeID;

                var MarketerUserPlanLevel = findedPlan.Level;

                var Plans = db.Plannns.Where(p => p.Level > MarketerUserPlanLevel && p.PlanType.Id == MarketerUserPlanType).ToList();
                var CurrrentPlan = db.Plannns.Where(d => d.PlanType.Id == MarketerUserPlanType).FirstOrDefault();
                var finalPlan = db.Plannns.Where(s => s.PlanType.Id == MarketerUserPlanType).ToList();
                var max = 0;
                long PriceFinalPlan = 0;
                foreach (var _item in finalPlan)
                {

                    if(_item.Level > max)
                    {
                        max = _item.Level;
                        PriceFinalPlan = _item.Price;
                    }

                }
                var NextPlan = db.PlanTypes.Where(p => p.Id > MarketerUserPlanType).FirstOrDefault();

                foreach (var item in Plans)
                {
                    if (TotalSumPrice >= item.Price) 
                    {
                        MarketerUser.PlannnID = item.Id;

                    }
                    if (MarketerUserPlanLevel == max  && TotalSumPrice > PriceFinalPlan)
                    {
                        MarketerUser.PlannnID = NextPlan.Id;
                    }


                }

                //return  MarketerUser.PlannnID;


                return Convert.ToInt32(MarketerUser.PlannnID);





            }
            return 0;




        }

        public void AddMarketerPoint(int UserId, double PointSum)
        {
            var MarketerUser = db.MarketerUsers.Where(s => s.Id == UserId).FirstOrDefault();
            MarketerUserPoints marketerUserPoints = new MarketerUserPoints();
            var _UserPoints = db.MarketerUserPoints.Where(p => p.MarketerUserId == UserId).FirstOrDefault();
            var TodayDate = DateTime.Now;
            var month = TodayDate.Month;
            var Year = TodayDate.Year;

            if (MarketerUser != null)
            {
             

                if (_UserPoints == null)
                {
                    marketerUserPoints.MarketerUserId = MarketerUser.Id;
                    marketerUserPoints.SavedDate = DateTime.Now;
                    marketerUserPoints.UserPoits = PointSum;

                    db.MarketerUserPoints.Add(marketerUserPoints);

                }
                if (_UserPoints != null)
                {
                    if (_UserPoints.SavedDate.Year == Year && _UserPoints.SavedDate.Month == month)
                    {

                        _UserPoints.UserPoits += PointSum;
                    }
                    else
                    {
                        _UserPoints.UserPoits = PointSum;
                    }
                }

                try
                {
                    db.SaveChanges();

                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

            }

        }
    }

}