namespace Wlog.Library.BLL.Reporitories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Wlog.BLL.Entities;
    using Classes;
    using Interfaces;
    using NLog;
    using System.Diagnostics;

    public class DeletedLogRepository : EntityRepository
    {
       

        public List<DeletedLogEntity> GetAllDeletedLogEntities()
        {
            logger.Debug("[repo] entering GetAllDeletedLogEntities");
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
            logger.Debug("[repo] entering RemoveDeletedLogEntity");
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
                logger.Error(ex);
            }
        }

        public void BatchRemoveDeletedLogEntities(List<DeletedLogEntity> deletedLog)
        {
            logger.Debug("[repo] entering BatchRemoveDeletedLogEntities");
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
                logger.Error(ex);
            }
        }

        public void Save(DeletedLogEntity deletedLog)
        {
            logger.Debug("[repo] entering Save");
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
                logger.Error(ex);
            }
        }

        public bool ExecuteEmptyBinJob(int daysToKeep, int rowsToKeep)
        {
            Debug.Write("ExecuteEmptyBinJob 3");
            logger.Debug("[repo] entering ExecuteEmptyBinJob");
            try
            { 
                Debug.Write("ExecuteEmptyBinJob 2");
                using (IUnitOfWork uow = BeginUnitOfWork())
                {
                    uow.BeginTransaction();
                    var entitiesToKeep = uow.Query<DeletedLogEntity>().Where(x => x.SourceDate > (DateTime.UtcNow.AddDays(-daysToKeep)))
                        .OrderByDescending(x => x.SourceDate).Take(rowsToKeep).ToList();
                    var entitiesToDelete = uow.Query<DeletedLogEntity>().Where(x => !entitiesToKeep.Contains(x)).ToList();

                    BatchRemoveDeletedLogEntities(entitiesToDelete);
                    Debug.Write("ExecuteEmptyBinJob");
                    return true;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);

                return false;
            }
        }
    }
}
