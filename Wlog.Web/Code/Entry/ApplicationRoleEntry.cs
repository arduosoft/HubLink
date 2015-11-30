using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wlog.Models
{
    public class ApplicationRoleEntry
    {
        public virtual int Id { get; protected set; }
        public virtual ApplicationEntry Application { get; set; }
        public virtual RolesEntry Role { get; set; }
    }

    public class ApplicationRoleMap : ClassMapping<ApplicationRoleEntry>
    {
        public ApplicationRoleMap()
        {
            Table("ApplicationRoleEntry");
            Schema("dbo");
            Id(x => x.Id, map => map.Generator(Generators.Identity));
            ManyToOne(x => x.Application, map => { map.Column("IdApplication"); map.Cascade(Cascade.None); });
            ManyToOne(x => x.Role, map => { map.Column("IdRole"); map.Cascade(Cascade.None); });
        }
    }
}