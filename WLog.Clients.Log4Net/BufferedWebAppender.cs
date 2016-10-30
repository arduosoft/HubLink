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
    public class BufferedWebAppender : BufferingAppenderSkeleton
    {
        public BufferedWebAppender()
        {
            this.Name = "BufferedWebAppender";
        }
        public string Destination { get; set; }
        public string ApplicationKey { get; set; }
        protected override void SendBuffer(LoggingEvent[] events)
        {
            List<LogMessage> entry = events.Select(p => new LogMessage()
            {
                ApplicationKey = ApplicationKey,
                Level = p.Level.ToString(),
                Message = p.RenderedMessage,
                SourceDate = p.TimeStamp
            }).ToList();

            LogHelper.DoRequest(Destination, JsonConvert.SerializeObject(entry));
        }
    }
}
