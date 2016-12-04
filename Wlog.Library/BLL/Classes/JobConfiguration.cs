using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wlog.Library.BLL.Classes
{
    public class JobConfiguration
    {
        public virtual Guid JobInstanceId { get; set; }

        public virtual string JobName { get; set; }

        public virtual string Description { get; set; }

        public virtual string FullClassName { get; set; }

        [Required]
        public virtual bool Active { get; set; }

        [Required]
        public virtual string CronExpression { get; set; }
    }
}
