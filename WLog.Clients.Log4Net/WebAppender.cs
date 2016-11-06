using log4net.Appender;
using log4net.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wlog.Clients.Classes;
using Wlog.Clients.Helpers;

namespace Wlog.Clients.Log4Net
{
    public class WebAppender : AppenderSkeleton
    {
        public string Destination { get; set; }
        public string ApplicationKey { get; set; }

        public WebAppender()
        {
            this.Name = "WebAppender";
        }
        protected override void Append(LoggingEvent loggingEvent)
        {
            string logMessage = loggingEvent.RenderedMessage;

            LogMessage entry = new LogMessage();
            entry.Message = logMessage;
            entry.SourceDate = loggingEvent.TimeStamp;
            entry.ApplicationKey = ApplicationKey;
            entry.Level = loggingEvent.Level.ToString();

            LogHelper.DoRequest(Destination, JsonConvert.SerializeObject(new LogMessage[] { entry }));
        }
    }
}
