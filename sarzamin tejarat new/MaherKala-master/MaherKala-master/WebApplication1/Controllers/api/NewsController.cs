using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers.api
{
    public class NewsController : ApiController
    {
        DBContext db = new DBContext();
        [HttpPost]
        [Route("api/News/Index")]
        public object Index()
        {
            var data = db.News.Select(p => new { p.Id, p.Title, p.Desc }).OrderByDescending(p => p.Id);
            var paged = new PagedItem<object>(data,"/api/News/Index");
            return paged;
        }
        [HttpGet]
        [Route("api/News/Detail")]
        public HttpResponseMessage Detail(int Id)
        {
            var data = db.News.Where(p => p.Id == Id).Select(p => new { p.Text }).FirstOrDefault().Text;
            var res = Request.CreateResponse(HttpStatusCode.OK);
            res.Content = new StringContent(data, Encoding.UTF8, "text/html");
            return res;

        }

    }
}
