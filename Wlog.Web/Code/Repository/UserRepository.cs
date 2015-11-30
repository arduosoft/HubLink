using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wlog.Models;

namespace Wlog.Web.Code.Repository
{
    public class UserRepository
    {
        /// <summary>
        /// Get User by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserEntry GetById(int id)
        {
            UserEntry user;
            using (ISession session = WebApiApplication.CurrentSessionFactory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    user = session.Query<UserEntry>().Where(x => x.Id == id).FirstOrDefault();
                }
            }
            if (user == null) return null;
            return user;
        }

        /// <summary>
        /// Get User By Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public UserEntry GetByEmail(string email)
        {
            UserEntry user;
            using (ISession session = WebApiApplication.CurrentSessionFactory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    user = session.Query<UserEntry>().Where(x => x.Email == email).FirstOrDefault();
                }
            }
            if (user == null) return null;
            return user;
        }

        /// <summary>
        /// Get User by Username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public UserEntry GetByUsername(string username)
        {
            UserEntry user;
            using (ISession session = WebApiApplication.CurrentSessionFactory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    user = session.Query<UserEntry>().Where(x => x.Username == username).FirstOrDefault();
                }
            }
            if (user == null) return null;
            return user;
        }

        /// <summary>
        /// Update the User
        /// </summary>
        /// <param name="usr"></param>
        /// <returns></returns>
        public bool UpdateUser(UserEntry usr)
        {
            bool result = false;
            try
            {
                using (ISession session = WebApiApplication.CurrentSessionFactory.OpenSession())
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.SaveOrUpdate(usr);
                        transaction.Commit();
                    }
                }
            }
            catch (Exception ee)
            {
                result = false;
            }
            return result;
        }
    }
}