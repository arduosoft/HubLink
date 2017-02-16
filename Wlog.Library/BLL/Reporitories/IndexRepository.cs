using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using NLog;
using Wlog.Library.BLL.Index;
using Wlog.Library.BLL.Interfaces;

namespace Wlog.Library.BLL.Reporitories
{
    /// <summary>
    /// Repository to manage indexs. "entities" managed by this repo are indexmanagers.
    /// </summary>
    public class IndexRepository: IRepository
    {

        public static Logger logger { get { return LogManager.GetCurrentClassLogger(); } }

        public static string BasePath { get; set; }

       
        private static Dictionary<string, LuceneIndexManager> indexList = new Dictionary<string, LuceneIndexManager>();

        public LuceneIndexManager GetByName(string entity,string segment)
        {
            logger.Debug("[repo] entering GetByName ({0},{1})",entity,segment);
            return GetByName(entity + "." + segment);
        }

        public  LuceneIndexManager GetByName(string name)
        {

            logger.Debug("[repo] entering GetByName ({0})", name);

            if (!indexList.ContainsKey(name))
            {
                CreateIndex(name);
            }
            return indexList[name];
        }

        private void CreateIndex(string name)
        {
            logger.Debug("[repo] entering CreateIndex ({0})", name);

            var path = Path.Combine(BasePath, name);
            var idx = new LuceneIndexManager(name, path);
            idx.CommitSize = int.MaxValue; //commit is owned by the caller.
            if (!Directory.Exists(idx.Path))
            {
                Directory.CreateDirectory(idx.Path);
            }
            indexList.Add(name,idx );
        }

        internal List<LuceneIndexManager> GetAll()
        {
            logger.Debug("[repo] entering GetAll ");
            return indexList.Values.ToList();
        }


        /// <summary>
        /// Commit all changes for all indexs
        /// </summary>
        public void CommitAllIndexChanges()
        {
            logger.Debug("[repo] entering CommitAllIndexChanges ");
            CommitAllIndexChanges(0);
        }

        /// <summary>
        /// for all index, if index has enough row to commit, it flus it
        /// </summary>
        /// <param name="minRowCount"></param>
        public void CommitAllIndexChanges(int minRowCount)
        {
            logger.Debug("[repo] entering CommitAllIndexChanges {0}", minRowCount);
            foreach (var idx in RepositoryContext.Current.Index.GetAll())
            {
                if (idx.IsDirty && idx.UncommittedFiles > minRowCount) idx.SaveUncommittedChanges();
            }
        }
    }
}
