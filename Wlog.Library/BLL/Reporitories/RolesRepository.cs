using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wlog.BLL.Entities;
using Wlog.DAL.NHibernate.Helpers;
using Wlog.Library.BLL.DataBase;
using Wlog.Library.BLL.Interfaces;

namespace Wlog.Library.BLL.Reporitories
{
    public class RolesRepository : IRepository
    {

        private static UnitFactory _UnitFactory;

        public RolesRepository()
        {
            _UnitFactory = new UnitFactory();
        }

        public List<RolesEntity> GetAllApplicationRoles(ApplicationEntity applicationEntity)
        {
            using (IUnitOfWork uow = _UnitFactory.GetUnit(this))
            {
                uow.BeginTransaction();
                if (applicationEntity != null)
                {
                    List<Guid> ids = uow.Query<ApplicationRoleEntity>().Where(x => x.ApplicationId.Equals( applicationEntity.IdApplication)).Select(x => x.RoleId).ToList();
                    return uow.Query<RolesEntity>().Where(x => ids.Contains(x.Id)).ToList();
                }
            }
            return null;
        }

        public List<AppUserRoleEntity> GetApplicationRolesForUser(UserEntity userEntity)
        {

            using (IUnitOfWork uow = _UnitFactory.GetUnit(this))
            {
                uow.BeginTransaction();
                return uow.Query<AppUserRoleEntity>().Where(c => c.UserId.Equals( userEntity.Id)).ToList();
            }
        }

        public RolesEntity GetRoleByName(string rolename)
        {
            using (IUnitOfWork uow = _UnitFactory.GetUnit(this))
            {
                uow.BeginTransaction();
                return uow.Query<RolesEntity>().FirstOrDefault(x => x.RoleName == rolename);
            }
        }

        public bool Save(RolesEntity rolesEntity)
        {
            try
            {
                using (IUnitOfWork uow = _UnitFactory.GetUnit(this))
                {
                    uow.BeginTransaction();
                    uow.SaveOrUpdate(rolesEntity);
                    uow.Commit();
                }
                return true;
            }
            catch (Exception err)
            {
                //TODO: log here:

            }
            return false;
        }

        public List<RolesEntity> GetAllRoles()
        {
            return new List<RolesEntity>();
        }
    }
}
