using Hangfire;
using NLog;
using System;
using Wlog.Library.BLL.Classes;
using Wlog.Library.BLL.Reporitories;

namespace Wlog.Library.Scheduler
{
    public class JobConfigurationManager : IJobConfigurationManager
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public void LoadAllJobsFromDatabase()
        {
            try
            {
                var jobDefintions = RepositoryContext.Current.JobDefinition.GetAllDefinitionsAndInstances();

                foreach (var job in jobDefintions)
                {
                    if (job.Active)
                    {
                        _logger.Info("[JobConfigurationManager]: Loading job " +job.JobInstanceId);
                        LoadJob(job);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        public bool UpdateJobInstance(JobConfiguration jobModel)
        {
            try
            {
                var jobInstance = RepositoryContext.Current.JobInstance.GetById(jobModel.JobInstanceId);

                if (jobInstance == null)
                {
                    return false;
                }

                jobInstance.Active = jobModel.Active;

                // deactivating job
                if (!jobModel.Active)
                {
                    jobInstance.DeactivationDate = DateTime.UtcNow;
                }

                jobInstance.CronExpression = jobModel.CronExpression;

                RepositoryContext.Current.JobInstance.Save(jobInstance);

                ReloadJob(jobModel);
                _logger.Info("[JobConfigurationManager]: Updated job "+jobModel.JobInstanceId);

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        public bool RunJobInstance(JobConfiguration jobModel)
        {
            try
            {
                bool updated = UpdateJobInstance(jobModel);
                if (updated && jobModel.Instantiable)
                {
                    TriggerJob(jobModel);
                    _logger.Info("[JobConfigurationManager]: Run job "+jobModel.JobInstanceId);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        private void ReloadJob(JobConfiguration jobConfiguration)
        {
            try
            {
                _logger.Info("[JobConfigurationManager]: Reloading job "+jobConfiguration.JobInstanceId);
                RecurringJob.RemoveIfExists(jobConfiguration.JobInstanceId.ToString());
                LoadJob(jobConfiguration);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        private void LoadJob(JobConfiguration jobConfiguration)
        {
            _logger.Info("[JobConfigurationManager]: Loading job "+jobConfiguration.JobInstanceId);
            var jobType = Type.GetType(jobConfiguration.FullClassName);
            var job = new Hangfire.Common.Job(jobType, jobType.GetMethod("Execute"));
            var manager = new RecurringJobManager();

            manager.AddOrUpdate(jobConfiguration.JobInstanceId.ToString(), job, jobConfiguration.CronExpression);
        }

        private void TriggerJob(JobConfiguration jobConfiguration)
        {
            _logger.Info("[JobConfigurationManager]: Trigger job "+jobConfiguration.JobInstanceId);
            RecurringJob.Trigger(jobConfiguration.JobInstanceId.ToString());
        }
    }
}
