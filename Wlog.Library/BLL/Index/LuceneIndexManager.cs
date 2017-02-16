using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using Lucene.Net.Analysis.Standard;
using PagedList;
using System.Web;
using NLog;

namespace Wlog.Library.BLL.Index
{
    


    /// <summary>
    /// Manage a lucene index in write and read
    /// </summary>
    public class LuceneIndexManager: IDisposable
    {

        #region private
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static IndexWriter writer;
        private static IndexSearcher searcher;
        private static IndexReader reader;
        private QueryParser parser;
        private Analyzer analyzer ;

        protected static bool inited = false;
        private static Directory luceneIndexDirectory;
        private bool dirty = false;
        private int uncommittedFiles = 0;
        #endregion

        public int UncommittedFiles { get { return uncommittedFiles; } }

        /// <summary>
        /// Tell how many rows to collect before automatically commit.
        /// This is used only if "Autocommit" is on
        /// </summary>
        public int CommitSize { get; set; }

        /// <summary>
        /// Name of the index
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Path of the directory index
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// if on reopen index when you are quering index to use uncommited row. This forces commit so may affect performance
        /// </summary>
        public bool ReopenIfDirty { get; set; }

        /// <summary>
        /// if true automatically commit periodically. otherwise commit is delegated to caller
        /// </summary>
        public bool Autocommit { get; set; }

      

        public bool IsDirty
        {
            get { return dirty; }
        }


        public LuceneIndexManager(string name,string path)
        {
            logger.Debug("[IndexManager] ctor {0}, {1}",name,path);
            this.Name = name;
            this.Path = path;
            this.CommitSize = 0;
            ReopenIfDirty = false;
            Init();
        }

        private void Init()
        {
            //TODO: use factory or DI to  setup analyzer, query parse, and other tuning attribute. 
            logger.Debug("[IndexManager] init");
            analyzer = new StandardAnalyzer(global::Lucene.Net.Util.Version.LUCENE_30);
            InitIndex();
            parser = new QueryParser(global::Lucene.Net.Util.Version.LUCENE_30, "", analyzer);
        }

        private void InitIndex()
        {
            //TODO: use factory or DI to  setup analyzer, query parse, and other tuning attribute. 
            logger.Debug("[IndexManager] InitIndex");

            logger.Debug("[IndexManager] Opening {0}", Path);
            luceneIndexDirectory = FSDirectory.Open(Path);
            logger.Debug("[IndexManager] Creating IndexWriter");
            writer = new IndexWriter(luceneIndexDirectory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED);
            logger.Debug("[IndexManager] Creating IndexReader");
            reader = IndexReader.Open(luceneIndexDirectory, true);
            logger.Debug("[IndexManager] Creating IndexSearcher");
            searcher = new IndexSearcher(reader);
        }

        public void AddDocument(Dictionary<string,object> docs)
        {
            logger.Debug("[IndexManager] AddDocument from dictionary");

            var doc = new Document();
            foreach (string s in docs.Keys)
            {
                object val = docs[s];
                if (val is int || val is Int16)
                {
                    var v = new NumericField(s, 0, true);
                    v.SetIntValue((int)val);
                    doc.Add(v);
                }
                else if (val is double || val is Double)
                {
                    var v = new NumericField(s, 0, true);
                    v.SetDoubleValue((double)val);
                    doc.Add(v);
                }
                else
                {
                    val = val ?? "";
                    doc.Add(new Field(s, val.ToString() , Field.Store.YES, Field.Index.ANALYZED));
                }
            }
            AddDocument(doc);

        }
        public void AddDocument(Document doc)
        {
            logger.Debug("[IndexManager] AddDocument from doc (do not force commit)");
            AddDocument(doc, false);
        }
        public void AddDocument(Document doc,bool commit)
        {
            dirty = true;
            writer.AddDocument(doc);
            uncommittedFiles++;
          
            if (commit || (this.Autocommit && uncommittedFiles > this.CommitSize))
            {
                logger.Debug("[IndexManager] AddDocument forces commit penting changes commit={0}, autocommit={1}, commitSize={2},uncommittedFiles={3}",
                            commit,
                            Autocommit,
                            CommitSize, 
                            uncommittedFiles);

                SaveUncommittedChanges();
            }

          
        }

