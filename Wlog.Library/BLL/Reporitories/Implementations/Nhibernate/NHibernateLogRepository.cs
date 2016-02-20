using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using Wlog.BLL.Entities;
using Wlog.DAL.NHibernate.Helpers;
using Wlog.Library.BLL.Classes;
using Wlog.Library.BLL.Enums;

namespace Wlog.Library.BLL.Reporitories.Implementations.Nhibernate
{
    public class NHibernateLogRepository : LogRepository
    {
        public override long CountByLevel(StandardLogLevels level)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
               return uow.Query<LogEntity>().Count(p => level==StandardLogLevels.ALL_LEVELS || ( p.Level != null && p.Level.ToLower().Contains(level.ToString())));
            }
        }

        public override void Save(LogEntity entToSave)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                 uow.SaveOrUpdate(entToSave);
            }
        }

        public override IPagedList<LogEntity> SeachLog(LogsSearchSettings logsSearchSettings)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                IEnumerable<LogEntity> query = uow.Query<LogEntity>();



                if (!String.IsNullOrWhiteSpace(logsSearchSettings.SerchMessage))
                {
                    query = query.Where(p =>
                        (logsSearchSettings.SerchMessage != null && p.Message != null && p.Message.ToLower().Contains(logsSearchSettings.SerchMessage))
                        &&
                        (logsSearchSettings.Applications.Contains(p.ApplictionId))
                        );
                      
                }


                query = query.Skip((logsSearchSettings.PageNumber - 1) * logsSearchSettings.PageSize);
                query = query.Take(logsSearchSettings.PageSize);


                IPagedList<LogEntity> result = new StaticPagedList<LogEntity>(query, logsSearchSettings.PageNumber, logsSearchSettings.PageSize,1000);

                return result;
            }
        }
    }
}
