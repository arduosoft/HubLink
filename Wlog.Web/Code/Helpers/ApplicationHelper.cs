using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wlog.Web.Code.Authentication;
using Wlog.Web.Code.Classes;
using Wlog.Web.Models;
using Wlog.Web.Models.Application;

namespace Wlog.Web.Code.Helpers
{
    public class ApplicationHelper
    {
        /// <summary>
        /// Get Application by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ApplicationModel GetById(int id)
        {
            ApplicationEntity app;
            using (UnitOfWork uow = new UnitOfWork())
            {
                app = uow.Query<ApplicationEntity>().Where(x => x.IdApplication == id).FirstOrDefault();
                if (app == null) return null;
                return EntityToModel(app);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serchFilter"></param>
        /// <param name="pagenumber"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public static IPagedList<ApplicationModel> FilterApplicationList(string serchFilter, int pagenumber, int pagesize)
        {
            List<ApplicationModel> data = new List<ApplicationModel>();
            using (UnitOfWork uow = new UnitOfWork())
            {
                List<ApplicationEntity> entity;
                if (string.IsNullOrEmpty(serchFilter))
                {
                    entity = uow.Query<ApplicationEntity>().OrderBy(x => x.ApplicationName).ToList();
                }
                else
                {
                    entity = uow.Query<ApplicationEntity>().Where(x => x.ApplicationName.Contains(serchFilter)).OrderBy(x => x.ApplicationName).ToList();
                }

                foreach (ApplicationEntity e in entity)
                {
                    AppUserRoleEntity r = uow.Query<AppUserRoleEntity>().Where(x => (x.Role.RoleName == Constants.Roles.Admin || x.Role.RoleName == Constants.Roles.Write)&&x.Application.IdApplication==e.IdApplication && x.User.Id==UserProfileContext.Current.User.Id).FirstOrDefault();
                    if (UserProfileContext.Current.User.IsAdmin || r != null)
                        data.Add(EntityToModel(e));
                }
            }

            return new PagedList<ApplicationModel>(data, pagenumber, pagesize);
        }

        public static bool DeleteById(int id)
        {
            try
            {
                ApplicationEntity app;
                using (UnitOfWork uow = new UnitOfWork())
                {
                    uow.BeginTransaction();
                    app = uow.Query<ApplicationEntity>().Where(x => x.IdApplication == id).FirstOrDefault();

                    List<LogEntity> logs = uow.Query<LogEntity>().Where(x => x.ApplictionId == id).ToList();
                    foreach (LogEntity e in logs)
                    {
                        uow.Delete(e);
                    }

                    uow.Delete(app);

                    uow.Commit();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        private static ApplicationModel EntityToModel(ApplicationEntity entity)
        {
            ApplicationModel result = new ApplicationModel();
            result.ApplicationName = entity.ApplicationName;
            result.EndDate = entity.EndDate;
            result.IdApplication = entity.IdApplication;
            result.IsActive = entity.IsActive;
            result.StartDate = entity.StartDate;
            result.PublicKey = entity.PublicKey;
            return result;
        }
    }
}