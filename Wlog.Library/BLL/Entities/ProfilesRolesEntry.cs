namespace Wlog.BLL.Entities
{
    using System;
    using Wlog.Library.BLL.Interfaces;

    public class ProfilesRolesEntity : IEntityBase
    {
        public virtual Guid ProfileId { get; set; }

        public virtual Guid RoleId { get; set; }
    }
}
