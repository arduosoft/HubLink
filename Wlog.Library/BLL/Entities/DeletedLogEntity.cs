using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wlog.Library.BLL.Interfaces;

namespace Wlog.BLL.Entities
{
    public class DeletedLogEntity : IEntityBase
    {
        public virtual DateTime CreateDate { get; set; }

        public virtual DateTime UpdateDate { get; set; }

        public virtual DateTime SourceDate { get; set; }

        public virtual DateTime DeletedOn { get; set; }

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
