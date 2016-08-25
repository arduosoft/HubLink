using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Wlog.BLL.Entities;

namespace Wlog.Library.DAL.Nhibernate.Mappings
{
    public class LogEntityMap : ClassMapping<LogEntity>
    {


        public LogEntityMap()
        {
            Table("WL_LogEntity");
            Schema("dbo");
            
            Id(x => x.Uid, map => {  map.Generator(Generators.Guid); });
            Property(x => x.AppDomain);
            Property(x => x.AppModule);
            Property(x => x.AppSession);
            Property(x => x.AppUser);
            Property(x => x.AppVersion);
            Property(x => x.Device);
            Property(x => x.Message);
            Property(x => x.Level);
            Property(x => x.SourceDate);
            Property(x => x.ApplictionId);
            Property(x => x.UpdateDate);
        }
    }
}
