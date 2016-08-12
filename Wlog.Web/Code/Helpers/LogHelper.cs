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


        public static IPagedList<LogEntity> GetLogs(Guid? applicationId, string sortOrder, string sortBy, string serchMessage, int pageSize, int pageNumber)
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

            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.ToLower())
                {
                    case "asc": settings.SortBy = Library.BLL.Enums.SortDirection.ASC; break;
                    case "dsc": settings.SortBy = Library.BLL.Enums.SortDirection.DESC; break;
                }
            }
            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortOrder.ToLower())
                {
                    case "date": settings.OrderBy = Library.BLL.Enums.LogsFields.SourceDate; break;
                    case "level": settings.OrderBy = Library.BLL.Enums.LogsFields.Level; break;
                    case "message": settings.OrderBy = Library.BLL.Enums.LogsFields.Message; break;
                }
            }


            return RepositoryContext.Current.Logs.SeachLog(settings);
        }

       

       
    }
}