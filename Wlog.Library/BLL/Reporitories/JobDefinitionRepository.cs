namespace Wlog.Library.BLL.Reporitories
{
    using System;
    using Wlog.BLL.Entities;
    using Classes;
    using Interfaces;
    using System.Collections.Generic;
    using System;
    using System.Linq;

    public class JobDefinitionRepository : EntityRepository
    {
        public bool Save(JobDefinitionEntity entity)
        {
            logger.Debug("[repo] JobDefinitionEntity entering Save");

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

        public List<JobDefinitionEntity> GetAllJobs()
        {
            var jobDefinitions = new List<JobDefinitionEntity>();

            try
            {
                using (IUnitOfWork uow = BeginUnitOfWork())
                {
                    uow.BeginTransaction();
                    jobDefinitions = uow.Query<JobDefinitionEntity>().Select(x => x).ToList();
                    uow.Commit();
                }
            }
            catch (Exception err)
            {
                logger.Error(err);
            }

            return jobDefinitions;
        }

        public IEnumerable<JobConfiguration> GetAllDefinitionsAndInstances()
        {
            var jobDefintions = GetAllJobs();
            var jobInstances = RepositoryContext.Current.JobInstance.GetAllJobs();

            var result = (from a in jobDefintions
                          join b in jobInstances
                          on a.Id equals b.JobDefinitionID
                          select new JobConfiguration
                          {
                              Active = b.Active,
                              CronExpression = b.CronExpression,
                              JobName = a.Name,
                              JobInstanceId = b.Id,
                              Description = a.Description,
                              FullClassName = a.FullClassname
                          }).ToList();

            return result;
        }

        public JobDefinitionEntity GetJobDefinitionByName(string jobName)
        {
            JobDefinitionEntity jobDefinition = null;

            try
            {
                using (IUnitOfWork uow = BeginUnitOfWork())
                {
                    uow.BeginTransaction();
                    jobDefinition = uow.Query<JobDefinitionEntity>().FirstOrDefault(x => x.Name == jobName);
                    uow.Commit();
                }
            }
            catch (Exception err)
            {
                logger.Error(err);
            }

            return jobDefinition;
        }
    }
}
