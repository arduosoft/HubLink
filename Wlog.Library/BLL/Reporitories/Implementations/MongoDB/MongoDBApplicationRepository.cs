using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using Wlog.BLL.Entities;
using Wlog.Library.BLL.Classes;

namespace Wlog.Library.BLL.Reporitories.Implementations.MongoDB
{
    public class MongoDBApplicationRepository : ApplicationRepository
    {
        public override bool AssignRoleToUser(ApplicationEntity app, UserEntity user, RolesEntity role)
        {
            throw new NotImplementedException();
        }

        public override void Delete(ApplicationEntity app)
        {
            throw new NotImplementedException();
        }

        public override List<ApplicationEntity> GetAppplicationForUser(UserEntity user)
        {
            throw new NotImplementedException();
        }

        public override List<Guid> GetAppplicationsIdsForUser(UserEntity user)
        {
            throw new NotImplementedException();
        }

        public override ApplicationEntity GetByApplicationKey(string applicationKey)
        {
            throw new NotImplementedException();
        }

        public override ApplicationEntity GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public override List<ApplicationEntity> GetByIds(List<Guid> ids)
        {
            throw new NotImplementedException();
        }

        public override void ResetUserRoles(UserEntity user, List<AppUserRoleEntity> newRoleList)
        {
            throw new NotImplementedException();
        }

        public override bool Save(UserEntity user)
        {
            throw new NotImplementedException();
        }

        public override IPagedList<ApplicationEntity> Search(ApplicationSearchSettings searchSettings)
        {
            throw new NotImplementedException();
        }
    }
}
