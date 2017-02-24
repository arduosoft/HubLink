
    using System;
    using Wlog.Library.BLL.Interfaces;

namespace Wlog.BLL.Entities
{
    /// <summary>
    /// Represent the list of roles inside a profile.
    /// </summary>
   public class ProfilesRolesEntity : IEntityBase
    {
        public virtual Guid ProfileId { get; set; }

        public virtual Guid RoleId { get; set; }
    }
}
