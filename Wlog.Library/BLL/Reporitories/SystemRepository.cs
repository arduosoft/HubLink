using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wlog.Library.BLL.DataBase;
using Wlog.Library.BLL.Interfaces;

namespace Wlog.Library.BLL.Reporitories
{
    public class SystemRepository : IRepository
    {
        public SystemRepository()
        {
            _UnitFactory = new UnitFactory();
        }

        private static UnitFactory _UnitFactory;

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void ApplySchemaChanges()
        {
            throw new NotImplementedException();
        }
    }
}
