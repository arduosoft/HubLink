using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wlog.Models
{
    public class ApplicationEntry
    {
        public virtual int Id { get; protected set; }
        [Required]
        [Display(Name = "Application name")]
        public virtual string ApplicationName { get; set; }
        [Required]
        [Display(Name = "Active")]
        public virtual bool IsActive { get; set; }
        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public virtual DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public virtual System.Nullable<DateTime> EndDate { get; set; }
        //public virtual IList<ApplicationRoleEntry> AppRoles { get; set; }

        public ApplicationEntry()
        {
            this.Id = 0;
            this.ApplicationName = "Default";
            this.IsActive = true;
            this.StartDate = DateTime.Now;
        }


    }

    public class ApplicationMap : ClassMapping<ApplicationEntry>
    {
        public ApplicationMap()
        {
            Table("ApplicationEntry");
            Schema("dbo");
            Id(x => x.Id, map => { map.Column("IdApplication"); map.Generator(Generators.Identity); });
            Property(x => x.ApplicationName);
            Property(x => x.IsActive);
            Property(x => x.StartDate);
            Property(x=>x.EndDate);
            //Bag(x => x.AppRoles, colmap => { colmap.Key(x => x.Column("IdApplication")); colmap.Inverse(true); }, map => { map.OneToMany(); });
        }
    }
}