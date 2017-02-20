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
    /// <summary>
    /// Repo used to store profiles
    /// </summary>
    public class ProfilesRepository : EntityRepository
    {
       

        
        /// <summary>
        /// Get a profile by name
        /// </summary>
        /// <param name="profileName"></param>
        /// <returns></returns>
        public ProfilesEntity GetProfileByName(string profileName)
        {
            logger.Debug("[repo] entering GetProfileByName");
            using (IUnitOfWork uow = this.BeginUnitOfWork())
            {
                uow.BeginTransaction();
                return uow.Query<ProfilesEntity>().FirstOrDefault(x => x.ProfileName == profileName);
            }
        }

        /// <summary>
        /// Delete one profle
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get all profiles
        /// </summary>
        /// <returns></returns>
        public List<ProfilesEntity> GetAllProfiles()
        {

            //TODO: this method should take paging options
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

        /// <summary>
        /// Save one profile entity
        /// </summary>
        /// <param name="profilesEntity"></param>
        /// <returns></returns>
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


        /// <summary>
        /// Assign multiple roles to a profile.
        /// Already present roles wont be tuched
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
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
