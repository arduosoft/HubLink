using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wlog.Web.Models;
using Wlog.Web.Code.Helpers;
using Wlog.Web.Code.Classes;
using System.Web.Security;
using PagedList;

namespace Wlog.Web.Controllers
{
    public class PrivateController : Controller
    {
        public ActionResult Index()
        {
            string username = Membership.GetUser().UserName;
            List<int> apps = UserHelper.GetAppsIdsForUser(username);
            using (UnitOfWork uow = new UnitOfWork())
            {
                

                DashboardModel dm = new DashboardModel();
                dm.ErrorCount = uow.Query<LogEntity>().Count(p => p.Level != null && p.Level.ToLower().Contains("err"));
                dm.InfoCount = uow.Query<LogEntity>().Count(p => p.Level != null && p.Level.ToLower().Contains("info"));
                dm.LogCount = uow.Query<LogEntity>().Count();
                dm.WarnCount = uow.Query<LogEntity>().Count(p => p.Level != null && p.Level.ToLower().Contains("warn"));
                dm.LastTen = ConversionHelper.ConvertLogEntityToMessage(uow, uow.Query<LogEntity>().Where(p=> apps.Contains(p.ApplictionId)).OrderByDescending(p => p.SourceDate).Take(10).ToList());

                dm.AppLastTen = new List<MessagesListModel>();
                foreach (ApplicationEntity app in UserHelper.GetAppsForUser(username))
                {
                    MessagesListModel list = new MessagesListModel();
                    list.ApplicationName = app.ApplicationName;
                    list.IdApplication = app.IdApplication;
                    list.Messages= ConversionHelper.ConvertLogEntityToMessage(uow,uow.Query<LogEntity>().Where(p => p.ApplictionId==app.IdApplication).OrderByDescending(p => p.SourceDate).Take(10).ToList());
                    dm.AppLastTen.Add(list);

                }

                return View(dm);
            }
        }


        public ActionResult Logs(int? applicationId,string level,string sortOrder,string serchMessage,int? page, int? pageSize)
        {

            LogListModel mm = new LogListModel()
            {
                ApplicationId =applicationId,
                Level = level,
                SortOrder = sortOrder,
                SerchMessage = serchMessage,               
                
            };
            MembershipUser current = Membership.GetUser();
            mm.Apps = UserHelper.GetAppsForUser(current.UserName);
            mm.Apps.Insert(0, new ApplicationEntity() {                
                ApplicationName="All application"
            });


            mm.Items = LogHelper.GetLogs(mm.ApplicationId, mm.SortOrder, mm.SerchMessage, 30, page??1);

            
           
           
            return View(mm);
        }

    }
}
