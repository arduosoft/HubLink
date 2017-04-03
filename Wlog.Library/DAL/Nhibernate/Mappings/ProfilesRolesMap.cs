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
    public class ProfilesRolesMap : ClassMapping<ProfilesRolesEntity>
    {
        public ProfilesRolesMap()
        {
            Table("wl_profilesroles");
            //Schema("dbo");

            Id(x => x.Id, map => map.Generator(Generators.Guid));
            Property(x => x.ProfileId, map => { map.Column("IdProfile"); });
            Property(x => x.RoleId, map => { map.Column("IdRole"); });
        }
    }
}
