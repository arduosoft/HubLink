using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wlog.BLL.Classes
{
    public class LogMessage
    {
        public DateTime SourceDate { get; set; }
        public string Message { get; set; }
        public string Level { get; set; }
        public string ApplicationKey { get; set; }
        public string AppDomain { get; set; }
        public string AppModule { get; set; }
        public string AppSession { get; set; }
        public string AppUser { get; set; }
        public string AppVersion { get; set; }
        public string Device { get; set; }
    }
}