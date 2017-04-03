using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wlog.BLL.Entities;

namespace Wlog.Library.DAL.Nhibernate.Mappings
{
    public class DeletedLogMap : ClassMapping<DeletedLogEntity>
    {
        public DeletedLogMap()
        {
            Table("wl_deletedlog");
           // Schema("dbo");

            Id(x => x.Uid, map => { map.Generator(Generators.Guid); });
            Property(x => x.Message);
            Property(x => x.Level);
            Property(x => x.SourceDate);
            Property(x => x.ApplictionId);
            Property(x => x.UpdateDate);
            Property(x => x.DeletedOn);
            Property(x => x.LogId);
        }
    }
}
