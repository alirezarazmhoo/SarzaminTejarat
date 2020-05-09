using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using WebApplication1.Models;

namespace WebApplication1.Controllers.api
{
    public class PocketBookController : ApiController
    {
        DBContext db = new DBContext();
        [HttpPost]
        [Route("api/PocketBook/Register")]
        public object Register()
        {
             var Token = HttpContext.Current.Request.Form["Api_Token"];
            var user = db.MarketerUsers.Where(p => p.Api_Token == Token).FirstOrDefault();
            string FullName = HttpContext.Current.Request.Form["FullName"];
            string Mobile = HttpContext.Current.Request.Form["Mobile"];
            string Description = HttpContext.Current.Request.Form["Description"];
            PocketBook pocketBook = new PocketBook();
            pocketBook.FullName = FullName;
            pocketBook.MarketerUserId= user.Id;
            pocketBook.Mobile = Mobile;
            pocketBook.Description = Description;
            db.PocketBooks.Add(pocketBook);
            try
            {
                db.SaveChanges();
                return new { StatusCode = 0, pocketBook };
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                  .SelectMany(x => x.ValidationErrors)
                  .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join(" - ", errorMessages);
                return new { StatusCode = 1, Message = fullErrorMessage };
            }

        }
         [HttpGet]
        public object GetPocketBook(string ApiToken)
        {
            var user = db.MarketerUsers.Where(p => p.Api_Token == ApiToken).FirstOrDefault();
            var data = db.PocketBooks.Where(x => x.MarketerUserId == user.Id).ToList();
            return new {
                Data =data,
                Status=0
            };
        }
        #region Remove
        [HttpPost]
        [Route("api/PocketBook/DeletePocketBook")]
        [ResponseType(typeof(PocketBook))]
        public async Task<object> DeletePocketBook()
        {
            int id =Convert.ToInt32( HttpContext.Current.Request.Form["Id"]);
            PocketBook pocketBook = await db.PocketBooks.FindAsync(id);
            if (pocketBook == null)
            {
                return new
                {
                    StatusCode = 1
                };

            }

            db.PocketBooks.Remove(pocketBook);
            await db.SaveChangesAsync();

            return new
            {
                StatusCode = 0
            };
        }
        #endregion

        #region Edit
          [ResponseType(typeof(void))]
        [Route("api/PocketBook/EditPocketBook")]
        public async Task<object> EditPocketBook()
        {
            int id = Convert.ToInt32(HttpContext.Current.Request.Form["Id"]);
           
            var Item = db.PocketBooks.Where(p => p.Id == id).FirstOrDefault();
            if (!PocketBookExists(id))
            {
                return new
                {
                    StatusCode = 1
                };
            }
            if (HttpContext.Current.Request.Form["FullName"] != null)
            {
                Item.FullName = HttpContext.Current.Request.Form["FullName"];
            }
            if (HttpContext.Current.Request.Form["Mobile"] != null)
            {
                Item.Mobile = HttpContext.Current.Request.Form["Mobile"];
            }
            if (HttpContext.Current.Request.Form["Description"] != null)
            {
                Item.Description = HttpContext.Current.Request.Form["Description"];
            }


            try
            {
                await db.SaveChangesAsync();
                return new
                {
                    StatusCode = 0
                };
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PocketBookExists(id))
                {
                    return new
                    {
                        StatusCode = 1
                    };
                }
                else
                {
                    throw;
                }
            }

            //return StatusCode(HttpStatusCode.Created);
        }
        #endregion
        private bool PocketBookExists(int id)
        {
            return db.PocketBooks.Count(e => e.Id == id) > 0;
        }

    }
}
