using System;
using System.ComponentModel.DataAnnotations;

namespace Wlog.Library.BLL.Classes
{
    public class UserApplication
    {
        public virtual Guid IdApplication { get; set; }

        [Display(Name = "Application name")]
        public virtual string ApplicationName { get; set; }

        public virtual Guid RoleId { get; set; }

        public virtual string RoleName { get; set; }
    }
}
