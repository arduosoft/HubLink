using System;
using System.Collections.Generic;
using Wlog.BLL.Entities;
using Wlog.BLL.Classes;
using Wlog.Library.BLL.Reporitories;

namespace Wlog.Web.Code.Helpers
{
    public static class SystemDataHelper
    {
        public static void EnsureSampleData()
        {

            List<UserEntity> userList = RepositoryContext.Current.Users.GetAll();


            if (userList == null || userList.Count == 0)
            {
                ApplicationEntity app = new ApplicationEntity();
                app.ApplicationName = "SampleApp";
                app.IsActive = true;
                app.StartDate = DateTime.Now;
                app.EndDate = DateTime.Now.AddYears(1);
                app.PublicKey = new Guid("{8C075ED0-45A7-495A-8E09-3A98FD6E8248}");
                RepositoryContext.Current.Applications.Save(app);

                // Insert admin user
                UserEntity user = new UserEntity();
                user.CreationDate = DateTime.Now;
                user.Email = "mymail@weblog-logger.it";
                user.IsAdmin = true;
                user.IsApproved = true;
                user.IsLockedOut = false;
                user.Password = UserHelper.EncodePassword("12345678");
                user.Username = "admin";

                RepositoryContext.Current.Users.Save(user);

                RolesEntity role = RepositoryContext.Current.Roles.GetRoleByName(Constants.Roles.Admin);
                RepositoryContext.Current.Applications.AssignRoleToUser(app, user, role);

            }



        }

        public static void InsertRoleIfNotExists(string Rolename)
        {

            RolesEntity role= RepositoryContext.Current.Roles.GetRoleByName(Rolename);
            if (role == null)
            {
                RepositoryContext.Current.Roles.Save(new RolesEntity()
                {
                    Id=Guid.NewGuid(),
                    RoleName=Rolename
                });
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