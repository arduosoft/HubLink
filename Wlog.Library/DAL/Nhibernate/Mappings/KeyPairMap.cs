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
    public class KeyPairMap : ClassMapping<KeyPairEntity>
    {
        public KeyPairMap()
        {
            Table("wl_keypair");
            //Schema("dbo");

            Id(x => x.Id, map => { map.Column("KeyPairId"); map.Generator(Generators.Guid); });

            Property(x => x.DictionaryId, map => {  map.UniqueKey("idx_dictuk");  });
            Property(x => x.ItemKey, map=> { map.UniqueKey("idx_dictuk"); });
            Property(x => x.ItemValue);
        }
    }
    
}
