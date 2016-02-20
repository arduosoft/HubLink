using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Wlog.BLL.Entities;
using Wlog.Library.BLL.Reporitories;

namespace Wlog.Web.Code.Authentication
{
    public class ApplicationContext
    {
        public ApplicationEntity ApplicationEntity;
        public List<RolesEntity> Roles;


        public ApplicationContext(Guid IdApplication)
        {

            this.ApplicationEntity = RepositoryContext.Current.Applications.GetById(IdApplication);
            this.Roles = RepositoryContext.Current.Roles.GetAllApplicationRoles(this.ApplicationEntity);

            
           
        }

        public static ApplicationContext Current
        {
            get
            {

                ApplicationContext _current = HttpContext.Current.Cache["ApplicationContext" + HttpContext.Current.Session.SessionID] as ApplicationContext;
                if (_current != null)
                    return _current;

                Guid idSessionApp;
                if (Guid.TryParse(HttpContext.Current.Cache["AppId_" + HttpContext.Current.Session.SessionID] as string, out idSessionApp))
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