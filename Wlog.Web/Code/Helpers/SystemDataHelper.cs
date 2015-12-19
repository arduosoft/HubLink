using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wlog.Web.Code.Classes;
using NHibernate.Linq;
using Wlog.Web.Code.Authentication;

namespace Wlog.Web.Code.Helpers
{
    public static class SystemDataHelper
    {
        public static void EnsureSampleData()
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                int users = uow.Query<UserEntity>().Count();
                if (users == 0)
                {
                    ApplicationEntity app = new ApplicationEntity();
                    app.ApplicationName = "SampleApp";
                    app.IsActive = true;
                    app.StartDate = DateTime.Now;
                    app.EndDate = DateTime.Now.AddYears(1);
                    app.PublicKey = new Guid("{8C075ED0-45A7-495A-8E09-3A98FD6E8248}");
                    uow.SaveOrUpdate(app);

                    // Insert admin user
                    UserEntity user = new UserEntity();                    
                    user.CreationDate = DateTime.Now;
                    user.Email = "mymail@weblog-logger.it";
                    user.IsAdmin = true;
                    user.IsApproved = true;
                    user.IsLockedOut = false;
                    user.Password = UserHelper.EncodePassword("12345678");
                    user.Username = "admin";

                    uow.SaveOrUpdate(user);

                    AppUserRoleEntity link = new AppUserRoleEntity();
                    link.Application = app;
                    link.Role = uow.Query<RolesEntity>().Where(p => p.RoleName == Constants.Roles.Admin).FirstOrDefault();
                    link.User = user;

                    uow.SaveOrUpdate(link);

                    uow.Commit();                
                }

                
            }
        }

        public static void InsertRoleIfNotExists(string Rolename)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                if (!uow.Query<RolesEntity>().Any(p => p.RoleName == Rolename))
                {
                    uow.SaveOrUpdate(new RolesEntity()
                    {
                        RoleName= Rolename
                    });
                    uow.Commit();
                }
            }
        }

        public static void InsertRoles()
        {
            InsertRoleIfNotExists(Constants.Roles.Admin);
            InsertRoleIfNotExists(Constants.Roles.Read);
            InsertRoleIfNotExists(Constants.Roles.Write);
        }





    }
}