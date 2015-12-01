using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wlog.Models;
using Wlog.Web.Code.Authentication;
using Wlog.Web.Code.Classes;
using Wlog.Web.Models;

namespace Wlog.Web.Code.Repository
{
    public class ApplicationRepository
    {
        /// <summary>
        /// Get ApplicationEntry by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ApplicationEntry GetById(int id)
        {
            ApplicationEntry app;
            using (ISession session = WebApiApplication.CurrentSessionFactory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    app = session.Query<ApplicationEntry>().Where(x => x.Id == id).FirstOrDefault();
                }
            }
            if (app == null) return null;
            return app;
        }

        /// <summary>
        /// Return Application for current User
        /// </summary>
        /// <returns></returns>
        public List<ApplicationHomeModel> GetHomeApp()
        {
            List<ApplicationHomeModel> result = new List<ApplicationHomeModel>();
            if (UserProfileContext.Current.User!=null)
            { 
            WLogRoleProvider roleProvider = new WLogRoleProvider();
            using (ISession session = WebApiApplication.CurrentSessionFactory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    List<ApplicationEntry> entity;
                    if (roleProvider.IsUserInRole(HttpContext.Current.User.Identity.Name, "ADMIN"))
                    {
                        entity = session.Query<ApplicationEntry>().ToList();
                    }
                    else
                    {
                        entity = session.Query<AppUserRoleEntry>().Where(x => x.User.Id == UserProfileContext.Current.User.Id).Select(x => x.Application).Where(x => x.IsActive == true).ToList();
                    }
                    result.AddRange(ConvertListEntityToListApplicationHome(entity));
                }
            };
        }
            return result;
        }

        public List<LogModel> GetLog(int ApplicationId, string sortOrder, string SerchMessage)
        {
            IQueryable<LogEntry> entry ;
            using (ISession session = WebApiApplication.CurrentSessionFactory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    entry = session.Query<LogEntry>().Where(x => x.ApplictionId == ApplicationId);
                    if (!string.IsNullOrEmpty(SerchMessage))
                        entry = entry.Where(x => x.Message.Contains(SerchMessage));

                    return ConvertListEntryToListLogModel(entry.ToList());
                }
            }
           
        }

        public void Save()
        {
            using (ISession session = WebApiApplication.CurrentSessionFactory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    ApplicationEntry entry = new ApplicationEntry();
                    entry.ApplicationName = "Test";
                    entry.EndDate = DateTime.Now.AddYears(1);
                    entry.StartDate = DateTime.Now.AddYears(-1);
                    entry.IsActive = true;
                    session.SaveOrUpdate(entry);
                    System.Web.Security.MembershipCreateStatus s;
                    (new WLogMembershipProvider()).CreateUser("Test", "123456", "", "12", null, true, null, out s);
                    

                    AppUserRoleEntry e = new AppUserRoleEntry();
                    e.Application = entry;
                    e.User = session.Query<UserEntry>().FirstOrDefault(); ;
                    session.SaveOrUpdate(e);

                    LogEntry l = new LogEntry();
                    l.ApplictionId=entry.Id;
                    l.Level="Debug";
                    l.Message="Test";
                    l.SourceDate=DateTime.Now;
                    session.SaveOrUpdate(l);

                    

                    transaction.Commit();
                }
            }
            
        }

        #region Entity To Model
        /// <summary>
        /// Convert LogEntry to LogModel
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static LogModel ConvertEntityToLogModel(LogEntry entity)
        {
            LogModel result = new LogModel();
            result.ApplictionId = entity.ApplictionId;
            result.Level = entity.Level;
            result.Message = entity.Message;
            result.SourceDate = entity.SourceDate;
            return result;
        }

        /// <summary>
        /// Convert List LogEntry to list LogModel
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static List<LogModel> ConvertListEntryToListLogModel(List<LogEntry> entity)
        {
            List<LogModel> result = new List<LogModel>();
            foreach (LogEntry item in entity)
            {
                result.Add(ConvertEntityToLogModel(item));
            }
            return result;
        }

        /// <summary>
        /// Convert ApplicationEntry to ApplicationHome
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static ApplicationHomeModel ConvertEntityToApplicationHome(ApplicationEntry entity)
        {
            ApplicationHomeModel result = new ApplicationHomeModel();
            result.Id = entity.Id;
            result.ApplicationName = entity.ApplicationName;
            result.StartDate = entity.StartDate;
            result.IsActive = entity.IsActive;
            return result;
        }

        

        /// <summary>
        /// Convert List<ApplicationEntry> to List<ApplicationHome>
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static List<ApplicationHomeModel> ConvertListEntityToListApplicationHome(List<ApplicationEntry> entity)
        {
            List<ApplicationHomeModel> result = new List<ApplicationHomeModel>();
            foreach (ApplicationEntry app in entity)
            {
                result.Add(ConvertEntityToApplicationHome(app));
            }
            return result;
        }
        #endregion
    }
}