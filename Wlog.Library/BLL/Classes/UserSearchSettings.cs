using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wlog.Library.BLL.Enums;

namespace Wlog.Library.BLL.Classes
{
    public class UserSearchSettings:SearchSettingsBase
    {
        public UserFields OrderBy { get; set; }

        public String Username { get; set; }
      public   bool UsernamePartialSearch { get; set; }
        public String Email { get; set; }
    }
}
