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

            //Move this to a background job
            if (LogQueue.Current.Count > 0)
            {
                LogEntry log = LogQueue.Current.Dequeue();



                using (ISession session = WebApiApplication.CurrentSessionFactory.OpenSession())
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(log);
                        transaction.Commit();
                    }
                }

            }
        }

    }
}