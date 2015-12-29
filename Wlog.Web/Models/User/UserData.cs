using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wlog.Web.Models.User
{
    public class UserData
    {
        public virtual int Id { get; set; }
        [Display(Name = "User name")]
        public virtual string Username { get; set; }
        public virtual string Email { get; set; }
        [Display(Name = "Is Admin")]
        public virtual bool IsAdmin { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Last Login")]
        public virtual DateTime LastLoginDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Creation Date")]
        public virtual DateTime CreationDate { get; set; }
        [Display(Name = "Is Online")]
        public virtual bool IsOnLine { get; set; }

    }
}