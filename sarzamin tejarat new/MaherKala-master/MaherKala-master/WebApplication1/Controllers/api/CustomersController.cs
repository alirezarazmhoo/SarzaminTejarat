using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using WebApplication1.Models;
using WebApplication1.Models.Errors;

namespace WebApplication1.Controllers.api
{
    public class CustomersController : ApiController
    {
        private DBContext db = new DBContext();

        // GET: api/Customers
        public IQueryable<Customer> GetCustomers()
        {
            return db.Customers;
        }

        [ResponseType(typeof(Customer))]
        [HttpPost]
        [Route("api/Customers/GetCustomerByMobile")]
        public async Task<IHttpActionResult> GetCustomerByMobile(GetCustomerByMobileModel searchModel)
        {  
            MarketerUser marketerUser =await db.MarketerUsers.Where(s => s.Api_Token == searchModel.api_Token).FirstOrDefaultAsync();
            if (marketerUser.Equals(null))
            {
                return new System.Web.Http.Results.ResponseMessageResult(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.MarketerNotFound)));
            }
            else
            {
                List<Customer> customers =await db.Customers.Where(s => s.MarketerUserId == marketerUser.Id && s.Mobile.Contains(searchModel.mobile)).ToListAsync();
                return Ok(customers);
            }
        }

        // GET: api/Customers/5
        [ResponseType(typeof(Customer))]
        public async Task<IHttpActionResult> GetCustomer(int id)
        {
            List<Customer> customers = new List<Customer>();
            if(id == 0)
            {
                return new System.Web.Http.Results.ResponseMessageResult(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.EmptyMarketerUserId)));

            }
            if(!db.MarketerUsers.Where(s=>s.Id == id).Any())
            {
                return new System.Web.Http.Results.ResponseMessageResult(
                   Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.MarketerNotFound)));
            }
            customers = await db.Customers.Where(s => s.MarketerUserId == id).ToListAsync();
            return Ok( customers);
        }

        // PUT: api/Customers/5
        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/Customers/EditCustomer/{id}")]
        public async Task<IHttpActionResult> PutCustomer(int id, Customer customer)
        {
            string data, data2, unique, unique2, imageUrl, path, path2, imageUrl2 = string.Empty;
            List<Customer> _CustomersItems = new List<Customer>();
            if (customer.Lat == 0 || customer.Lng == 0 || customer.MarketerUserId == 0)
            {
                return new System.Web.Http.Results.ResponseMessageResult(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.BadData)));
            }
            Convert.ToDouble(customer.Lat);
            Convert.ToDouble(customer.Lng);
            Convert.ToInt32(customer.MarketerUserId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != customer.Id)
            {
                return new System.Web.Http.Results.ResponseMessageResult(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.ConflitId)));
            }
            Customer _customerItem = await db.Customers.Where(s=>s.Id==customer.Id).FirstOrDefaultAsync();
            if (_customerItem == null)
            {
              return new System.Web.Http.Results.ResponseMessageResult(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.UserNotFound)));
            }
            _CustomersItems=await db.Customers.Where(s => s.Mobile == customer.Mobile).ToListAsync();
            if(_CustomersItems.Count != 0)
            {
                foreach (var item in _CustomersItems)
                {
                  if(item.Id != customer.Id)
                    {
                        return new System.Web.Http.Results.ResponseMessageResult(
                            Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.MobileError)));
                    }

                }
            }
            if(_CustomersItems.Count !=0)
            _CustomersItems.Clear();
            _CustomersItems = await db.Customers.Where(s => s.IDCardNumber == customer.IDCardNumber).ToListAsync();
            if (_CustomersItems.Count != 0)
            {
                foreach (var item in _CustomersItems)
                {
                    if (item.Id != customer.Id)
                    {
                        return new System.Web.Http.Results.ResponseMessageResult(
                            Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.IDCardNumberError)));
                    }

                }
            }
            if (!string.IsNullOrEmpty(_customerItem.IDCardPicture))
            {
                File.Delete(HttpContext.Current.Server.MapPath("~/" + _customerItem.IDCardPicture));
            }
            if (!string.IsNullOrEmpty(_customerItem.PersonalPicture))
            {
                File.Delete(HttpContext.Current.Server.MapPath("~/" + _customerItem.PersonalPicture));
            }
            data = customer.IDCardPicture.Replace("data:image/png;base64,", "");
            data = data.Replace("data:image/jpeg;base64,", "");
            data = data.Replace(" ", "+");
            byte[] imgBytes = Convert.FromBase64String(data);
            if (CheckImageSize(imgBytes.Length) == 1)
            {
                return new System.Web.Http.Results.ResponseMessageResult(
                 Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, new HttpError(ErrorsText.MinPictureError)));
            }
            if (CheckImageSize(imgBytes.Length) == 2)
            {
                return new System.Web.Http.Results.ResponseMessageResult(
          Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, new HttpError(ErrorsText.MaxPictureError)));
            }
            unique = BulidUrl();
            imageUrl = "/Upload/Customer/IDCardPicture/" + unique;
            path = HttpContext.Current.Server.MapPath(imageUrl);
            data2 = customer.PersonalPicture.Replace("data:image/png;base64,", "");
            data2 = customer.PersonalPicture.Replace("data:image/jpeg;base64,", "");
            data2 = data2.Replace(" ", "+");
            byte[] imgBytes2 = Convert.FromBase64String(data2);
            if (CheckImageSize(imgBytes2.Length) == 1)
            {
                return new System.Web.Http.Results.ResponseMessageResult(
                    Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, new HttpError(ErrorsText.MinPictureError)));
            }
            if (CheckImageSize(imgBytes2.Length) == 2)
            {
                return new System.Web.Http.Results.ResponseMessageResult(
                Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, new HttpError(ErrorsText.MaxPictureError)));
            }
            unique2 = BulidUrl();
            imageUrl2 = "/Upload/Customer/PersonalPicture/" + unique2;
            path2 = HttpContext.Current.Server.MapPath(imageUrl2);
            _customerItem.AccountNumber = customer.AccountNumber;
            _customerItem.Address = customer.Address;
            _customerItem.CardAccountNumber = customer.CardAccountNumber;
            _customerItem.CertificateNumber = customer.CertificateNumber;
            _customerItem.Description = customer.Description;
            _customerItem.FullName = customer.FullName;
            _customerItem.IBNA = customer.IBNA;
            _customerItem.IDCardNumber = customer.IDCardNumber;
            _customerItem.IDCardPhotoAddress = customer.IDCardPhotoAddress;
            _customerItem.IDCardPicture = imageUrl;
            _customerItem.IsMarrid = customer.IsMarrid;
            _customerItem.Lat = customer.Lat;
            _customerItem.Lng = customer.Lng;
            _customerItem.Mobile = customer.Mobile;
            _customerItem.PersonalPicture = imageUrl2;
            _customerItem.Phone = customer.Phone;
            _customerItem.PostalCode = customer.PostalCode;
            try
            {
                using (var imageFile = new FileStream(path, FileMode.Create))
                {
                    imageFile.Write(imgBytes, 0, imgBytes.Length);
                    imageFile.Flush();
                }
                using (var imageFile = new FileStream(path2, FileMode.Create))
                {
                    imageFile.Write(imgBytes2, 0, imgBytes2.Length);
                    imageFile.Flush();
                }
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return new System.Web.Http.Results.ResponseMessageResult(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ex.Message)));
                }
            }

            return StatusCode(HttpStatusCode.OK);
        }

        // POST: api/Customers
        [ResponseType(typeof(Customer))]
        public async Task<IHttpActionResult> PostCustomer(Customer customer)
        {
            string data, data2, unique, unique2, imageUrl,path,path2, imageUrl2 = string.Empty;
            if (customer.Lat == 0 || customer.Lng == 0 || customer.MarketerUserId == 0)
            {
                return new System.Web.Http.Results.ResponseMessageResult(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.BadData)));
            }
            Convert.ToDouble(customer.Lat);
            Convert.ToDouble(customer.Lng);
            Convert.ToInt32(customer.MarketerUserId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!db.MarketerUsers.Where(s=>s.Id == customer.MarketerUserId).Any())
            {
                return new System.Web.Http.Results.ResponseMessageResult(
            Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.MarketerNotFound)));

            }
            if(db.Customers.Where(s=>s.PostalCode == customer.PostalCode && s.Mobile == customer.Mobile).Any())
            {
                return new System.Web.Http.Results.ResponseMessageResult(
          Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.RepeatError)));
            }
            if (db.Customers.Any(p => p.IDCardNumber == customer.IDCardNumber))
            {
                return new System.Web.Http.Results.ResponseMessageResult(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.IDCardNumberError)));

            }
            if (db.Customers.Any(p => p.Mobile == customer.Mobile))
            {
                return new System.Web.Http.Results.ResponseMessageResult(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.MobileError)));
            }
            data = customer.IDCardPicture.Replace("data:image/png;base64,", "");
            data = data.Replace("data:image/jpeg;base64,", "");
            data = data.Replace(" ", "+");
            byte[] imgBytes = Convert.FromBase64String(data);
            if (CheckImageSize(imgBytes.Length) == 1)
            {
                return new System.Web.Http.Results.ResponseMessageResult(
                 Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, new HttpError(ErrorsText.MinPictureError)));
           
            }
            if (CheckImageSize(imgBytes.Length) == 2)
            {
                return new System.Web.Http.Results.ResponseMessageResult(
          Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, new HttpError(ErrorsText.MaxPictureError)));
            }
            unique = BulidUrl();
            imageUrl = "/Upload/Customer/IDCardPicture/" + unique;
            path = HttpContext.Current.Server.MapPath(imageUrl);
            customer.IDCardPicture = imageUrl;
            data2 = customer.PersonalPicture.Replace("data:image/png;base64,", "");
            data2 = customer.PersonalPicture.Replace("data:image/jpeg;base64,", "");
            data2 = data2.Replace(" ", "+");
            byte[] imgBytes2 = Convert.FromBase64String(data2);
            if (CheckImageSize(imgBytes2.Length) == 1)
            {
                return new System.Web.Http.Results.ResponseMessageResult(
                    Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, new HttpError(ErrorsText.MinPictureError)));

            }
            if (CheckImageSize(imgBytes2.Length) == 2)
            {
                return new System.Web.Http.Results.ResponseMessageResult(
                Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, new HttpError(ErrorsText.MaxPictureError)));
            }
            unique2 =BulidUrl();
            imageUrl2 = "/Upload/Customer/PersonalPicture/" + unique2;
            path2 = HttpContext.Current.Server.MapPath(imageUrl2);
            customer.PersonalPicture = imageUrl2;
            customer.CreatedDate = DateTime.Now;

            try
            {
        
            db.Customers.Add(customer);
                using (var imageFile = new FileStream(path, FileMode.Create))
                {
                    imageFile.Write(imgBytes, 0, imgBytes.Length);
                    imageFile.Flush();
                }
                using (var imageFile = new FileStream(path2, FileMode.Create))
                {
                    imageFile.Write(imgBytes2, 0, imgBytes2.Length);
                    imageFile.Flush();
                }
                await db.SaveChangesAsync();
                return CreatedAtRoute("DefaultApi", new { id = customer.Id }, customer);

            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
                var fullErrorMessage = string.Join(" - ", errorMessages);
                return new System.Web.Http.Results.ResponseMessageResult(
                 Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(fullErrorMessage)));
            }
       
        }

        // DELETE: api/Customers/5
        [ResponseType(typeof(Customer))]
        [HttpPost]
        [Route("api/Customers/DeleteCustomer/{id}")]
        public async Task<IHttpActionResult> DeleteCustomer(int id)
        {       
            Customer customer = await db.Customers.FindAsync(id);
            if (customer == null)
            {
                return new System.Web.Http.Results.ResponseMessageResult(
                  Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(ErrorsText.UserNotFound)));
            }
            if (!string.IsNullOrEmpty(customer.IDCardPicture))
            {
               File.Delete(HttpContext.Current.Server.MapPath("~/"+ customer.IDCardPicture));
            }
            if (!string.IsNullOrEmpty(customer.PersonalPicture))
            {
                File.Delete(HttpContext.Current.Server.MapPath("~/"+ customer.PersonalPicture));
            }
            db.Customers.Remove(customer);
            await db.SaveChangesAsync();

            return Ok(customer);
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        private bool CustomerExists(int id)
        {
            return db.Customers.Count(e => e.Id == id) > 0;
        }
        private byte CheckImageSize(int ImageLenght)
        {
            if (ImageLenght < 10000)
                return 1;
            else if (ImageLenght > 5242880)
                return 2;
            else
                return 0;
        }
        private string BulidUrl()
        {
            return Guid.NewGuid().ToString().Replace('-', '0') + "." + "jpg";
        }


        public class GetCustomerByMobileModel
        {
            public string mobile { get; set; }
            public string api_Token { get; set; }
        }
    }
}