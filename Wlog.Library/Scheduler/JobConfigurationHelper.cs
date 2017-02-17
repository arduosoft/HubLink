using Hangfire;
using NLog;
using System;
using System.Diagnostics;
using System.Reflection;
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
            try
            {
                //var jobType = Type.GetType(jobConfiguration.FullClassName);
                //var instance = Activator.CreateInstance(jobType);
                //var job = new Hangfire.Common.Job(jobType, jobType.GetMethod("Execute"));
                //var manager = new RecurringJobManager();

                //manager.AddOrUpdate(jobConfiguration.JobInstanceId.ToString(), job, jobConfiguration.CronExpression);

                //var manager = new RecurringJobManager();
                Type jobType = Type.GetType(jobConfiguration.FullClassName);
                //MethodInfo methodInfo = jobType.GetMethod("Execute", BindingFlags.Instance | BindingFlags.Public);
                //var job = new Hangfire.Common.Job(jobType, methodInfo);
                //manager.AddOrUpdate(jobConfiguration.JobInstanceId.ToString(), job, jobConfiguration.CronExpression);
                var job = new Hangfire.Common.Job(jobType, jobType.GetMethod("Execute"));
                var manager = new RecurringJobManager();

                manager.AddOrUpdate(jobConfiguration.JobInstanceId.ToString(), job, jobConfiguration.CronExpression);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public static void TriggerJob(JobConfiguration jobConfiguration)
        {
            try
            {
                //var jobType = Type.GetType(jobConfiguration.FullClassName);
                //var instance = Activator.CreateInstance(jobType);
                //var method = jobType.GetMethod("Execute");

                //method.Invoke(instance, null);
                //var manager = new RecurringJobManager();
                //Type jobType = Type.GetType(jobConfiguration.FullClassName);
                //MethodInfo methodInfo = jobType.GetMethod("Execute", BindingFlags.Instance | BindingFlags.Public);
                //var job = new Hangfire.Common.Job(jobType, methodInfo);
                ////manager.AddOrUpdate(jdvm.Name, job, model.CronExpression);

                ////var jobType = Type.GetType(jobConfiguration.FullClassName);
                ////var instance = Activator.CreateInstance(jobType);
                ////var job = new Hangfire.Common.Job(jobType, jobType.GetMethod("Execute"));

                //BackgroundJob.Schedule( () => job.Method.Invoke(null, null), DateTimeOffset.Now);
             
                RecurringJob.Trigger(jobConfiguration.JobInstanceId.ToString());
                Debug.Write("TriggerJob");
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
