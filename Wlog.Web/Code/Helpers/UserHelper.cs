using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Wlog.Web.Code.Authentication;
using Wlog.Web.Code.Classes;
using NHibernate.Linq;
using PagedList;
using Wlog.Web.Models.User;

namespace Wlog.Web.Code.Helpers
{
    public class UserHelper
    {
        /// <summary>
        /// Get User by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static UserEntity GetById(int id)
        {
            UserEntity user;
            using (UnitOfWork uow = new UnitOfWork())
            {
                user = uow.Query<UserEntity>().Where(x => x.Id == id).FirstOrDefault();
            }
            if (user == null) return null;
            return user;
        }

        /// <summary>
        /// Get All User
        /// </summary>
        /// <returns></returns>
        public static List<UserEntity> GetAll()
        {
            List<UserEntity> result = new List<UserEntity>();
            using (UnitOfWork uow = new UnitOfWork())
            {
                result = uow.Query<UserEntity>().ToList();
            }

            return result;
        }

        public static IPagedList<UserData> FilterUserList(string serchFilter, int pagenumber, int pagesize)
        {
            List<UserData> data = new List<UserData>();
            using (UnitOfWork uow = new UnitOfWork())
            {
                List<UserEntity> entity;
                if (string.IsNullOrEmpty(serchFilter))
                {
                    entity = uow.Query<UserEntity>().OrderBy(x => x.Username).ToList();
                }
                else
                {
                    entity = uow.Query<UserEntity>().Where(x => x.Username.Contains(serchFilter)).OrderBy(x => x.Username).ToList();
                }

                foreach (UserEntity e in entity)
                {
                    data.Add(new UserData
                    {
                        Id = e.Id,
                        Username = e.Username,
                        Email = e.Email,
                        IsAdmin = e.IsAdmin,
                        LastLoginDate = e.LastLoginDate,
                        CreationDate = e.CreationDate,
                        IsOnLine = e.IsOnLine
                    });
                }
            }

            return new PagedList<UserData>(data, pagenumber, pagesize);
        }

        /// <summary>
        /// Get User By Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static UserEntity GetByEmail(string email)
        {
            UserEntity user;
            using (UnitOfWork uow = new UnitOfWork())
            {
                user = uow.Query<UserEntity>().Where(x => x.Email == email).FirstOrDefault();

            }
            if (user == null) return null;
            return user;
        }

        public static List<ApplicationEntity> GetAppsForUser(string userName)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                int userId = uow.Query<UserEntity>().Where(p => p.Username == userName).Select(p => p.Id).First();
                List<int> appIds = uow.Query<AppUserRoleEntity>().Where(p => p.User.Id == userId).Select(p => p.Application.IdApplication).ToList();
                return uow.Query<ApplicationEntity>().Where(p => appIds.Contains(p.IdApplication)).ToList();

            }

        }

        public static List<int> GetAppsIdsForUser(string userName)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                int userId = uow.Query<UserEntity>().Where(p => p.Username == userName).Select(p => p.Id).First();
                List<int> appIds = uow.Query<AppUserRoleEntity>().Where(p => p.User.Id == userId).Select(p => p.Application.IdApplication).ToList();
                return appIds;

            }
        }

        /// <summary>
        /// Get User by Username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static UserEntity GetByUsername(string username)
        {
            UserEntity user;
            using (UnitOfWork uow = new UnitOfWork())
            {
                user = uow.Query<UserEntity>().Where(x => x.Username == username).FirstOrDefault();

            }
            if (user == null) return null;
            return user;
        }

        /// <summary>
        /// Update the User
        /// </summary>
        /// <param name="usr"></param>
        /// <returns></returns>
        public static bool UpdateUser(UserEntity usr)
        {
            bool result = false;

            using (UnitOfWork uow = new UnitOfWork())
            {
                try
                {
                    uow.SaveOrUpdate(usr);
                    uow.Commit();
                }
                catch (Exception ee)
                {

                    result = false;
                }
            }

            return result;
        }


        public static bool DeleteById(int Id)
        {
            bool result = true;
            try
            {
                using (UnitOfWork uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    List<AppUserRoleEntity> entity = uow.Query<AppUserRoleEntity>().Where(x => x.User.Id == Id).ToList();
                    foreach (AppUserRoleEntity e in entity)
                    {
                        uow.Delete(e);
                    }

                    UserEntity user = uow.Query<UserEntity>().Where(x => x.Id == Id).First();

                    uow.Delete(user);

                    uow.Commit();
                }
            }
            catch (Exception e)
            {
                result = false;
            }

            return result;
        }
        public static List<UserApps> GetApp(int id)
        {
            List<UserApps> result = new List<UserApps>();
            using (UnitOfWork uow = new UnitOfWork())
            {
                List<AppUserRoleEntity> entity;
                if (UserProfileContext.Current.User.IsAdmin)
                {
                    entity = uow.Query<AppUserRoleEntity>().Where(x => x.User.Id == id).Fetch(x => x.Application).Fetch(x => x.Role).ToList();
                }
                else
                {
                    List<int> AppContext = uow.Query<AppUserRoleEntity>().Where(x => x.User.Id == UserProfileContext.Current.User.Id && x.Role.RoleName == Constants.Roles.Admin).Select(x => x.Application.IdApplication).ToList();
                    entity = uow.Query<AppUserRoleEntity>().Where(x => x.User.Id == id && AppContext.Contains(x.Application.IdApplication)).Fetch(x => x.Application).Fetch(x => x.Role).ToList();
                }
                List<ApplicationEntity> App;
                if (UserProfileContext.Current.User.IsAdmin)
                {
                    App = uow.Query<ApplicationEntity>().Where(x => x.IsActive == true).ToList();
                }
                else
                {
                    App = uow.Query<AppUserRoleEntity>().Where(x => x.User.Id == UserProfileContext.Current.User.Id && (x.Role.RoleName == Constants.Roles.Admin || x.Role.RoleName == Constants.Roles.Write)).Select(x => x.Application).ToList();

                }

                foreach (ApplicationEntity ae in App)
                {
                    if (entity.Where(x => x.Application.IdApplication == ae.IdApplication) != null && entity.Where(x => x.Application.IdApplication == ae.IdApplication).Select(x => x.Role).Count() != 0)
                    {
                        RolesEntity r = entity.Where(x => x.Application.IdApplication == ae.IdApplication).Select(x => x.Role).FirstOrDefault();
                        result.Add(new UserApps
                        {
                            ApplicationName = ae.ApplicationName,
                             IdApplication=ae.IdApplication,
                            RoleId=r.Id,
                           RoleName=r.RoleName
                        });
                    }
                    else
                    {
                        result.Add(new UserApps
                        {
                            ApplicationName = ae.ApplicationName,
                            IdApplication = ae.IdApplication,
                            RoleId = 0,
                            RoleName = "No Role"
                        });
                    }
                }
            }
            return result;
        }

        public static string EncodePassword(string password)
        {
            using (SHA1CryptoServiceProvider Sha = new SHA1CryptoServiceProvider())
            {
                return Convert.ToBase64String(Sha.ComputeHash(Encoding.ASCII.GetBytes(password)));
            }
        }
    }
}