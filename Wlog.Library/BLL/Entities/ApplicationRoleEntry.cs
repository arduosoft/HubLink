using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson.Serialization.Attributes;

namespace Wlog.BLL.Entities
{
    public class ApplicationRoleEntity
    {
        [BsonId]
        public virtual Guid Id { get; protected set; }
        public virtual Guid ApplicationId { get; set; }
        public virtual Guid RoleId{ get; set; }
    }

    
}