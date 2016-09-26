using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wlog.Library.BLL.Index
{
    class LuceneIndexContext
    {
        public   LuceneIndexManager DefaultIndexManager { get; set; }

        public LuceneIndexContext()
        {
            DefaultIndexManager = new LuceneIndexManager("Default");
        }

      static  LuceneIndexContext _current;
        public static  LuceneIndexContext Current
        {
            get
            {
                return _current ?? (_current = new LuceneIndexContext());
            }
        }
    }
}
