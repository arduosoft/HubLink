using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson.Serialization.Attributes;
using Wlog.Library.BLL.Interfaces;

namespace Wlog.BLL.Entities
{
    public class RolesEntity : IEntityBase
    {

        public override Guid Id { get; set; }
        public virtual string RoleName { get; set; }

    }

  
}