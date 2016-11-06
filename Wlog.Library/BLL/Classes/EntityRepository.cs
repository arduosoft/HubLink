using System;
using NLog;
using NLog.Config;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wlog.Library.BLL.DataBase;
using Wlog.Library.BLL.Interfaces;

namespace Wlog.Library.BLL.Classes
{
    public abstract class EntityRepository : IRepository
    {
        private static UnitFactory unitFactory;

        public static  Logger logger { get { return LogManager.GetCurrentClassLogger(); } }

        public IUnitOfWork BeginUnitOfWork()
        {
            // I don't check if unitFactory is null prior to initialization since it's already done within its constructor
            unitFactory = new UnitFactory();
            return unitFactory.GetUnit(this);
        }
    }
}
