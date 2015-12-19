using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace Wlog.Web.Code.Classes
{
    public class UserEntity
    {
        public virtual int Id { get; protected set; }
        [Required]
        [Display(Name = "User name")]
        public virtual string Username { get; set; }
        public virtual string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public virtual string Password { get; set; }
        public virtual bool IsAdmin { get; set; }
        public virtual string PasswordQuestion { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "PasswordAnswer")]
        public virtual string PasswordAnswer { get; set; }
        public virtual bool  IsApproved { get; set; }
        [DataType(DataType.Date)]
        public virtual DateTime LastActivityDate { get; set; }
        [DataType(DataType.Date)]
        public virtual DateTime LastLoginDate { get; set; }
        [DataType(DataType.Date)]
        public virtual DateTime LastPasswordChangedDate { get; set; }
        [DataType(DataType.Date)]
        public virtual DateTime CreationDate { get; set; }
        public virtual bool IsOnLine { get; set; }
        public virtual bool IsLockedOut { get; set; }
        [DataType(DataType.Date)]
        public virtual DateTime LastLockedOutDate { get; set; }
        public virtual IList<ApplicationEntity> Application { get; set; }


        public UserEntity()
        {
            this.PasswordQuestion = "";
            this.PasswordAnswer = "";
            this.Email = "";
            this.IsAdmin = false;
            this.IsApproved = true;
            this.IsOnLine = false;
            this.IsLockedOut = false;

            this.CreationDate = DateTime.Now;
            this.LastPasswordChangedDate = DateTime.Now;
            this.LastActivityDate = DateTime.Now;
            this.LastLockedOutDate = DateTime.Now;
            this.LastLoginDate = DateTime.Now;
        }
        //public virtual void AddApplication(ApplicationEntity application,RolesEntry role)
        //{

        //    role.UsersInRole.Add(this);
        //    Roles.Add(role);
        //}

        //public virtual void RemoveRole(Roles role)
        //{
        //    role.UsersInRole.Remove(this);
        //    Roles.Remove(role);
        //}
    }

    public class UsersMap : ClassMapping<UserEntity>
    {
        public UsersMap()
        {
            Table("WL_User");
            Schema("dbo");
            Id(x => x.Id, map => { map.Column("IdUser"); map.Generator(Generators.Identity); });
            Property(x => x.Username);
            Property(x => x.Email);
            Property(x => x.Password);
            Property(x => x.IsAdmin);
            Property(x => x.PasswordQuestion);
            Property(x => x.PasswordAnswer);
            Property(x => x.IsApproved);
            Property(x => x.LastActivityDate);
            Property(x => x.LastLoginDate);
            Property(x => x.LastPasswordChangedDate);
            Property(x => x.CreationDate);
            Property(x => x.IsOnLine);
            Property(x => x.IsLockedOut);
            Property(x => x.LastLockedOutDate);
            Bag(x => x.Application, colmap => { colmap.Key(x => x.ForeignKey("")); colmap.Inverse(true); }, map => { map.OneToMany(); });
        }

    }
}