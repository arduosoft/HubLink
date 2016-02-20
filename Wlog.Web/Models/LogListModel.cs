using PagedList;
using System;
using System.Collections.Generic;
using Wlog.BLL.Entities;

namespace Wlog.Web.Models
{
    //Wlog.Web.Models.LogListModel
    public class LogListModel
    {

        public Guid? ApplicationId { get; set; }
        public string Level { get; set; }
        public string SortOrder { get; set; }
        public string SerchMessage { get; set; }
        public List<ApplicationEntity> Apps { get; set; }

       


      
        public IPagedList<LogEntity> Items { get; set; }

    }
}