using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wlog.Web.Models.Application
{
    public class ApplicationList
    {
        public IPagedList<ApplicationModel> AppList { get; set; }
        public string SerchMessage { get; set; }
    }
}