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
using Wlog.DAL.NHibernate.Helpers;
using Wlog.Library.BLL.DataBase;
using Wlog.BLL.Classes;

namespace Wlog.Library.BLL.Reporitories
{
    public class LogRepository : IRepository
    {
        private static UnitFactory _UnitFactory;

        public LogRepository()
        {
            _UnitFactory = new UnitFactory();
        }

        public long CountByLevel(StandardLogLevels level)
        {
            using (IUnitOfWork uow = _UnitFactory.GetUnit(this))
            {
                uow.BeginTransaction();
                return uow.Query<LogEntity>().Count(p => level == StandardLogLevels.ALL_LEVELS || (p.Level != null && p.Level.ToLower().Contains(level.ToString())));
            }
        }

        public void Save(LogEntity entToSave)
        {
            using (IUnitOfWork uow = _UnitFactory.GetUnit(this))
            {
                uow.BeginTransaction();
                uow.SaveOrUpdate(entToSave);
                uow.Commit();
            }
        }

        public IPagedList<LogEntity> SeachLog(LogsSearchSettings logsSearchSettings)
        {
            using (IUnitOfWork uow = _UnitFactory.GetUnit(this))
            {
                uow.BeginTransaction();
                IEnumerable<LogEntity> query = uow.Query<LogEntity>().Where(x=>logsSearchSettings.Applications.Contains(x.ApplictionId));



                if (!String.IsNullOrWhiteSpace(logsSearchSettings.SerchMessage))
                {
                    query = query.Where(p =>(logsSearchSettings.SerchMessage != null && p.Message != null && p.Message.ToLower().Contains(logsSearchSettings.SerchMessage)));

                }

                if (logsSearchSettings.SortDirection == SortDirection.ASC)
                {
                    switch (logsSearchSettings.OrderBy)
                    {
                        case LogsFields.SourceDate:
                            query = query.OrderBy(l => l.SourceDate);
                            break;
                        case LogsFields.Message:
                            query = query.OrderBy(l => l.Message);
                            break;
                        case LogsFields.Level:
                            query = query.OrderBy(l => l.Level);
                            break;
                    }
                }
                else
                {
                    switch (logsSearchSettings.OrderBy)
                    {
                        case LogsFields.SourceDate:
                            query = query.OrderByDescending(l => l.SourceDate);
                            break;
                        case LogsFields.Message:
                            query = query.OrderByDescending(l => l.Message);
                            break;
                        case LogsFields.Level:
                            query = query.OrderByDescending(l => l.Level);
                            break;
                    }
                }
                



                int count = query.Count();


                query = query.Skip((logsSearchSettings.PageNumber - 1) * logsSearchSettings.PageSize);
                query = query.Take(logsSearchSettings.PageSize);

                IPagedList<LogEntity> result = new StaticPagedList<LogEntity>(query.ToList(), logsSearchSettings.PageNumber, logsSearchSettings.PageSize, count);

                return result;
            }
        }

        public void Run()
        {
            LogQueue.Current.AppendLoadValue(LogQueue.Current.Count, LogQueue.Current.MaxQueueSize);

            if (LogQueue.Current.Count > 0)
            {
                using (IUnitOfWork uow = _UnitFactory.GetUnit(this))
                {
                    uow.BeginTransaction();

                    for (int i = 0; i < Math.Min(LogQueue.Current.Count, LogQueue.Current.MaxProcessedItems); i++)
                    {

                        LogMessage log = LogQueue.Current.Dequeue();


                        //PersistLog(log);

                    }
                    uow.Commit();
                }
                LogQueue.Current.AppendLoadValue(LogQueue.Current.Count, LogQueue.Current.MaxQueueSize);
            }
        }
    }
}
