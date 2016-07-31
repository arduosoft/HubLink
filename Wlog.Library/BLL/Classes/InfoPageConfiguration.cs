using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Wlog.Library.BLL.Helpers;

namespace Wlog.Library.BLL.Classes
{
    public class InfoPageConfiguration
    {
        public String BaseUrl { get; set; }
        public String ChangeLogPath { get; set; }
        public String LicensePath { get; set; }
        public String InfoPath { get; set; }
        public bool ShowInfo { get; set; }
        public bool ShowLicense { get; set; }
        public bool ShowChangeLog { get; set; }
        public Assembly MainAssembly { get; set; }
        public String ApplicationName { get; set; }
        public String ApplicationSubtitle { get; set; }

        public InfoPageConfiguration()
        {
            BaseUrl = "info";
            InfoPath = "~/Info.md";
            LicensePath = "~/License.md";
            ChangeLogPath = "~/ChangeLogs.md";
            this.ResolvePaths();
            //Set flags basing on visibility. user can change in config
            ShowInfo = File.Exists(InfoPath);
            ShowLicense = File.Exists(LicensePath);
            ShowChangeLog = File.Exists(ChangeLogPath);

        }



        internal void ResolvePaths()
        {
            InfoPath = InfoHelper.ResolveUrl(InfoPath);
            LicensePath = InfoHelper.ResolveUrl(LicensePath);
            ChangeLogPath = InfoHelper.ResolveUrl(ChangeLogPath);
        }
    }
}
