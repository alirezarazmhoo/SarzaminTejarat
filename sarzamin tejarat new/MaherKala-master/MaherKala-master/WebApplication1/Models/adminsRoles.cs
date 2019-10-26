using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class adminsRoles
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string RoleDes { get; set; }

        public ICollection<AdminsInRoles> AdminsInRoles { get; set; }



    }
}