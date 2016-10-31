using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wlog.BLL.Entities;
using Wlog.Library.BLL.DataBase;
using Wlog.Library.BLL.Interfaces;

namespace Wlog.Library.BLL.Reporitories
{
    public class ProfilesRepository : IRepository
    {
        private Logger _logger => LogManager.GetCurrentClassLogger();

        private static UnitFactory unitFactory;

        public ProfilesRepository()
        {
            unitFactory = new UnitFactory();
        }


        public ProfilesEntity GetProfileByName(string profileName)
        {
            using (IUnitOfWork uow = unitFactory.GetUnit(this))
            {
                uow.BeginTransaction();
                return uow.Query<ProfilesEntity>().FirstOrDefault(x => x.ProfileName == profileName);
            }
        }

        public bool Delete(ProfilesEntity profile)
        {
            bool result = true;
            try
            {
                using (IUnitOfWork uow = unitFactory.GetUnit(this))
                {
                    uow.BeginTransaction();
                    List<ProfilesRolesEntity> entity = uow.Query<ProfilesRolesEntity>()
                        .Where(x => x.RoleId.Equals(profile.Id)).ToList();

                    foreach (ProfilesRolesEntity e in entity)
                    {
                        uow.Delete(e);
                    }

                    uow.Delete(profile);

                    uow.Commit();
                }
            }
            catch (Exception e)
            {
                _logger.Error(e);
                result = false;
            }

            return result;
        }

        public List<ProfilesEntity> GetAllProfiles()
        {
            List<ProfilesEntity> result = new List<ProfilesEntity>();
            try
            {
                using (IUnitOfWork uow = unitFactory.GetUnit(this))
                {
                    result.AddRange(uow.Query<ProfilesEntity>().ToList());
                }
            }
            catch (Exception err)
            {
                _logger.Error(err);
            }

            return result;
        }

        public bool Save(ProfilesEntity profilesEntity)
        {
            try
            {
                using (IUnitOfWork uow = unitFactory.GetUnit(this))
                {
                    uow.BeginTransaction();
                    uow.SaveOrUpdate(profilesEntity);
                    uow.Commit();
                }

                return true;
            }
            catch (Exception err)
            {
                _logger.Error(err);
            }

            return false;
        }

        public bool AssignRolesToProfile(ProfilesEntity profile, List<RolesEntity> roles)
        {
            try
            {
                using (IUnitOfWork uow = unitFactory.GetUnit(this))
                {
                    uow.BeginTransaction();

                    foreach (var role in roles)
                    {
                        if (!uow.Query<ProfilesRolesEntity>().Any(x => x.ProfileId.Equals(profile.Id) && x.RoleId.Equals(role.Id)))
                        {
                            ProfilesRolesEntity entity = new ProfilesRolesEntity();
                            entity.ProfileId = profile.Id;
                            entity.RoleId = role.Id;
                            uow.SaveOrUpdate(entity);
                        }
                    }

                    uow.Commit();

                    return true;
                }
            }
            catch (Exception err)
            {
                _logger.Error(err);
            }

            return false;
        }

    }
}
