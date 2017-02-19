﻿//******************************************************************************
// <copyright file="license.md" company="Wlog project  (https://github.com/arduosoft/wlog)">
// Copyright (c) 2016 Wlog project  (https://github.com/arduosoft/wlog)
// Wlog project is released under LGPL terms, see license.md file.
// </copyright>
// <author>Daniele Fontani, Emanuele Bucaelli</author>
// <autogenerated>true</autogenerated>
//******************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using Wlog.BLL.Entities;
using Wlog.Library.BLL.Classes;
using Wlog.Library.BLL.Interfaces;
using Wlog.DAL.NHibernate.Helpers;
using Wlog.BLL.Classes;
using Wlog.Library.BLL.DataBase;

namespace Wlog.Library.BLL.Reporitories
{
    /// <summary>
    /// Repository to store applications
    /// </summary>
    public class ApplicationRepository: EntityRepository
    {

        public ApplicationRepository()
        {
            
        }

        public ApplicationEntity GetById(Guid id)
        {
            logger.Debug("[repo] entering GetById");
            ApplicationEntity app;
            using (IUnitOfWork uow = BeginUnitOfWork())
            {
                uow.BeginTransaction();
                app = uow.Query<ApplicationEntity>().Where(x => x.IdApplication.Equals(id)).FirstOrDefault();
                return app;
            }
        }

        /// <summary>
        /// Reset user roles basing on role list
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        public void ResetUserRoles(UserEntity user, List<AppUserRoleEntity> role)
        {
            logger.Debug("[repo] entering ResetUserRoles");
            try
            {
                Guid idapp=role.Select(x=>x.ApplicationId).Distinct().First();
                using (IUnitOfWork uow = BeginUnitOfWork())
                {
                    uow.BeginTransaction();

                    //TODO: delete only removed entry,add the new ones.
                    List<AppUserRoleEntity> deleterole = uow.Query<AppUserRoleEntity>()
                        .Where(x => x.UserId.Equals(user.Id) && x.ApplicationId.Equals(idapp)).ToList();

                    foreach (AppUserRoleEntity del in deleterole)
                    {
                        uow.Delete(del);
                    }

                    foreach (AppUserRoleEntity rol in role)
                    {
                        uow.SaveOrUpdate(rol);
                    }

                    uow.Commit();
                }
            }
            catch (Exception err)
            {
                logger.Error(err);
            }
        }

        /// <summary>
        /// Deletes the application from table AppUserRoleEntity
        /// </summary>
        /// <param name="app">the application that need to be deleted</param>
        public void DeleteApplicationRole(ApplicationEntity app)
        {

            logger.Debug("[repo] entering DeleteApplicationRole");
            using (IUnitOfWork uow = BeginUnitOfWork())
            {
                uow.BeginTransaction();

                List<AppUserRoleEntity> deleterole = uow.Query<AppUserRoleEntity>()
                        .Where(x =>  x.ApplicationId.Equals(app.IdApplication)).ToList();

                foreach (AppUserRoleEntity del in deleterole)
                {
                    uow.Delete(del);
                }

                uow.Commit();
            }
        }


        /// <summary>
        /// Search application basing on search settings.
        /// </summary>
        /// <param name="searchSettings"></param>
        /// <returns></returns>
        public IPagedList<ApplicationEntity> Search(ApplicationSearchSettings searchSettings)
        {
            logger.Debug("[repo] entering Search");

            List<ApplicationEntity> applitationList;

            using (IUnitOfWork uow = BeginUnitOfWork())
            {
                uow.BeginTransaction();

                if (string.IsNullOrEmpty(searchSettings.SerchFilter))
                {
                    applitationList = uow.Query<ApplicationEntity>()
                        .OrderBy(x => x.ApplicationName).ToList();
                }
                else
                {
                    applitationList = uow.Query<ApplicationEntity>()
                        .Where(x => x.ApplicationName.Contains(searchSettings.SerchFilter))
                        .OrderBy(x => x.ApplicationName).ToList();
                }

                UserEntity user = RepositoryContext.Current.Users.GetByUsername(searchSettings.UserName);

                if (!user.IsAdmin)
                {
                    var applicationsForUser = GetAppplicationsIdsByUsername(user.Username);
                    applitationList = applitationList.Where(x => applicationsForUser.Contains(x.IdApplication)).ToList();
                }

                //TODO: why all application are retrieved from db, then search result is filtered by usename permission? could be easier to pass filters to GetAppplicationsIdsByUsername ?? 
            }

            return new StaticPagedList<ApplicationEntity>(applitationList, searchSettings.PageNumber, searchSettings.PageSize, applitationList.Count);
        }


