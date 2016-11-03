using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using Hangfire;
using Hangfire.MemoryStorage;
using Wlog.BLL.Classes;
using Wlog.Library.Scheduler.Jobs;

namespace Wlog.Library.Scheduler
{
    public class HangfireBootstrapper : IRegisteredObject
    {
        public static readonly HangfireBootstrapper Instance = new HangfireBootstrapper();

        private readonly object _lockObject = new object();
        private bool _started;

        private BackgroundJobServer _backgroundJobServer;

        private HangfireBootstrapper()
        {
        }

     
        public void Start()
        {
            lock (_lockObject)
            {
                if (_started) return;
                _started = true;

                HostingEnvironment.RegisterObject(this);


                JobStorage.Current = new MemoryStorage();
                RecurringJob.AddOrUpdate(() => LogQueue.Current.Run(), "*/1 * * * *");
                RecurringJob.AddOrUpdate(() => new MoveToBinJob(100,100).Execute(), "0 */1 * * *");


                _backgroundJobServer = new BackgroundJobServer();
            }
        }

        public void Stop()
        {
            lock (_lockObject)
            {
                if (_backgroundJobServer != null)
                {
                    _backgroundJobServer.Dispose();
                }

                HostingEnvironment.UnregisterObject(this);
            }
        }


        

        void IRegisteredObject.Stop(bool immediate)
        {
            Stop();
        }
    }
}
