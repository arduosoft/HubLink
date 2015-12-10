using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wlog.Web.Code.Classes
{
    public class LogEntity
    {
        //TODO: Extend this entity by introducing field like:
        /*
        -Application source (there could be more than one app logging...)
        -Level
        -StackTrace
        -potentially every ${param} defined in NLog; in practise we can take a look to https://github.com/nlog/nlog/wiki/Layout-Renderers and include what it's useful
        */
        public virtual DateTime SourceDate { get; set; }
        public  virtual string Message { get; set; }
        public virtual string Level { get; set; }
        public  virtual Guid Uid {get;set;}
        public virtual int ApplictionId { get; set; }

    }

    public class LogEntityMap:ClassMapping<LogEntity>
    {
       
    
        public LogEntityMap()
        {
            Table("LogEntity");
            Schema("dbo");
            Id(x => x.Uid,map=> map.Generator(Generators.Guid) );
            Property(x => x.Message);
            Property(x => x.Level);
            Property(x => x.SourceDate);
            Property(x => x.ApplictionId);
        }
    }
}
