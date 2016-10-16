namespace Wlog.Library.BLL.Reporitories
{
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

    public class DeletedLogRepository : EntityRepository
    {

        public List<DeletedLogEntity> GetAllDeletedLogEntities()
        {
            List<DeletedLogEntity> result = new List<DeletedLogEntity>();
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

        /// </summary>
        /// <param name="daysToKeep"></param>
        /// <param name="rowsToKeep"></param>
        /// <returns></returns>
        public List<DeletedLogEntity> GetLogsForEmptyBinJob(int daysToKeep, int rowsToKeep)
        {
            using (IUnitOfWork uow = BeginUnitOfWork())
            {
                uow.BeginTransaction();
                var entitiesToKeep = uow.Query<DeletedLogEntity>().Where(x => x.SourceDate > (DateTime.UtcNow.AddDays(-daysToKeep)))
                    .OrderByDescending(x => x.SourceDate).Take(rowsToKeep).ToList();
                return uow.Query<DeletedLogEntity>().Where(x => !entitiesToKeep.Contains(x)).ToList();
            }
        }
    }
}
