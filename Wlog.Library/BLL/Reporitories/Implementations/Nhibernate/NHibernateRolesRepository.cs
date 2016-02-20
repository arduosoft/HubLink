using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Linq;
using Wlog.BLL.Entities;
using Wlog.DAL.NHibernate.Helpers;

namespace Wlog.Library.BLL.Reporitories.Implementations.Nhibernate
{
    public class NHibernateRolesRepository : RolesRepository
    {
        public override List<RolesEntity> GetAllApplicationRoles(ApplicationEntity applicationEntity)
        {
            using (UnitOfWork uof = new UnitOfWork())
            {

                if (applicationEntity != null)
                {
                    List<Guid> ids=uof.Session.Query<ApplicationRoleEntity>().Where(x => x.ApplicationId == applicationEntity.IdApplication).Select(x => x.RoleId).ToList();
                    return uof.Query<RolesEntity>().Where(x => ids.Contains(x.Id)).ToList();
                }
            }
            return null;
        }

        public override List<AppUserRoleEntity> GetApplicationRolesForUser(UserEntity userEntity)
        {
            
            using (UnitOfWork u = new UnitOfWork())
            {
                return u.Query<AppUserRoleEntity>().Where(c=> c.UserId == userEntity.Id).ToList();
            }
        }

        public override RolesEntity GetRoleByName(string rolename)
        {
            using (UnitOfWork u = new UnitOfWork())
            {
               return u.Query<RolesEntity>().FirstOrDefault(x => x.RoleName == rolename);
            }
        }

        public override bool Save(RolesEntity rolesEntity)
        {
            try

            {
                using (UnitOfWork u = new UnitOfWork())
                {
                    u.SaveOrUpdate(rolesEntity);
                }
                return true;
            }
            catch (Exception err)
            {
               //TODO: log here:

            }
            return false;
        }
    }
}
