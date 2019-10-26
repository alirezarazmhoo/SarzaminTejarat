using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class CompanyAgent
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public long Mobile { get; set; }
        public long NationalCode { get; set; }
        public string Adress { get; set; }

        public string password { get; set; }

        public bool Status { get; set; }


    }
}