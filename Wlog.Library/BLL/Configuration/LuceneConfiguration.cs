using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Store;

namespace Wlog.Library.BLL.Configuration
{

    public enum LuceneStorage
    {
        FSDirectory,
        MMapDirectory,
        NIOFSDirectory,
        SimpleFSDirectory
    }

    public class LuceneConfigurationSettings: ConfigurationElement
    {

        [ConfigurationProperty("storage", DefaultValue = LuceneStorage.FSDirectory, IsRequired = false)]
        public LuceneStorage Storage {
            get
            { return (LuceneStorage)this["storage"]; }
            set
            { this["storage"] = value; }
        }



        /// <summary>
        /// In case of memory, path is unused
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        internal Directory GetLuceneIndexDirectory(string path)
        {
            switch (this.Storage)
            {
                case LuceneStorage.FSDirectory: return SimpleFSDirectory.Open(path);
                
                case LuceneStorage.MMapDirectory: return MMapDirectory.Open(path);
                case LuceneStorage.NIOFSDirectory: return NIOFSDirectory.Open(path);
                case LuceneStorage.SimpleFSDirectory: return SimpleFSDirectory.Open(path); 
                    
            }
            return FSDirectory.Open(path);

        }
    }
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
