using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wlog.Web.Code.Jobs
{
    public class HangFireAuthorizationFilter : IAuthorizationFilter
    {
        public bool Authorize(IDictionary<string, object> owinEnvironment)
        {
            return true;
        }
    }
}