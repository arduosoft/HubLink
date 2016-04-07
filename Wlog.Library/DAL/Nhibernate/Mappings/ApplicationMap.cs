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
    public class ApplicationMap : ClassMapping<ApplicationEntity>
    {
        public ApplicationMap()
        {
            Table("WL_Application");
            Schema("dbo");
            Id(x => x.IdApplication, map => { map.Column("IdApplication"); map.Generator(Generators.Guid); });
            Property(x => x.ApplicationName);
            Property(x => x.IsActive);
            Property(x => x.StartDate);
            Property(x => x.EndDate);
            Property(x => x.PublicKey);
            //Bag(x => x.AppRoles, colmap => { colmap.Key(x => x.Column("IdApplication")); colmap.Inverse(true); }, map => { map.OneToMany(); });
        }
    }
}
