namespace Wlog.Library.BLL.Reporitories
{
    using System;
    using Wlog.BLL.Entities;
    using Classes;
    using Interfaces;
    using System.Collections.Generic;
    using System.Linq;

    public class JobInstanceRespository : EntityRepository<JobInstanceEntity>
    {
       

        public List<JobInstanceEntity> GetAllJobInstances()
        {
            return this.QueryOver(null);
        }

        

        public JobInstanceEntity GetJobInstanceByDefinitionAndCron(string cron, JobDefinitionEntity jobDefinition)
        {
            return this.FirstOrDefault(x => x.JobDefinitionID == jobDefinition.Id && x.CronExpression == cron);
        }
    }
}
