using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Wlog.Web.Code.Classes;

namespace Wlog.Web.Models
{
    //Wlog.Web.Models.LogListModel
    public class LogListModel
    {

        public int? ApplicationId { get; set; }
        public string Level { get; set; }
        public string SortOrder { get; set; }
        public string SerchMessage { get; set; }
        public List<ApplicationEntity> Apps { get; set; }

       


      
        public IPagedList<LogEntity> Items { get; set; }

    }
}