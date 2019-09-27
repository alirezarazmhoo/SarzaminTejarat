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
    public class MarketerToturialController : ApiController
    {
        DBContext db = new DBContext();

        [Route("api/MarketerToturial/ShowTutorial")]
        [HttpGet]
        public async Task<object> ShowTutorials()
        {

            var items =await db.MarketerTutorials.
                Include("MarketerTutorialFiles").
                Select(p => new { p.Id, p.Description, files = p.MarketerTutorialFiles.Select(s =>
                new { s.ImageUrl, s.FileUrl }) }).ToListAsync();
        
            return    items;
        }




    }
}
