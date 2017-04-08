using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wlog.Web.Models.Dictionary
{
    public class KeyValueItemModel
    {
        public string ItemKey { get; set; }
        public string ItemValue { get; set; }
        public string ApplicationId { get; set; }
        public string DictionaryName { get; set; }
    }
}