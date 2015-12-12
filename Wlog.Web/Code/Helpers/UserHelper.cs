using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Wlog.Models;
using Wlog.Web.Code.Classes;

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
                int userId=uow.Query<UserEntity>().Where(p => p.Username == userName).Select(p =>  p.Id ).First();
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

        public static string EncodePassword(string password)
        {
            using (SHA1CryptoServiceProvider Sha = new SHA1CryptoServiceProvider())
            {
                return Convert.ToBase64String(Sha.ComputeHash(Encoding.ASCII.GetBytes(password)));
            }
        }
    }
}