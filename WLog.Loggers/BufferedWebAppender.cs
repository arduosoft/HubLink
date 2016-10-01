using log4net.Appender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Core;

namespace NLog.WebLog
{
    class BufferedWebAppender : BufferingAppenderSkeleton
    {
        protected override void SendBuffer(LoggingEvent[] events)
        {
            throw new NotImplementedException();
        }
    }
}
