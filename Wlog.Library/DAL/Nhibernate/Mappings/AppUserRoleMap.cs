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
    public class AppUserRoleMap : ClassMapping<AppUserRoleEntity>
    {
        public AppUserRoleMap()
        {
            Table("WL_AppUserRole");
            Schema("dbo");
            Id(x => x.Id, map => map.Generator(Generators.Guid));
            Property(x => x.ApplicationId, map => { map.Column("IdApplication");  });
            Property(x => x.UserId, map => { map.Column("IdUser");  });
            Property(x => x.RoleId, map => { map.Column("IdRole");  });
        }
    }
}
