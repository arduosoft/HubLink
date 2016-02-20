using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson.Serialization.Attributes;

namespace Wlog.BLL.Entities
{
    public class UserEntity
    {
        [BsonId]
        public virtual Guid Id { get; set; }
       
        public virtual string Username { get; set; }
        public virtual string Email { get; set; }
      
        public virtual string Password { get; set; }
        public virtual bool IsAdmin { get; set; }
        public virtual string PasswordQuestion { get; set; }
 
        public virtual string PasswordAnswer { get; set; }
        public virtual bool  IsApproved { get; set; }

        public virtual DateTime LastActivityDate { get; set; }
  
        public virtual DateTime LastLoginDate { get; set; }

        public virtual DateTime LastPasswordChangedDate { get; set; }

        public virtual DateTime CreationDate { get; set; }
        public virtual bool IsOnLine { get; set; }
        public virtual bool IsLockedOut { get; set; }
       
        public virtual DateTime LastLockedOutDate { get; set; }


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

   
}