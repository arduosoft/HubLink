using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Wlog.Library.BLL.Configuration;
using Wlog.Library.BLL.Helpers;

//Wlog.Web.Models.Install.InstallModel
namespace Wlog.Web.Models.Install
{
    public class InstallModel
    {
        [Display(Name = "Connection string")]
        public virtual string ConnectionString { get; set; }

        [Display(Name = "Driver")]
        public virtual string Driver { get; set; }

        [Display(Name = "Dialect")]
        public virtual string Dialect { get; set; }


        public Dictionary<string, List<string>> Drivers = new Dictionary<string, List<string>>();

        public string License { get; set; }

        public bool LicenseAccepted { get; set; }


        public InstallModel()
        {
            var model = InfoHelper.GetInfoPage(InfoPageConfigurator.Configuration);

            this.License = model.License;

        }
    }
}