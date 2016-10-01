using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLog.WebLog.Classes
{
    public class LogMessage
    {
        public LogMessage()
        {
        }
        public DateTime SourceDate { get; set; }
        public string Message { get; set; }
        public string Level { get; set; }
        public string ApplicationKey { get; set; }
    }
}
