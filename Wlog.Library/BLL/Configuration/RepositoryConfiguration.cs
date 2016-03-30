using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wlog.Library.BLL.Configuration
{
    public class RepositoryConfiguration : ConfigurationSection
    {
        internal static RepositoryConfiguration GetConfig()
        {
            return (RepositoryConfiguration)System.Configuration.ConfigurationManager.GetSection("RepositoryConfiguration") ?? new RepositoryConfiguration();
        }


        [System.Configuration.ConfigurationProperty("RepositoryCollection")]
        [ConfigurationCollection(typeof(RepositoryCollection), AddItemName = "Repository")]
        internal RepositoryCollection RepositoryCollection
        {
            get
            {
                object o = this["RepositoryCollection"];
                return o as RepositoryCollection;
            }
        }

    }
}
