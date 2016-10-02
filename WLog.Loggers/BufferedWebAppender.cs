using log4net.Appender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Core;
using NLog.WebLog.Helpers;
using NLog.WebLog.Classes;
using Newtonsoft.Json;

namespace Log4Net.WebLog
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
