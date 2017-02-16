using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wlog.Library.BLL.Interfaces;

namespace Wlog.BLL.Entities
{

    /// <summary>
    /// This entity represent a copy of a log.
    /// It is used to backup deleted log in a virtual BIN
    /// </summary>
    public class DeletedLogEntity : IEntityBase
    {
        [BsonId]
        public override Guid Id { get; set; }

        /// <summary>
        /// Date when log is deleted
        /// </summary>
        public virtual DateTime DeletedOn { get; set; }

        /// <summary>
        /// Id of the log
        /// </summary>
        public virtual Guid LogId { get; set; }

        /*
         Following fields are a replica of standard log entity
        */
        
        public virtual DateTime CreateDate { get; set; }

        public virtual DateTime UpdateDate { get; set; }

        public virtual DateTime SourceDate { get; set; }

        public virtual string Message { get; set; }

        public virtual string Level { get; set; }

        public virtual Guid Uid
        {
            get
            {
                return this.Id;
            }
            set
            {
                this.Id = value;
            }
        }
        public virtual Guid ApplictionId { get; set; }
    }
}
