using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using Wlog.BLL.Entities;
using Wlog.DAL.NHibernate.Helpers;
using Wlog.Library.BLL.Classes;

namespace Wlog.Library.BLL.Reporitories.Implementations.Nhibernate
{
    public class NHibernateUserRepository : UserRepository
    {
        public override bool Delete(UserEntity user)
        {
            bool result = true;
            try
            {
                using (UnitOfWork uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    List<AppUserRoleEntity> entity = uow.Query<AppUserRoleEntity>().Where(x => x.UserId == user.Id).ToList();
                    foreach (AppUserRoleEntity e in entity)
                    {
                        uow.Delete(e);
                    }

                  

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

        public override List<UserEntity> GetAll()
        {
            List<UserEntity> result = new List<UserEntity>();
            using (UnitOfWork uow = new UnitOfWork())
            {
                result = uow.Query<UserEntity>().ToList();
            }

            return result;
        }

        public override UserEntity GetById(Guid id)
        {
            UserEntity user;
            using (UnitOfWork uow = new UnitOfWork())
            {
                user = uow.Query<UserEntity>().Where(x => x.Id == id).FirstOrDefault();
            }
            if (user == null) return null;
            return user;
        }

        public override bool Save(UserEntity usr)
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

        public override IPagedList<UserEntity> SearchUsers(UserSearchSettings userSearchSettings)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                List<UserEntity> entity;
                if (string.IsNullOrEmpty(userSearchSettings.Username))
                {
                    entity = uow.Query<UserEntity>().OrderBy(x => x.Username).ToList();
                }
                else
                {
                    entity = uow.Query<UserEntity>().Where(x => x.Username.Contains(userSearchSettings.Username)).OrderBy(x => x.Username).ToList();
                }

                return new
                      StaticPagedList<UserEntity>(entity, userSearchSettings.PageNumber, userSearchSettings.PageSize, 1000);
            }
        }
    }
}
