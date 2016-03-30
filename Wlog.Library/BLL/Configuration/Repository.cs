using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wlog.Library.BLL.Configuration
{
    public class Repository : ConfigurationElement
    {
        [ConfigurationProperty("RepositoryName", IsRequired = true)]
        public string RepositoryName
        {
            get { return this["RepositoryName"] as string; }
        }

        [ConfigurationProperty("DataBase", IsRequired = true)]
        public string DataBase
        {
            get { return this["DataBase"] as string; }
        }
    }
}
