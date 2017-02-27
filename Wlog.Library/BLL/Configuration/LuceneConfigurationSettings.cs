using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wlog.Library.BLL.Enums;

namespace Wlog.Library.BLL.Configuration
{
    public class LuceneConfigurationSettings : ConfigurationElement
    {

        [ConfigurationProperty("storage", DefaultValue = LuceneStorage.FSDirectory, IsRequired = false)]
        public LuceneStorage Storage
        {
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
}
