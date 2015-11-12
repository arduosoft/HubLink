using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wlog.Web.Code.Classes
{
    public class LogEntry
    {
        public  virtual DateTime SourceDate { get; set; }
        public  virtual string Message { get; set; }
        public  virtual Guid Uid {get;set;}

    }

    public class LogEntryMap:ClassMapping<LogEntry>
    {
       
    
        public LogEntryMap()
        {
            Table("LogEntry");
            Schema("dbo");
            Id(x => x.Uid,map=> map.Generator(Generators.Guid) );
            Property(x => x.Message);
            Property(x => x.SourceDate);
        }
    }
}
