using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wlog.Library.BLL.Enums;

namespace Wlog.Library.BLL.Classes
{
    public class LogsSearchSettings:SearchSettingsBase
    {
        public String SerchMessage { get; set; }
        public List<Guid> Applications { get; set; }
        public LogsFields OrderBy { get; set; }
        public SortDirection SortBy { get; set; }

        public LogsSearchSettings()
        {
            this.Applications = new List<Guid>();
        }

    }
}
