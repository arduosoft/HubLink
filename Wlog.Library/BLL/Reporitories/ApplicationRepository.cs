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
    public abstract class ApplicationRepository:IRepository
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public abstract ApplicationEntity GetById(Guid id);

        public abstract IPagedList<ApplicationEntity> Search(ApplicationSearchSettings searchSettings);

        public abstract void Delete(ApplicationEntity app);

        public List<ApplicationEntity> GetAppplicationsByUsername(string userName)
        {
            UserEntity user = RepositoryContext.Current.Users.GetByUsername(userName);
            List<Guid> Ids = GetAppplicationsIdsForUser( user);
            return RepositoryContext.Current.Applications.GetByIds(Ids);


        }

        public abstract List<ApplicationEntity> GetByIds(List<Guid> ids);

        public void Save(ApplicationEntity app)
        {
            throw new NotImplementedException();
        }

        public List<Guid> GetAppplicationsIdsByUsername(string userName)
        {
            UserEntity user = RepositoryContext.Current.Users.GetByUsername(userName);
            List<Guid> Ids = GetAppplicationsIdsForUser( user);
            return Ids;
        }


        public abstract List<Guid> GetAppplicationsIdsForUser(UserEntity user);

        public abstract List<ApplicationEntity> GetAppplicationForUser(UserEntity user);

        public abstract bool Save(UserEntity user);

        public abstract bool AssignRoleToUser(ApplicationEntity app, UserEntity user, RolesEntity role);

        public abstract ApplicationEntity GetByApplicationKey(string applicationKey);

        public abstract  void ResetUserRoles(UserEntity user,List<AppUserRoleEntity> newRoleList);
    }
}
