using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson.Serialization.Attributes;

namespace Wlog.BLL.Entities
{
    public class RolesEntity
    {
        [BsonId]
        public virtual Guid Id { get; set; }
        public virtual string RoleName { get; set; }

    }

  
}