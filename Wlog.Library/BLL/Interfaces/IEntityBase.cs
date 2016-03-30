using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wlog.Library.BLL.Interfaces
{
    public abstract class IEntityBase
    {
        [BsonId]
        public abstract Guid Id { get; set; }
    }
}
