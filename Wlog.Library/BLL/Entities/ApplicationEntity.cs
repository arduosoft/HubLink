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
    public class ApplicationEntity : IEntityBase
    {

        
        public override Guid Id { get { return this.IdApplication; } set { this.Id = value; } }
        public virtual Guid IdApplication { get; set; }

        public virtual string ApplicationName { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual DateTime StartDate { get; set; }

        public virtual System.Nullable<DateTime> EndDate { get; set; }
        //public virtual IList<ApplicationRoleEntry> AppRoles { get; set; }


        public virtual Guid PublicKey { get; set; }

        public ApplicationEntity()
        {
            this.ApplicationName = "Default";
            this.IsActive = true;
            this.StartDate = DateTime.Now;
        }

    }

    
}