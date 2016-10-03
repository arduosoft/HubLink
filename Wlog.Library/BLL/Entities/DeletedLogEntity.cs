using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wlog.Library.BLL.Interfaces;

namespace Wlog.BLL.Entities
{
    public class DeletedLogEntity : LogEntity
    {
        [BsonId]
        public override Guid Id { get; set; }

        public virtual DateTime DeletedOn { get; set; }

        public DeletedLogEntity()
        {
            Id = Guid.NewGuid();
        }
    }
}
