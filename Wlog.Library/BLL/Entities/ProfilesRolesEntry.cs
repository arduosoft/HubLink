using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wlog.Library.BLL.Interfaces;

namespace Wlog.BLL.Entities
{
   public class ProfilesRolesEntity : IEntityBase
    {
        public override Guid Id { get; set; }

        public virtual Guid ProfileId { get; set; }

        public virtual Guid RoleId { get; set; }
    }
}
