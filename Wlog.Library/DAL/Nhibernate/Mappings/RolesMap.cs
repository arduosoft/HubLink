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
    public class RolesMap : ClassMapping<RolesEntity>
    {
        public RolesMap()
        {
            Table("WL_Roles");
            Schema("dbo");
            Id(x => x.Id, map => { map.Column("IdRole"); map.Generator(Generators.Guid); });
            Property(x => x.RoleName);
        }
    }
}
