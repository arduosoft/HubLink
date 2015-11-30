using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wlog.Models
{
    public class RolesEntry
    {
        public virtual int Id { get; protected set; }
        public virtual string RoleName { get; set; }

    }

    public class RolesMap : ClassMapping<RolesEntry>
    {
        public RolesMap()
        {
            Table("Roles");
            Schema("dbo");
            Id(x => x.Id, map => { map.Column("IdRole"); map.Generator(Generators.Identity); });
            Property(x => x.RoleName);
        }
    }
}