        public void SaveUncommittedChanges()
        {
            logger.Debug("[IndexManager] Save uncommitted files");
            logger.Debug("[IndexManager] Optimize");
            writer.Optimize();
            logger.Debug("[IndexManager] Commit");
            writer.Commit();
            logger.Debug("[IndexManager] Reset uncommitted Files");
            uncommittedFiles = 0;
            logger.Debug("[IndexManager] Reopen index");
            ReopenIndex();
        }

        
        public IPagedList<Document> Query(string queryTxt, int start, int size)
        {
            logger.Debug("[IndexManager] Query ({0},{1},{2})", queryTxt,start,size);
            return   Query(queryTxt, null, 0, false, start, size,"");
        }


        /// <summary>
        /// Query index
        /// </summary>
        /// <param name="queryTxt">full text query </param>
        /// <param name="sortname">name of field for sort</param>
        /// <param name="sortType">lucene sort type</param>
        /// <param name="desc">sort desc if true</param>
        /// <param name="start">number of first row to take</param>
        /// <param name="size">numer of row to take</param>
        /// <param name="defaultField">default field for searc. Empty string to searcg by complex query</param>
        /// <returns></returns>
        public IPagedList<Document> Query(string queryTxt,string sortname,int sortType,bool desc, int start, int size,string defaultField)
        {
            logger.Debug("[IndexManager] Query ({0},{1},{2},{3},{4},{5},{6})",
                 queryTxt,  sortname,  sortType,  desc,  start,  size,  defaultField);

            Query query = new MatchAllDocsQuery();
            if (!string.IsNullOrWhiteSpace(queryTxt))
            {
                try
                {
                    var localanalyzer = new StandardAnalyzer(global::Lucene.Net.Util.Version.LUCENE_30);
                    var localparser = new QueryParser(global::Lucene.Net.Util.Version.LUCENE_30, defaultField, localanalyzer);
                    query = parser.Parse(queryTxt);
                }
                catch(Exception err)
                {
                    //This is used to control flow, no log needed here
                    throw new UnableToParseQuery();
                    
                }
            }
            Sort s =null;
            if (sortname != null)
            {
                s = new Sort();  
                s.SetSort(new SortField(sortname, sortType, desc));
            }

            return Query(query, s, start,  size);
        }

       

       

        

        public IPagedList<Document> Query(Query query, Sort field, int start, int size)
        {

            logger.Debug("[IndexManager] Query ({0},{1},{2},{3})",
                  query,  field,  start,  size);
            if (dirty  && ReopenIfDirty)
            {
                ReopenIndex();

            }

            TopDocs docs = null;
            if (field == null)
            {
                docs = searcher.Search(query, start + size);
            }
            else
            {
                docs = searcher.Search(query,null,  start + size, field);
            }

            ScoreDoc item;
            Document doc;
            List<Document> result = new List<Document>();
            for (int i = start; i < start + size && i<docs.TotalHits;i++)
            {
                item=docs.ScoreDocs[i];
                doc=reader.Document(item.Doc);                
                result.Add(doc);
            }
            return new StaticPagedList<Document>(result,1,size,docs.TotalHits);
        }

        private void ReopenIndex()
        {

            logger.Debug("[IndexManager] Reopen index");

            DisposeIndex();
            InitIndex();
        }

        public void Dispose()
        {
            logger.Debug("[IndexManager] Dispose");

            logger.Debug("[IndexManager] Optimize and commit before destroing index manager");
            writer.Optimize();
            SaveUncommittedChanges();
            DisposeIndex();
            
            analyzer.Dispose();
            
        }
        public void DisposeIndex()
        {
            logger.Debug("[IndexManager] DisposeIndex");
            if (writer != null)
            {
                try
                {
                    writer.Commit();
                    writer.Dispose();
                }
                catch (Exception err)
                {
                    logger.Error(err);
                }
            }
            if (reader != null)
            {
                reader.Dispose();
            }
            if (searcher != null)
            {
                searcher.Dispose();
            }
        }

        public void RemoveDocument(string IdField,object value)
        {
            logger.Debug("[IndexManager] RemoveDocument");
            var localparser = new QueryParser(global::Lucene.Net.Util.Version.LUCENE_30, IdField, analyzer);

           Query q = localparser.Parse(value.ToString());
            writer.DeleteDocuments(q);
        }
    }
}
