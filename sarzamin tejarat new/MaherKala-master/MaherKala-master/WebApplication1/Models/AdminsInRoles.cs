using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class AdminsInRoles
    {
        public int Id { get; set; }
        public int AdminUsersID { get; set; }
       public AdminUsers AdminUsers { get; set; }
        public int AdminsRolesID { get; set; }
        public adminsRoles AdminsRoles { get; set; }




    }
}