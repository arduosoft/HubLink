using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wlog.BLL.Entities;
using Wlog.Library.BLL.Interfaces;

namespace Wlog.Library.BLL.Reporitories
{
    public abstract class RolesRepository : IRepository
    {
        public List<RolesEntity> GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public abstract List<RolesEntity> GetAllApplicationRoles(ApplicationEntity applicationEntity);

        public abstract List<AppUserRoleEntity> GetApplicationRolesForUser(UserEntity userEntity);

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public abstract bool Save(RolesEntity rolesEntity);
        public abstract RolesEntity GetRoleByName(string rolename);
    }
}
