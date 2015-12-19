using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wlog.Web.Code.Classes;

namespace Wlog.Web.Models
{
    public class MessagesListModel
    {
        public string ApplicationName { get; set; }
        public int IdApplication { get; set; }                
        public List<LogMessage> Messages {get;set;}
    }
}