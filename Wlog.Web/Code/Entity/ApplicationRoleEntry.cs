using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wlog.Web.Code.Classes
{
    public class ApplicationRoleEntity
    {
        public virtual int Id { get; protected set; }
        public virtual ApplicationEntity Application { get; set; }
        public virtual RolesEntity Role { get; set; }
    }

    public class ApplicationRoleMap : ClassMapping<ApplicationRoleEntity>
    {
        public ApplicationRoleMap()
        {
            Table("WL_ApplicationRole");
            Schema("dbo");
            Id(x => x.Id, map => map.Generator(Generators.Identity));
            ManyToOne(x => x.Application, map => { map.Column("IdApplication"); map.Cascade(Cascade.None); });
            ManyToOne(x => x.Role, map => { map.Column("IdRole"); map.Cascade(Cascade.None); });
        }
    }
}