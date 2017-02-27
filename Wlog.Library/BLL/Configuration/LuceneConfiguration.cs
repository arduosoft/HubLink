using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Store;
using Wlog.Library.BLL.Enums;

namespace Wlog.Library.BLL.Configuration
{

   
    
    public class LuceneConfiguration:ConfigurationSection
    {

        public static LuceneConfigurationSettings GetConfig()
        {
            var section = System.Configuration.ConfigurationManager.GetSection("LuceneConfiguration");
            var config = (LuceneConfiguration)section;
            if(config==null) return new LuceneConfigurationSettings();
            if (config.Settings == null) return new LuceneConfigurationSettings();
            return config.Settings;
        }

        [ConfigurationProperty ("Settings")]
        public LuceneConfigurationSettings Settings
        {
            get
            {
                object o = this["Settings"];
                return o as LuceneConfigurationSettings;
            }
        }
    }
}
