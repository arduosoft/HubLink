using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wlog.Library.BLL.Reporitories
{
    public class RepositoryContext
    {
        public ApplicationRepository Applications { get; set; }
        public UserRepository Users { get; set; }
        public LogRepository Logs { get; set; }
        public RolesRepository Roles { get; set; }
        public SystemRepository System { get; set; }

        private static RepositoryContext _current;
        public static RepositoryContext Current
         {
            get
                { return _current ?? (_current=new RepositoryContext() { }); }
            
           }
    }
}
