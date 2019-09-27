using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebApplication1.Models;

namespace WebApplication1.MyRoleProvider
{
    public class RoleProviderManager : RoleProvider
    {
        DBContext db = new DBContext();

        public override bool IsUserInRole(string username, string roleName)
        {
            return db.Users.Include("Role").Any(p => p.Email == username && p.Role.RoleNameEn== roleName);
        }
        public override string[] GetRolesForUser(string username)
        {
            var role = db.Users.Include("Role").Where(p => p.Email == username).Select(p => new { p.Role.RoleNameEn });
            return new[] { role.FirstOrDefault().RoleNameEn };
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

      

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}