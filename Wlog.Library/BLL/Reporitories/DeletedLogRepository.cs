namespace Wlog.Library.BLL.Reporitories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Wlog.BLL.Entities;
    using Classes;
    using Interfaces;
    using NLog;

    public class DeletedLogRepository : EntityRepository
    {
        private Logger _logger => LogManager.GetCurrentClassLogger();

        public List<DeletedLogEntity> GetAllDeletedLogEntities()
        {
            var result = new List<DeletedLogEntity>();
            using (IUnitOfWork uow = BeginUnitOfWork())
            {
                uow.BeginTransaction();
                result = uow.Query<DeletedLogEntity>().ToList();
            }

            return result;
        }

        public void RemoveDeletedLogEntity(DeletedLogEntity deletedLog)
        {
            try
            {
                using (IUnitOfWork uow = BeginUnitOfWork())
                {
                    uow.BeginTransaction();
                    uow.Delete(deletedLog);
                    uow.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public void BatchRemoveDeletedLogEntities(List<DeletedLogEntity> deletedLog)
        {
            try
            {
                using (IUnitOfWork uow = BeginUnitOfWork())
                {
                    uow.BeginTransaction();
                    var logsCount = deletedLog.Count();

                    for (int i = 0; i < logsCount; i++)
                    {
                        uow.Delete(deletedLog[i]);

                        if (i == logsCount - 1)
                        {
                            uow.Commit();
                        }
                        else if (i % 100 == 0)
                        {
                            uow.Commit();
                            uow.BeginTransaction();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public void Save(DeletedLogEntity deletedLog)
        {
            try
            {
                using (IUnitOfWork uow = BeginUnitOfWork())
                {
                    uow.BeginTransaction();
                    uow.SaveOrUpdate(deletedLog);
                    uow.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public bool ExecuteEmptyBinJob(int daysToKeep, int rowsToKeep)
        {
            try
            {
                using (IUnitOfWork uow = BeginUnitOfWork())
                {
                    uow.BeginTransaction();
                    var entitiesToKeep = uow.Query<DeletedLogEntity>().Where(x => x.SourceDate > (DateTime.UtcNow.AddDays(-daysToKeep)))
                        .OrderByDescending(x => x.SourceDate).Take(rowsToKeep).ToList();
                    var entitiesToDelete = uow.Query<DeletedLogEntity>().Where(x => !entitiesToKeep.Contains(x)).ToList();

                    BatchRemoveDeletedLogEntities(entitiesToDelete);

                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);

                return false;
            }
        }
    }
}
