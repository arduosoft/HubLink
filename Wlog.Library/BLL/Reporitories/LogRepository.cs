using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using Wlog.BLL.Entities;
using Wlog.Library.BLL.Classes;
using Wlog.Library.BLL.Enums;
using Wlog.Library.BLL.Interfaces;

namespace Wlog.Library.BLL.Reporitories
{
    public abstract class LogRepository : IRepository
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public abstract IPagedList<LogEntity> SeachLog(LogsSearchSettings logsSearchSettings);

        public abstract void Save(LogEntity entToSave);

        public abstract long CountByLevel(StandardLogLevels level);
    }
}
