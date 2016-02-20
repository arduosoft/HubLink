using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Wlog.BLL.Entities;
using Wlog.Library.BLL.Classes;
using Wlog.Library.BLL.Reporitories;

namespace Wlog.Web.Code.Helpers
{
    public class LogHelper
    {


        public static IPagedList<LogEntity> GetLogs(Guid? applicationId, string sortOrder, string serchMessage, int pageSize, int pageNumber)
        {
           


            List<Guid> alloweApps=UserHelper.GetAppsIdsForUser(Membership.GetUser().UserName);
            if (applicationId.HasValue)
            {
                if (alloweApps.Contains(applicationId.Value))
                {
                    alloweApps.Clear();
                    alloweApps.Add(applicationId.Value);
                }
            }

            LogsSearchSettings settings = new LogsSearchSettings()
            {
                Applications=alloweApps,
                SerchMessage= serchMessage,
                PageNumber=pageNumber,
                PageSize=pageSize
            };

            return RepositoryContext.Current.Logs.SeachLog(settings);
        }

       

       
    }
}