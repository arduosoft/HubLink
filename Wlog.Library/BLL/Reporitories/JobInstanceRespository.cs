namespace Wlog.Library.BLL.Reporitories
{
    using System;
    using Wlog.BLL.Entities;
    using Classes;
    using Interfaces;
    using System.Collections.Generic;
    using System.Linq;

    public class JobInstanceRespository : EntityRepository
    {
        public bool Save(JobInstanceEntity entity)
        {
            logger.Debug("[repo] JobInstanceEntity entering Save");

            try
            {

                using (IUnitOfWork uow = BeginUnitOfWork())
                {
                    uow.BeginTransaction();
                    uow.SaveOrUpdate(entity);
                    uow.Commit();
                }

                return true;
            }
            catch (Exception err)
            {
                logger.Error(err);
            }

            return false;
        }

        public List<JobInstanceEntity> GetAllJobs()
        {
            var jobInstances = new List<JobInstanceEntity>();

            try
            {
                using (IUnitOfWork uow = BeginUnitOfWork())
                {
                    uow.BeginTransaction();
                    jobInstances = uow.Query<JobInstanceEntity>().Select(x => x).ToList();
                    uow.Commit();
                }
            }
            catch (Exception err)
            {
                logger.Error(err);
            }

            return jobInstances;
        }
    }
}
