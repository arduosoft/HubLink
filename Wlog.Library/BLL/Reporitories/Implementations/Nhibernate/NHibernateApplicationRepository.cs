using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Linq;
using PagedList;
using Wlog.BLL.Classes;
using Wlog.BLL.Entities;
using Wlog.DAL.NHibernate.Helpers;
using Wlog.Library.BLL.Classes;

namespace Wlog.Library.BLL.Reporitories.Implementations.Nhibernate
{
    public class NHibernateApplicationRepository : ApplicationRepository
    {
        public override bool AssignRoleToUser(ApplicationEntity application, UserEntity user, RolesEntity role)
        {
            try

            {
                using (UnitOfWork uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    if (!uow.Query<AppUserRoleEntity>().Any(x => x.ApplicationId == application.IdApplication && x.RoleId == role.Id && x.UserId == user.Id))
                    {
                        AppUserRoleEntity app = new AppUserRoleEntity();
                        app.ApplicationId = app.ApplicationId;
                        app.RoleId = role.Id;
                        app.UserId = user.Id;
                        uow.SaveOrUpdate(app);

                        uow.Commit();
                        return true;
                    }

                }
            }
            catch (Exception err)
            {
                //TODO:Add log here
                return false;
            }
            return true;
        }

        public override void Delete(ApplicationEntity app)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                uow.BeginTransaction();
                ApplicationEntity appToDelete = uow.Query<ApplicationEntity>().Where(x => x.IdApplication == app.IdApplication).FirstOrDefault();

                List<LogEntity> logs = uow.Query<LogEntity>().Where(x => x.ApplictionId == app.IdApplication).ToList();
                foreach (LogEntity e in logs)
                {
                    uow.Delete(e);
                }

                uow.Delete(app);

                uow.Commit();
            }
        }

        public override List<ApplicationEntity> GetAppplicationForUser(UserEntity user)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {

                List<ApplicationEntity> applications;
                if (UserProfileContext.Current.User.IsAdmin)
                {
                    applications = uow.Query<ApplicationEntity>().ToList();
                }
                else
                {
                    List<Guid> appLinks = uow
                        .Query<AppUserRoleEntity>()
                        .Where(x => x.UserId == UserProfileContext.Current.User.Id)
                        .Select(x => x.ApplicationId)
                        .ToList();

                    applications = uow
                        .Query<ApplicationEntity>()
                        .Where(x => appLinks.Contains(x.IdApplication))
                        .ToList();
                }
                return applications;
            }
        }

        public override List<Guid> GetAppplicationsIdsForUser(UserEntity user)
        {
            return GetAppplicationForUser(user).Select(x => x.IdApplication).ToList();

        }

        public override ApplicationEntity GetByApplicationKey(string applicationKey)
        {
            Guid pk = new Guid(applicationKey);
            using (UnitOfWork uow = new UnitOfWork())
            {
                return  uow.Query<ApplicationEntity>().Where(x => pk.CompareTo(applicationKey)==0).FirstOrDefault();
                
            }
        }

        public override ApplicationEntity GetById(Guid id)
        {
            ApplicationEntity app;
            using (UnitOfWork uow = new UnitOfWork())
            {
                app = uow.Query<ApplicationEntity>().Where(x => x.IdApplication == id).FirstOrDefault();
                return app;
            }

        }

        public override List<ApplicationEntity> GetByIds(List<Guid> ids)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                return uow.Query<ApplicationEntity>().Where(p => ids.Contains(p.IdApplication)).ToList();
            }
        }

        public override void ResetUserRoles(UserEntity user,List<AppUserRoleEntity> newRoleList)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var appToDelete=uow.Query<AppUserRoleEntity>().Where(x => x.UserId == user.Id).ToList();
                foreach (var app in appToDelete)
                {
                    uow.Delete(app);
                }

                foreach (AppUserRoleEntity newRole in newRoleList)
                {
                    uow.SaveOrUpdate(newRole);
                }
                uow.Commit();
            }
        }

        public override bool Save(UserEntity user)
        {
            throw new NotImplementedException();
        }

        public override IPagedList<ApplicationEntity> Search(ApplicationSearchSettings searchSettings)
        {
          
            using (UnitOfWork uow = new UnitOfWork())
            {
                List<ApplicationEntity> entity;
                if (string.IsNullOrEmpty(searchSettings.SerchFilter))
                {
                    entity = uow.Query<ApplicationEntity>().OrderBy(x => x.ApplicationName).ToList();
                }
                else
                {
                    entity = uow.Query<ApplicationEntity>().Where(x => x.ApplicationName.Contains(searchSettings.SerchFilter)).OrderBy(x => x.ApplicationName).ToList();
                }

                //foreach (ApplicationEntity e in entity)
                //{
                //    AppUserRoleEntity r = uow.Query<AppUserRoleEntity>().Where(x => (x.Role.RoleName == Constants.Roles.Admin || x.Role.RoleName == Constants.Roles.Write) && x.Application.IdApplication == e.IdApplication && x.User.Id == UserProfileContext.Current.User.Id).FirstOrDefault();
                //    if (UserProfileContext.Current.User.IsAdmin || r != null)
                //        data.Add(EntityToModel(e));
                //}
            }

            return null;

        }
    }
}
