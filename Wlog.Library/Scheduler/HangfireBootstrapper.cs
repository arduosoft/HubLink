using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using Hangfire;
using Hangfire.MemoryStorage;
using NLog;
using Wlog.BLL.Classes;
using Wlog.Library.Scheduler.Jobs;

namespace Wlog.Library.Scheduler
{

    /// <summary>
    /// Class to manage insternal scheduler
    /// </summary>
    public class HangfireBootstrapper : IRegisteredObject
    {
        public static readonly HangfireBootstrapper Instance = new HangfireBootstrapper();

        private readonly object _lockObject = new object();
        private bool _started;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private BackgroundJobServer _backgroundJobServer;

        private HangfireBootstrapper()
        {
        }

     
        public void Start()
        {
            logger.Info("[HangfireBootstrapper]: Starting hangfire");
            lock (_lockObject)
            {
                logger.Debug("[HangfireBootstrapper]: Starting hangfire (access acquired)");
                if (_started)
                {
                    logger.Debug("[HangfireBootstrapper]: already up and runnig, nothing to do here");
                    return;
                }

                logger.Debug("[HangfireBootstrapper]: not already  runnig, starting up");
                _started = true;

                logger.Debug("[HangfireBootstrapper]:  HostingEnvironment.RegisterObject");
                HostingEnvironment.RegisterObject(this);

                logger.Debug("[HangfireBootstrapper]:  Setting up Job storage (Memory sorage hardcoded)");
                JobStorage.Current = new MemoryStorage();
                logger.Debug("[HangfireBootstrapper]:  Registering jobs (hard coded)");
                RecurringJob.AddOrUpdate(() => LogQueue.Current.Run(), "*/1 * * * *");
                JobConfigurationHelper.LoadAllJobs();

                logger.Debug("[HangfireBootstrapper]: starting BackgroundJobServer");
                _backgroundJobServer = new BackgroundJobServer();
            }
        }

        public void Stop()
        {
            logger.Info("[HangfireBootstrapper]: Stopping hangfire");
            lock (_lockObject)
            {
                logger.Debug("[HangfireBootstrapper]: Stopping hangfire (session acquired");
                if (_backgroundJobServer != null)
                {
                    logger.Debug("[HangfireBootstrapper]: dispose _backgroundJobServer");
                    _backgroundJobServer.Dispose();
                }
                logger.Debug("[HangfireBootstrapper]: unregister  HostingEnvironment.RegisterObject");
                HostingEnvironment.UnregisterObject(this);
            }
        }

        void IRegisteredObject.Stop(bool immediate)
        {
            logger.Debug("[HangfireBootstrapper]: forse stop on pool hangout");
            Stop();
        }
    }
}
