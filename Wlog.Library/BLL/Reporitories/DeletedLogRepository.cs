namespace Wlog.Library.BLL.Reporitories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Wlog.BLL.Entities;
    using Classes;
    using Interfaces;

    public class DeletedLogRepository : EntityRepository
    {
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
            using (IUnitOfWork uow = BeginUnitOfWork())
            {
                uow.BeginTransaction();
                uow.Delete(deletedLog);
                uow.Commit();
            }
        }

        public void Save(DeletedLogEntity deletedLog)
        {
            using (IUnitOfWork uow = BeginUnitOfWork())
            {
                uow.BeginTransaction();
                uow.SaveOrUpdate(deletedLog);
                uow.Commit();
            }
        }

        //TODO:it that the correct name? for what I understood from implementation this delete rows from bin
        public bool ExecuteMoveToBinJob(int daysToKeep, int rowsToKeep)
        {
            try
            {
                using (IUnitOfWork uow = BeginUnitOfWork())
                {
                    uow.BeginTransaction();
                    var entitiesToKeep = uow.Query<DeletedLogEntity>().Where(x => x.SourceDate > (DateTime.UtcNow.AddDays(-daysToKeep)))
                        .OrderByDescending(x => x.SourceDate).Take(rowsToKeep).ToList();
                    var entitiesToDelete= uow.Query<DeletedLogEntity>().Where(x => !entitiesToKeep.Contains(x)).ToList();

                    foreach (var entity in entitiesToDelete)
                    {
                        //TODO:this means you have a commit on each operation. When you have thousand deletion to do, what we need is to commit each x operation, to have better perfomance. X could be harcoded to 1000 atm.
                        RemoveDeletedLogEntity(entity);
                    }

                    return true;
                }
            }
            catch (Exception)
            {
                //TODO: please log something here.
                return false;
            }
        }
    }
}
