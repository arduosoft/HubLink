using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Wlog.Models;
using Wlog.Web.Code.Classes;

namespace Wlog.Web.Code.Authentication
{
    public class ApplicationContext
    {
        public ApplicationEntity ApplicationEntity;
        public List<RolesEntity> Roles;


        public ApplicationContext(int IdApplication)
        {
            using (UnitOfWork uof = new UnitOfWork())
            {
                this.ApplicationEntity = uof.Session.Query<ApplicationEntity>().Where(x => x.Id == IdApplication).FirstOrDefault();
                if (this.ApplicationEntity != null)
                {
                    this.Roles = uof.Session.Query<ApplicationRoleEntity>().Where(x => x.Application.Id == IdApplication).Select(x => x.Role).ToList();
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