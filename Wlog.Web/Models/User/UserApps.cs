using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wlog.Web.Models.User
{
    /// <summary>
    /// Class uset to represent relasionship between user and apps
    /// </summary>
    public class UserApps
    {
        //public int Id { get; set; }
        //public ApplicationEntity Application { get; set; }
        //public RolesEntity Role { get; set; }
        public virtual Guid IdApplication { get; set; }
        [Display(Name = "Application name")]
        public virtual string ApplicationName { get; set; }
        public virtual Guid RoleId { get; set; }
        public virtual string RoleName { get; set; }

    }
}