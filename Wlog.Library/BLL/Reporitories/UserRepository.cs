using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using Wlog.BLL.Entities;
using Wlog.Library.BLL.Classes;
using Wlog.Library.BLL.Interfaces;

namespace Wlog.Library.BLL.Reporitories
{
    public abstract class UserRepository : IRepository
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public abstract UserEntity GetById(Guid id);

        public abstract List<UserEntity> GetAll();

        public abstract IPagedList<UserEntity> SearchUsers(UserSearchSettings userSearchSettings);

        public UserEntity GetByEmail(string email)
        {
            IPagedList<UserEntity> users = SearchUsers(new UserSearchSettings() {
                Email=email
            });

            return GetFirstItem(users);

        }

        private UserEntity GetFirstItem(IPagedList<UserEntity> users)
        {
            if (users != null && users.Count > 0) return users.FirstOrDefault();
            return null;
        }

        public UserEntity GetByUsername(string username)
        {
            IPagedList<UserEntity> users = SearchUsers(new UserSearchSettings()
            {
                Username = username,
                UsernamePartialSearch=true
            });

            return GetFirstItem(users);
        }

        public abstract bool Save(UserEntity usr);

        public abstract bool Delete(UserEntity user);
    }
}
