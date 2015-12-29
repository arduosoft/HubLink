using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wlog.Web.Code.Classes;

namespace Wlog.Web.Models.User
{
    public class UserApps
    {
        public int Id { get; set; }
        public ApplicationEntity Application { get; set; }
        public RolesEntity Role { get; set; }

    }

    public class EditUser
    {

        public UserEntity DataUser { get; set; }
        public List<UserApps> Apps { get; set; }
        public IEnumerable<RolesEntity> RoleList
        { 
            get 
            {
                using (UnitOfWork uow = new UnitOfWork())
                {
                    List<RolesEntity> role= uow.Query<RolesEntity>().ToList();
                    role.Add(new RolesEntity { RoleName = "No Role" });
                    return role;//new SelectList(role, "Id", "RoleName");
                }
            }
        }

        public EditUser()
        {
            this.Apps = new List<UserApps>();
        }
    }
}