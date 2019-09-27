using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers.api
{
    public class CategoryController : ApiController
    {
        DBContext db = new DBContext();
        [HttpGet]
        public object GetCategory(int parent_id)
        {
            Category c = new Models.Category();

            IEnumerable<Category> result = null;

            if (parent_id == 0)
            {
                result = db.Categories.Where(p => p.Parent == null).ToList();

            }
            else
            {
                result = db.Categories.Where(p => p.Parent.Id == parent_id).ToList();
            }
            return new {
                Data =result,
                Message=0
            };
        }
        
    }
}
