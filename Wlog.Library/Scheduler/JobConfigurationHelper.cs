using Hangfire;
using NLog;
using System;
using Wlog.Library.BLL.Classes;
using Wlog.Library.BLL.Reporitories;

namespace Wlog.Library.Scheduler
{
    public static class JobConfigurationHelper
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public static void LoadAllJobs()
        {
            try
            {
                var jobDefintions = RepositoryContext.Current.JobDefinition.GetAllDefinitionsAndInstances();

                foreach (var job in jobDefintions)
                {
                    if (job.Active)
                    {
                        _logger.Info($"[JobHelper]: Loading job {job.JobInstanceId}");
                        LoadJob(job);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public static void RemoveAllJobs()
        {
            try
            {
                var jobDefintions = RepositoryContext.Current.JobDefinition.GetAllDefinitionsAndInstances();

                foreach (var job in jobDefintions)
                {
                    _logger.Info($"[JobHelper]: Removing job {job.JobInstanceId}");
                    RecurringJob.RemoveIfExists(job.JobInstanceId.ToString());
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public static void ReloadJob(JobConfiguration jobConfiguration)
        {
            try
            {
                _logger.Info($"[JobHelper]: Reloading job {jobConfiguration.JobInstanceId}");
                RecurringJob.RemoveIfExists(jobConfiguration.JobInstanceId.ToString());
                LoadJob(jobConfiguration);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private static void LoadJob(JobConfiguration jobConfiguration)
        {
            var jobType = Type.GetType(jobConfiguration.FullClassName);
            var instance = Activator.CreateInstance(jobType);
            var method = jobType.GetMethod("Execute");
            RecurringJob.AddOrUpdate(jobConfiguration.JobInstanceId.ToString(), () => method.Invoke(instance, null), jobConfiguration.CronExpression);
        }
    }
}
