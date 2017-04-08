using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wlog.Library.BLL.Interfaces;

namespace Wlog.BLL.Entities
{
    
    public class DictionaryEntity : IEntityBase
    {
        public virtual string Name { get; set; }
        public virtual Guid ApplicationId { get; set; }
       
    }
}
