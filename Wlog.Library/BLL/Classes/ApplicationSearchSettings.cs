using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wlog.Library.BLL.Enums;

namespace Wlog.Library.BLL.Classes
{
    public class ApplicationSearchSettings:SearchSettingsBase
    {
        public ApplicationFields Orderby { get; set; }
        public string SerchFilter { get; set; }

    }
}
