using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Areas.Admin.Utility
{
    public class Utility
    {
		
		bool NeedCheck = false;
		int Count = 0;
        DBContext db = new DBContext();
        public int CheckMarketerPlan(int UserId)
        {
            DateTime NowDateTime = DateTime.Now;
            var thisYear = NowDateTime.Year;
            var thisMonth = NowDateTime.Month;
			double TotalSumPrice = 0;
			var TotaFactors = db.MarketerFactorItem.Where(p =>
            p.MarketerFactor.MarketerUser.Id 
			== UserId && p.MarketerFactor.Date.Month
            == thisMonth && p.MarketerFactor.Date.Year == 
			thisYear ).ToList();
			if (TotaFactors.Count != 0)
            {
                foreach (var item in TotaFactors)
                {
                    TotalSumPrice += (item.UnitPrice) * (item.Qty);
                }	
				//پیدا کردن کاربر
                var MarketerUser = db.MarketerUsers.Where(s => s.Id == UserId).FirstOrDefault();
				//پیدا کردن پلن کاربر
                var findedPlan = db.Plannns.Where(p => p.Id == MarketerUser.PlannnID).FirstOrDefault();
                var MarketerUserPlanType = findedPlan.PlanTypeID;
                var MarketerUserPlanLevel = findedPlan.Level;
				//پیدا کردن پلن هایی که از سطح فعلی پلن کاربر بیشتر هستند
                var Plans = db.Plannns.Where(p => p.Level > MarketerUserPlanLevel && p.PlanType.Id == MarketerUserPlanType).ToList();
				if (findedPlan.PlanTypeID == 1)
				{
					var NextPlansList = db.Plannns.Where(s => s.PlanTypeID == 2).ToList();
					foreach (var item in NextPlansList)
					{
						if (TotalSumPrice >= item.Price)
						{
							MarketerUser.PlannnID = item.Id;
							Count++;
						}				
						else
						{
							if (Count == 0)
							{
								NeedCheck = true;
								break;
							}
						}
					}
				}
				if(NeedCheck || findedPlan.PlanTypeID ==2)
				{
					foreach (var item in Plans)
					{
						if (TotalSumPrice >= item.Price)
						{
							MarketerUser.PlannnID = item.Id;
						}
					}
				}              
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
						_UserPoints.SavedDate = DateTime.Now;
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