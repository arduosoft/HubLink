using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wlog.Web.Code.Classes;

namespace Wlog.Web.Models
{
    public class DashboardModel
    {
        public int ErrorCount { get; set; }
        public int LogCount { get; set; }
        public int WarnCount { get; set; }
        public int InfoCount { get; set; }

        public List<QueueLoad> QueueLoad { get; set; }

      public  List<LogMessage> LastTen { get; set; }
      public List<MessagesListModel> AppLastTen { get; set; }


    }
}