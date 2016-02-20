using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http;
using Wlog.BLL.Classes;

namespace Wlog.Web.Code.API
{
    [AllowAnonymous]
    public class LogController : ApiController
    {
        
        // POST api/<controller>
        public void Post([FromBody]LogMessage value)
        {
            if (LogQueue.Current.MaxQueueSize > LogQueue.Current.Count)
            {
                LogQueue.Current.Enqueue(value);
            }
            else
            {
                LogQueue.Current.PersistLog(value);
            }

        }

    }
}