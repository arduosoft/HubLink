using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wlog.Web.Models
{
    public class JobsConfigurationModel
    {
        [Required]
        public virtual Guid JobInstanceId { get; set; }

        [Required]
        public virtual string JobName { get; set; }

        [Required]
        public virtual string Description { get; set; }

        [Required]
        public virtual bool Active { get; set; }

        [Required]
        public virtual string CronExpression { get; set; }
    }
}