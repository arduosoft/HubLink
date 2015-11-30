using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Security;
using Wlog.Models;
using Wlog.Web.Code.Repository;

namespace Wlog.Web.Code.Authentication
{
    public class WLogRoleProvider:RoleProvider
    {

        private const string ADMIN ="ADMIN";
        private const string USER = "USER";


        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                return "WLog";
            }
            set
            {
                throw new NotImplementedException();
            }
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

        public override string[] GetRolesForUser(string username)
        {
            List<string> Roles = new List<string>();
            using (ISession session = WebApiApplication.CurrentSessionFactory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    UserEntry u = session.Query<UserEntry>().Where(x => x.Username == username).FirstOrDefault();
                    if (u != null && u.IsAdmin)
                        Roles.Add(ADMIN);
                }
            }
            Roles.Add(USER);
            return Roles.ToArray();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            if (roleName == USER)
                return true;
            if (roleName == ADMIN)
            {
                UserRepository repo = new UserRepository();
                UserEntry usr = repo.GetByUsername(username);
                if (usr == null || !usr.IsAdmin)
                    return false;
                return true;
            }
            return false;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            bool result = false;
            if (roleName == ADMIN || roleName == USER)
                result = true;
            return result;
        }
    }
}