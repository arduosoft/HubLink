using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Wlog.Models;

namespace Wlog.Web.Code.Authentication
{
    public class ApplicationContext
    {
        public ApplicationEntry ApplicationEntry;
        public List<RolesEntry> Roles;
        private ApplicationContext _Current;


        public ApplicationContext(int IdApplication)
        {
            using (ISession session = WebApiApplication.CurrentSessionFactory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    ApplicationEntry app=session.Query<ApplicationEntry>().Where(x => x.Id == IdApplication).FirstOrDefault();
                    if (app == null || app.Id < 1)
                    {
                        throw new Exception("Application not found");
                    }
                    this.ApplicationEntry = app;
                    this.Roles = session.Query<ApplicationRoleEntry>().Where(x => x.Application.Id == IdApplication).Select(x => x.Role).ToList();
                }
            }
        }

        public static ApplicationContext Current
        {
            get
            {

                ApplicationContext _current = HttpContext.Current.Cache["ApplicationContext" + HttpContext.Current.Session.SessionID] as ApplicationContext;
                if (_current != null)
                    return _current;

                int idSessionApp;
                if (int.TryParse(HttpContext.Current.Cache["AppId_" + HttpContext.Current.Session.SessionID] as string, out idSessionApp))
                {
                    _current = new ApplicationContext(idSessionApp);
                    HttpContext.Current.Cache["ApplicationContext" + HttpContext.Current.Session.SessionID] = _current;
                }
                else
                {
                    throw new Exception("Session App not Set");
                }
                return _current;
            }
        }


    }
}