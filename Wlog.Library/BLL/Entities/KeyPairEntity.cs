using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wlog.Library.BLL.Interfaces;

namespace Wlog.BLL.Entities
{
    public class KeyPairEntity :  IEntityBase
    {
        public virtual string ItemKey { get; set; }
        public virtual string ItemValue { get; set; }
        public virtual Guid DictionaryId { get; set; }
    }
}
