using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wlog.BLL.Entities;
using Wlog.Library.BLL.DataBase;
using Wlog.Library.BLL.Interfaces;
using Wlog.Library.BLL.Classes;

namespace Wlog.Library.BLL.Reporitories
{
    public class ProfilesRepository : EntityRepository
    {
       

        

        public ProfilesEntity GetProfileByName(string profileName)
        {
            logger.Debug("[repo] entering GetProfileByName");
            using (IUnitOfWork uow = this.BeginUnitOfWork())
            {
                uow.BeginTransaction();
                return uow.Query<ProfilesEntity>().FirstOrDefault(x => x.ProfileName == profileName);
            }
        }

        public bool Delete(ProfilesEntity profile)
        {
            logger.Debug("[repo] entering Delete");
            bool result = true;
            try
            {
                using (IUnitOfWork uow =this.BeginUnitOfWork())
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
                logger.Error(e);
                result = false;
            }

            return result;
        }

        public List<ProfilesEntity> GetAllProfiles()
        {
            logger.Debug("[repo] entering GetAllProfiles");
            List<ProfilesEntity> result = new List<ProfilesEntity>();
            try
            {
                using (IUnitOfWork uow = this.BeginUnitOfWork())
                {
                    result.AddRange(uow.Query<ProfilesEntity>().ToList());
                }
            }
            catch (Exception err)
            {
                logger.Error(err);
            }

            return result;
        }

        public bool Save(ProfilesEntity profilesEntity)
        {
            logger.Debug("[repo] entering Save");
            try
            {
                using (IUnitOfWork uow = this.BeginUnitOfWork())
                {
                    uow.BeginTransaction();
                    uow.SaveOrUpdate(profilesEntity);
                    uow.Commit();
                }

                return true;
            }
            catch (Exception err)
            {
                logger.Error(err);
            }

            return false;
        }

        public bool AssignRolesToProfile(ProfilesEntity profile, List<RolesEntity> roles)
        {
            logger.Debug("[repo] entering AssignRolesToProfile");
            try
            {
                using (IUnitOfWork uow = this.BeginUnitOfWork())
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
                logger.Error(err);
            }

            return false;
        }

    }
}
