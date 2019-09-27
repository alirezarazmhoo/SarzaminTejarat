using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers.api.Marketer
{
    [Route("api/MarketerShowTicketsForAddSubSetCount")]
    public class MarketerShowTicketsForAddSubSetCountController : ApiController
    {
        DBContext db = new DBContext();
        #region ShowTickets
        [Route("api/MarketerShowTicketsForAddSubSetCount/GetTickets/{page}")]
        [HttpGet]
        public object GetTickets(int page)
        {
            var Items =  db.RateOfAddSubSets.Select(p => new { p.Id, p.Price, p.AddSubsetCounts }).AsQueryable();
            return   new {  Data = Items.OrderByDescending(x => x.Id).Skip(10 * (page - 1)).Take(10).ToList(), totalCount = Items.Count() };
        }
        #endregion
        #region ShowPricePointForAddSubSet
        [Route("api/MarketerShowTicketsForAddSubSetCount/ShowPricePointForAddSubSet")]
        [HttpGet]
        public async Task<object> ShowPricePointForAddSubSet()
        {
            var Item = db.pricePointForAddSubsets.Select(p => new { p.Id, p.MinimumPrice }).ToListAsync();
            return await Item;
        }

        #endregion

    }
}
