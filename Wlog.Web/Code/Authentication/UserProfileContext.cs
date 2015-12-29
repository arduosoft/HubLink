using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wlog.Web.Code.Classes;
using Wlog.Web.Code.Helpers;

namespace Wlog.Web.Code.Authentication
{
    public class UserProfileContext
    {
        public UserEntity User { get; set; }
        public bool IsEditorUser { get; set; }
        //TODO: set property for usercontext

        public UserProfileContext()
        {
          
            this.User = UserHelper.GetByUsername(HttpContext.Current.User.Identity.Name);
            if (this.User.IsAdmin)
            {
                this.IsEditorUser = true;
            }
            else
           {
                int count;
                using (UnitOfWork uow = new UnitOfWork())
                {
                    count = uow.Query<AppUserRoleEntity>().Where(x => x.User.Id == this.User.Id && (x.Role.RoleName == Constants.Roles.Admin || x.Role.RoleName == Constants.Roles.Write)).Count();
                    if (count > 0)
                    {
                        this.IsEditorUser = true;
                    }
                    else
                        this.IsEditorUser = false;
                }
            }
        }


        public static UserProfileContext Current
        {
            get
            {

                UserProfileContext _current = HttpContext.Current.Cache["UserProfileContext" + HttpContext.Current.Session.SessionID] as UserProfileContext;
                    
                if (_current != null)
                    return _current;

                _current = new UserProfileContext();
                HttpContext.Current.Cache["UserProfileContext" + HttpContext.Current.Session.SessionID] = _current;
                return _current;
            }
        }

        public void Refresh()
        {
            HttpContext.Current.Cache.Remove("UserProfileContext" + HttpContext.Current.Session.SessionID);
        }
    }
}