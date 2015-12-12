using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Wlog.Web.Code.Classes;
using Wlog.Web.Models;

namespace Wlog.Web.Code.Helpers
{
    public class LogHelper
    {


        public static IPagedList<LogEntity> GetLogs(int? applicationId, string sortOrder, string serchMessage, int pageSize, int pageNumber)
        {
           
            List<int> alloweApps=UserHelper.GetAppsIdsForUser(Membership.GetUser().UserName);

            using (UnitOfWork uow = new UnitOfWork())
            {
                IEnumerable<LogEntity> query = uow.Query<LogEntity>();

              

                if(!String.IsNullOrWhiteSpace(serchMessage))
                {
                    query = query.Where(p => 
                        (serchMessage!=null &&p.Message != null && p.Message.ToLower().Contains(serchMessage))
                        &&
                        (applicationId!=null && p.ApplictionId==applicationId)
                        &&
                        (alloweApps.Contains(p.ApplictionId))
                        );
                }

               
                //query = query.Skip((pageNumber - 1) * pageSize);
                //query = query.Take(pageSize);


                PagedList<LogEntity> result = new PagedList<LogEntity>(query, pageNumber, pageSize);
            
                 return result;
            }
        }

        public static void AppendLog(UnitOfWork uow,LogEntity log)
        {
            
                uow.SaveOrUpdate(log);
            
        }

        internal static void AppendLog(UnitOfWork uow, LogMessage log)
        {
            throw new NotImplementedException();
        }
    }
}