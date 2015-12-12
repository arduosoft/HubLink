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
            return View();
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
            mm.Apps.Insert(0, new Wlog.Models.ApplicationEntity() {                
                ApplicationName="All application"
            });


            mm.Items = LogHelper.GetLogs(mm.ApplicationId, mm.SortOrder, mm.SerchMessage, 30, page??1);

            
           
           
            return View(mm);
        }

    }
}
