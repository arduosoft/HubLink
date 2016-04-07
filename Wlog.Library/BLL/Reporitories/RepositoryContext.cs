using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wlog.Library.BLL.Reporitories
{
    public class RepositoryContext
    {
        public ApplicationRepository Applications { get; private set; }
        public UserRepository Users { get; private set; }
        public LogRepository Logs { get; private set; }
        public RolesRepository Roles { get; private set; }
        public SystemRepository System { get; private set; }

        private static RepositoryContext _current;
        public static RepositoryContext Current
         {
             get
             {
                 if (_current != null && _current.Applications != null && _current.Users != null && _current.Logs != null && _current.Roles != null && _current.System != null)
                 {
                     
                 }
                 else
                 {
                     _current = new RepositoryContext();
                     _current.Applications = new ApplicationRepository();
                     _current.Users = new UserRepository();
                     _current.Logs = new LogRepository();
                     _current.Roles = new RolesRepository();
                     _current.System = new SystemRepository();
                 }
                 return _current;
             }
                    
            
           }
    }
}
