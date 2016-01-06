using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wlog.Web.Code.Jobs
{
    /// <summary>
    /// This classes is required to allow "Always run" IIS configuration see http://docs.hangfire.io/en/latest/deployment-to-production/making-aspnet-app-always-running.html
    /// </summary>
    public class ApplicationPreload : System.Web.Hosting.IProcessHostPreloadClient
    {
        public void Preload(string[] parameters)
        {
          //  HangfireBootstrapper.Instance.Start();
        }
    }
}