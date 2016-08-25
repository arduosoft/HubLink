﻿using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson.Serialization.Attributes;
using Wlog.Library.BLL.Interfaces;

namespace Wlog.BLL.Entities
{
    public class LogEntity : IEntityBase
    {
        //TODO: Extend this entity by introducing field like:
        /*
        -Application source (there could be more than one app logging...)
        -Level
        -StackTrace
        -potentially every ${param} defined in NLog; in practise we can take a look to https://github.com/nlog/nlog/wiki/Layout-Renderers and include what it's useful
        */

        public override Guid Id { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual DateTime UpdateDate { get; set; }
        public virtual DateTime SourceDate { get; set; }
        public virtual string AppDomain { get; set; }
        public virtual string AppModule { get; set; }
        public virtual string AppSession { get; set; }
        public virtual string AppUser { get; set; }
        public virtual string AppVersion { get; set; }
        public virtual string Device { get; set; }
        public virtual string Message { get; set; }
        public virtual string Level { get; set; }
        //[BsonId]
        public virtual Guid Uid { get { return this.Id; } set { this.Id = value; } }
        public virtual Guid ApplictionId { get; set; }

    }

   
}
