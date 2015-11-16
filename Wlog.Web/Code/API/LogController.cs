using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Wlog.Web.Code.Classes;

namespace Wlog.Web.Code.API
{
    public class LogController : ApiController
    {


        // POST api/<controller>
        public void Post([FromBody]LogEntry value)
        {
            
            LogQueue.Current.Enqueue(value);

           
            
        }

    }
}