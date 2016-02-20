using System;
using System.Collections.Generic;
using Wlog.BLL.Classes;

namespace Wlog.Web.Models
{
    public class MessagesListModel
    {
        public string ApplicationName { get; set; }
        public Guid IdApplication { get; set; }                
        public List<LogMessage> Messages {get;set;}
    }
}