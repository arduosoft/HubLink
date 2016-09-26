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

namespace Wlog.Library.BLL.Index
{
   
    public class LuceneIndexManager: IDisposable
    {
        public int CommitSize { get; set; }
        
        private static IndexWriter writer;
        private static IndexSearcher searcher;
        private static IndexReader reader;
        QueryParser parser;
        private Analyzer analyzer ;

        protected static bool inited = false;
        private static Directory luceneIndexDirectory;

        public string Name { get; set; }
        public string Path { get; set; }

        private bool dirty = false;
        int uncommittedFiles = 0;

        public bool IsDirty
        {
            get { return dirty; }
        }

        public LuceneIndexManager(string name)
        {
            this.Name = name;
            this.CommitSize = 0;
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
            luceneIndexDirectory = MMapDirectory.Open(Path);
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
            writer.AddDocument(doc);
            // writer.Optimize();
            //writer.Commit();
            uncommittedFiles++;
            if(uncommittedFiles > this.CommitSize)
            {
                if (commit)
                {
                    SaveUncommittedChanges();
                }
            }
            
            dirty = true;
        }

        internal void SaveUncommittedChanges()
        {
           
            writer.Commit();

            uncommittedFiles = 0;
        }

        public IPagedList<Document> Query(string queryTxt, int start, int size)
        {

         return   Query(queryTxt, null, 0, false, start, size);
        }


        public IPagedList<Document> Query(string queryTxt,string sortname,int sortType,bool desc, int start, int size)
        {
            Query query = new MatchAllDocsQuery();
            if (!string.IsNullOrEmpty(queryTxt))
            {
                query = parser.Parse(queryTxt);
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
            if (dirty)
            {
                  DisposeIndex();
                  InitIndex(); 

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
            return new StaticPagedList<Document>(result,start/size,size,docs.TotalHits);
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
            writer.Commit();
            writer.Dispose();
            reader.Dispose();
            searcher.Dispose();
        }
    }
}
