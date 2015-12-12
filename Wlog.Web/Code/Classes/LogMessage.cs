using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wlog.Web.Code.Classes
{
    public class LogMessage
    {
        public DateTime SourceDate { get; set; }
        public string Message { get; set; }
        public string Level { get; set; }
        public string ApplicationKey { get; set; }
    }
}