using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wlog.Web.Code.Classes
{
    public class RolesEntity
    {
        public virtual int Id { get; set; }
        public virtual string RoleName { get; set; }

    }

    public class RolesMap : ClassMapping<RolesEntity>
    {
        public RolesMap()
        {
            Table("WL_Roles");
            Schema("dbo");
            Id(x => x.Id, map => { map.Column("IdRole"); map.Generator(Generators.Identity); });
            Property(x => x.RoleName);
        }
    }
}