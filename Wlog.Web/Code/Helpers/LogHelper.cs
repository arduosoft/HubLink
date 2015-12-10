using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wlog.Web.Code.Classes;
using Wlog.Web.Models;

namespace Wlog.Web.Code.Helpers
{
    public class LogHelper
    {
       

        internal static IList<LogEntity> GetLogs(int? applicationId, string sortOrder, string serchMessage, int pageSize, int pageNumber)
        {
            return new List<LogEntity>();
        }

        public static void AppendLog(LogEntity log)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                uow.SaveOrUpdate(log);
            }
        }
    }
}