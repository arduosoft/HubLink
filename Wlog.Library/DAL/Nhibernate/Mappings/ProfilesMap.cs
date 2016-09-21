using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode.Conformist;
using Wlog.BLL.Entities;
using NHibernate.Mapping.ByCode;

namespace Wlog.Library.DAL.Nhibernate.Mappings
{
    public class ProfilesMap : ClassMapping<ProfilesEntity>
    {
        public ProfilesMap()
        {
            Table("WL_Profiles");
            Schema("dbo");
            Id(x => x.Id, map =>
            {
                map.Column("IdRole");
                map.Generator(Generators.Guid);
            });
            Property(x => x.ProfileName);
        }
    }
}
