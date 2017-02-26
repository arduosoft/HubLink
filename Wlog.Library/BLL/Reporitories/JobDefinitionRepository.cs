namespace Wlog.Library.BLL.Reporitories
{
    using System;
    using Wlog.BLL.Entities;
    using Classes;
    using Interfaces;
    using System.Collections.Generic;
    using System.Linq;

    public class JobDefinitionRepository : EntityRepository<JobDefinitionEntity>
    {
      

        public List<JobDefinitionEntity> GetAllJobDefinitions()
        {
            return this.QueryOver(null);
        }

        public IEnumerable<JobConfiguration> GetAllDefinitionsAndInstances()
        {
            var jobDefintions = GetAllJobDefinitions();
            var jobInstances = RepositoryContext.Current.JobInstance.GetAllJobInstances();

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
                              FullClassName = a.FullClassname,
                              Instantiable = a.Instantiable
                          }).ToList();

            return result;
        }

        public JobDefinitionEntity GetJobDefinitionByName(string jobName)
        {
            return this.FirstOrDefault(x => jobName.Equals(x.Name));
        }
    }
}
