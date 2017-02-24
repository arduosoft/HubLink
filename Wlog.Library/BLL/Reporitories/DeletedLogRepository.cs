﻿namespace Wlog.Library.BLL.Reporitories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Wlog.BLL.Entities;
    using Classes;
    using Interfaces;
    using NLog;
    using System.Diagnostics;

    /// <summary>
    /// Repository to store deleted logs
    /// </summary>
    public class DeletedLogRepository : EntityRepository
    {
       
        /// <summary>
        /// Get all deleted logs
        /// </summary>
        /// <returns></returns>
        public List<DeletedLogEntity> GetAllDeletedLogEntities()
        {
            //TODO: this method seems to be used only in test.
            //TODO: this method should have parameter to return a list of data (full list can be retrieved passing int.MaxValue)
            logger.Debug("[repo] entering GetAllDeletedLogEntities");
            var result = new List<DeletedLogEntity>();
            using (IUnitOfWork uow = BeginUnitOfWork())
            {
                uow.BeginTransaction();
                result = uow.Query<DeletedLogEntity>().ToList();
            }

            return result;
        }

        /// <summary>
        /// Remove a deleted entry
        /// </summary>
        /// <param name="deletedLog"></param>
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


        /// <summary>
        /// Remove a list of deletd logs
        /// </summary>
        /// <param name="deletedLog"></param>
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

        /// <summary>
        /// Add or update deleted log
        /// </summary>
        /// <param name="deletedLog"></param>
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

        /// <summary>
        /// Execute a job to clead recicle bing
        /// </summary>
        /// <param name="daysToKeep"></param>
        /// <param name="rowsToKeep"></param>
        /// <returns></returns>
        public bool ExecuteEmptyBinJob(int daysToKeep, int rowsToKeep)
        {
            Debug.Write("ExecuteEmptyBinJob 3");
            logger.Debug("[repo] entering ExecuteEmptyBinJob");
            try
            { 
                Debug.Write("ExecuteEmptyBinJob 2");
                using (IUnitOfWork uow = BeginUnitOfWork())
                {

                    //TODO: this couldn't work on large amount of data. This beacause it assume to load in memory result of the "big" query returning all entity to delete,
                    //one solution could be limit the numeber of row to take (but could avoid complete bin flush) or invoke nhibernate batch statement

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
