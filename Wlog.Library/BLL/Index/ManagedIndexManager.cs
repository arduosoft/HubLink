using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Linq;
using Lucene.Net.Linq.Fluent;
using Wlog.BLL.Entities;

namespace Wlog.Library.BLL.Index
{

    public class ManagedIndexManager : IDisposable
    {


        //protected static bool inited = false;
        //private static Directory luceneIndexDirectory;
        //LuceneDataProvider luceneDataProvider;

        //public string Name { get; set; }
        //public string Path { get { return ".\\" + Name; } }

        //private bool dirty = false;
        //int uncommittedFiles = 0;

        //public LuceneIndexManager(string name)
        //{
        //    this.Name = name;

        //    Init();
        //}

        //private void Init()
        //{

        //    InitIndex();
        //}

        //private void InitIndex()
        //{

        //    //           luceneIndexDirectory = FSDirectory.Open(Path);
        //    luceneIndexDirectory = MMapDirectory.Open(Path);
        //    luceneDataProvider = new LuceneDataProvider(luceneIndexDirectory, Lucene.Net.Util.Version.LUCENE_30);
        //    luceneDataProvider.Settings.EnableMultipleEntities = false;


        //}


        //public void CreateMapping()
        //{
        //    var map = new ClassMap<LogEntity>(Lucene.Net.Util.Version.LUCENE_30);

        //    map.Key(p => p.Id);

        //    map.Property(p => p.Message)
        //        .AnalyzeWith(new PorterStemAnalyzer(Version.LUCENE_30))
        //        .WithTermVector.PositionsAndOffsets();

        //    map.Property(p => p.DownloadCount)
        //        .AsNumericField()
        //        .WithPrecisionStep(8);

        //    map.Property(p => p.IconUrl).NotIndexed();

        //    map.Score(p => p.Score);

        //    map.DocumentBoost(p => p.Boost);
        //}


        //public ISession<T> GetSession<T>() where T :new()
        //{
        //    return luceneDataProvider.OpenSession<T>();
        //}
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
