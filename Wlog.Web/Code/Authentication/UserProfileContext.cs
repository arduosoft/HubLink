using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wlog.Models;
using Wlog.Web.Code.Helpers;

namespace Wlog.Web.Code.Authentication
{
    public class UserProfileContext
    {
        public UserEntity User { get; set; }
        //TODO: set property for usercontext

        public UserProfileContext()
        {
          
            this.User = UserHelper.GetByUsername(HttpContext.Current.User.Identity.Name);
            
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
    }
}