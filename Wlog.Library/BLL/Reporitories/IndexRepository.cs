using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Wlog.Library.BLL.Index;
using Wlog.Library.BLL.Interfaces;

namespace Wlog.Library.BLL.Reporitories
{
    public class IndexRepository: IRepository
    {
        public static string BasePath { get; set; }
        private static Dictionary<string, LuceneIndexManager> indexList = new Dictionary<string, LuceneIndexManager>();

        public LuceneIndexManager GetByName(string entity,string segment)
        {
            return GetByName(entity + "." + segment);
        }

        public  LuceneIndexManager GetByName(string name)
        {
            if(!indexList.ContainsKey(name))
            {
                CreateIndex(name);
            }
            return indexList[name];
        }

        private void CreateIndex(string name)
        {
            var path = Path.Combine(BasePath, name);
            var idx = new LuceneIndexManager(name, path);
            if (!Directory.Exists(idx.Path))
            {
                Directory.CreateDirectory(idx.Path);
            }
            indexList.Add(name,idx );
        }

        internal List<LuceneIndexManager> GetAll()
        {
            return indexList.Values.ToList();
        }
    }
}