        /// <summary>
        /// Delete one application from db, removing all relate data
        /// </summary>
        /// <param name="app"></param>
        public void Delete(ApplicationEntity app)
        {
            logger.Debug("[repo] entering Delete");
            using (IUnitOfWork uow = BeginUnitOfWork())
            {
                uow.BeginTransaction();
                ApplicationEntity appToDelete = uow.Query<ApplicationEntity>().Where(x => x.IdApplication.Equals(app.IdApplication)).FirstOrDefault();


                //TODO: for performance issuea could be better to: 1. use a batch statment 2. use a sessionless transaction
                //TODO: index of log on filesistem must be deleted too.
                List<LogEntity> logs = uow.Query<LogEntity>().Where(x => x.ApplictionId.Equals( app.IdApplication)).ToList();
                foreach (LogEntity e in logs)
                {
                    uow.Delete(e);
                }

                DeleteApplicationRole(app);

                uow.Delete(app);

                uow.Commit();
            }
        }

        /// <summary>
        /// Get the list of allowed application by username
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public List<ApplicationEntity> GetAppplicationsByUsername(string userName)
        {
            logger.Debug("[repo] entering GetAppplicationsByUsername");
            UserEntity user = RepositoryContext.Current.Users.GetByUsername(userName);
            List<Guid> ids = GetAppplicationsIdsForUser(user);
            return RepositoryContext.Current.Applications.GetByIds(ids);
        }

        /// <summary>
        /// Return a list of application by a list of ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<ApplicationEntity> GetByIds(List<Guid> ids)
        {
            logger.Debug("[repo] entering GetByIds");

            List<ApplicationEntity> result = new List<ApplicationEntity>();
     
            using (IUnitOfWork uow = BeginUnitOfWork())
            {
                uow.BeginTransaction();
                foreach (Guid id in ids)
                {
                    result.Add(GetById(id));
                }
                
            }
            return result;
        }

        public void Save(ApplicationEntity app)
        {
            logger.Debug("[repo] entering Save");
            using (IUnitOfWork uow = BeginUnitOfWork())
            {
                uow.BeginTransaction();
                uow.SaveOrUpdate(app);
                uow.Commit();
            }
        }

        /// <summary>
        /// Give alist of application ids for username
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public List<Guid> GetAppplicationsIdsByUsername(string userName)
        {
            logger.Debug("[repo] entering GetAppplicationsIdsByUsername");
            UserEntity user = RepositoryContext.Current.Users.GetByUsername(userName);
            List<Guid> ids = GetAppplicationsIdsForUser(user);
            return ids;
        }

        /// <summary>
        /// get a list of allowed application ids for the user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<Guid> GetAppplicationsIdsForUser(UserEntity user)
        {
            //TODO: why this is not just an overload of  List<Guid> GetAppplicationsIdsByUsername(string userName) ??
            logger.Debug("[repo] entering GetAppplicationsIdsForUser");
            return GetAppplicationForUser(user).Select(x => x.IdApplication).ToList();

        }

        /// <summary>
        /// Return list of allowed application for a given user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<ApplicationEntity> GetAppplicationForUser(UserEntity user)
        {
            logger.Debug("[repo] entering GetAppplicationForUser");
            using (IUnitOfWork uow = BeginUnitOfWork())
            {
                uow.BeginTransaction();
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

        /// <summary>
        /// Find application by its key
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <returns></returns>
        public ApplicationEntity GetByApplicationKey(string applicationKey)
        {
            logger.Debug("[repo] entering GetByApplicationKey");
            Guid pk = new Guid(applicationKey);
       
            using (IUnitOfWork uow = BeginUnitOfWork())
            {
                uow.BeginTransaction();
                return uow.Query<ApplicationEntity>().Where(x => x.IdApplication.Equals(pk) || x.PublicKey.Equals(pk)).FirstOrDefault();

            }
        }

        /// <summary>
        /// Assign a role to the user
        /// </summary>
        /// <param name="application"></param>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool AssignRoleToUser(ApplicationEntity application, UserEntity user, RolesEntity role)
        {
            logger.Debug("[repo] entering AssignRoleToUser");
            try
            {
          
                using (IUnitOfWork uow = BeginUnitOfWork())
                {
                    uow.BeginTransaction();
                    if (!uow.Query<AppUserRoleEntity>().Any(x => x.ApplicationId.Equals(application.IdApplication) && x.RoleId.Equals(role.Id) && x.UserId.Equals(user.Id)))
                    {
                        AppUserRoleEntity app = new AppUserRoleEntity();
                        app.ApplicationId = application.IdApplication;
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
                logger.Error(err);
                return false;
            }
            return true;
        }
    }
}
