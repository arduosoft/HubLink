using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Documents;
using Wlog.Library.BLL.Index;
using Wlog.Test.Attributes;
using Xunit;

namespace Wlog.Test.Tests
{
    public class IndexManager
    {
        LuceneIndexManager index;


        public IndexManager()
        {
            InitTest();
        }

        private void InitTest()
        {
            string foldername = ".\\TestIndexRemove\\" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            Directory.CreateDirectory(foldername);
            index = new LuceneIndexManager("Test", foldername);
        }

        [Fact, TestPriority(0) Trait("Category", "ExcludedFromCI")]
        public void AddDocumentAndCommit()
        {
            string key = "test";
            string value = "value";
            

            Dictionary<string, object> doc = GetDocument(key, value);

            index.AddDocument(doc);
            index.SaveUncommittedChanges();

        }

        public Dictionary<string, object> GetDocument(string key, string value)
        {
            Dictionary<string, object> doc = new Dictionary<string, object>();
            doc.Add("key", value);
            doc.Add("content", "random" + DateTime.Now.Ticks);
            return doc;
        }
        [Fact, TestPriority(1) Trait("Category", "ExcludedFromCI")]
        public void TestIndexRemove()
        {
            Dictionary<string, object>[] docs = new Dictionary<string, object>[] {
                GetDocument("key", "doc1"),
                GetDocument("key", "doc2"),
                GetDocument("key", "doc3"),
                GetDocument("key", "doc4"),
                GetDocument("key", "doc5"),
                GetDocument("key", "doc6"),
                GetDocument("key", "doc7"),
                GetDocument("key", "doc8"),
            };

            foreach (var doc in docs)
            {
                index.AddDocument(doc);
            }
            index.SaveUncommittedChanges();

            index.RemoveDocument("key", "doc3");

            index.SaveUncommittedChanges();
        }
    }
}
