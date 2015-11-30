using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wlog.Web.Models
{
    public class ApplicationHomeModel
    {
        public virtual int Id { get; set; }
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

    }

    //public class ApplicationHome
    //{
    //    public virtual int Id;
    //    [Display(Name = "Application Name")]
    //    public virtual string ApplicationName { get; set; }
    //    [Display(Name = "Start Date")]
    //    [DataType(DataType.Date)]
    //    public virtual DateTime StartDate { get; set; }
    //    [Display(Name="Active")]
    //    public virtual bool IsActive { get; set; }
        
    //}
}