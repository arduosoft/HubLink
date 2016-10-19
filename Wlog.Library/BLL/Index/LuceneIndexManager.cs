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

namespace Wlog.Library.BLL.Index
{
    class UnableToParseQuery : Exception
    { }


    /// <summary>
    /// Manage a lucene indxe
    /// </summary>
    public class LuceneIndexManager: IDisposable
    {

        #region private
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
            this.Name = name;
            this.Path = path;
            this.CommitSize = 0;
            ReopenIfDirty = false;
            Init();
        }

        private void Init()
        {
            analyzer = new StandardAnalyzer(global::Lucene.Net.Util.Version.LUCENE_30);
            InitIndex();
            parser = new QueryParser(global::Lucene.Net.Util.Version.LUCENE_30, "", analyzer);
        }

        private void InitIndex()
        {

            //           luceneIndexDirectory = FSDirectory.Open(Path);
            luceneIndexDirectory = FSDirectory.Open(Path);
            writer = new IndexWriter(luceneIndexDirectory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED);
            
            reader = IndexReader.Open(luceneIndexDirectory, true);
            searcher = new IndexSearcher(reader);
        }

        public void AddDocument(Dictionary<string,object> docs)
        {
           
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
            AddDocument(doc, true);
        }
        public void AddDocument(Document doc,bool commit)
        {
            dirty = true;
            writer.AddDocument(doc);
            uncommittedFiles++;
            if (this.Autocommit && uncommittedFiles > this.CommitSize)
            {
                if (commit)
                {
                    SaveUncommittedChanges();
                }
            }

          
        }

        internal void SaveUncommittedChanges()
        {
            writer.Optimize();
            writer.Commit();

            uncommittedFiles = 0;

            ReopenIndex();
        }

        
        public IPagedList<Document> Query(string queryTxt, int start, int size)
        {

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
            DisposeIndex();
            InitIndex();
        }

        public void Dispose()
        {
            writer.Optimize();
            SaveUncommittedChanges();
            DisposeIndex();
            
            analyzer.Dispose();
            
        }
        public void DisposeIndex()
        {
            if (writer != null)
            {
                try
                {
                    writer.Commit();
                    writer.Dispose();
                }
                catch (Exception err)
                {
                    //TODO: Add log here
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
    }
}
