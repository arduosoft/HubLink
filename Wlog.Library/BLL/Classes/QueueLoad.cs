using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wlog.BLL.Classes
{
    public class QueueLoad
    {
        public DateTime Time { get; set; }
        public int QueueSize { get; set; }
        public int MaxSize { get; set; }
    }
}