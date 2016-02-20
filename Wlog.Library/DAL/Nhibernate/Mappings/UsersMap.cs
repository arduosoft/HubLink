using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode.Conformist;
using Wlog.BLL.Entities;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Wlog.BLL.Entities;

namespace Wlog.Library.DAL.Nhibernate.Mappings
{
    public class UsersMap : ClassMapping<UserEntity>
    {
        public UsersMap()
        {
            Table("WL_User");
            Schema("dbo");
            Id(x => x.Id, map => { map.Column("IdUser"); map.Generator(Generators.Guid); });
            Property(x => x.Username);
            Property(x => x.Email);
            Property(x => x.Password);
            Property(x => x.IsAdmin);
            Property(x => x.PasswordQuestion);
            Property(x => x.PasswordAnswer);
            Property(x => x.IsApproved);
            Property(x => x.LastActivityDate);
            Property(x => x.LastLoginDate);
            Property(x => x.LastPasswordChangedDate);
            Property(x => x.CreationDate);
            Property(x => x.IsOnLine);
            Property(x => x.IsLockedOut);
            Property(x => x.LastLockedOutDate);
            
        }

    }
}
