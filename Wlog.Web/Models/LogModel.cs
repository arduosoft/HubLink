using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wlog.Web.Models
{
    public class LogModel
    {
        public virtual int ApplictionId { get; set; }
        [DataType(DataType.DateTime)]
        public virtual DateTime SourceDate { get; set; }
        public virtual string Message { get; set; }
        public virtual string Level { get; set; }
        public virtual Guid Uid { get; set; }
        
    }
}