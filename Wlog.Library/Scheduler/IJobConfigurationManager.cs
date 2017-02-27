using Wlog.Library.BLL.Classes;

namespace Wlog.Library.Scheduler
{
    public interface IJobConfigurationManager
    {
        void LoadAllJobsFromDatabase();
        bool UpdateJobInstance(JobConfiguration jobModel);
        bool RunJobInstance(JobConfiguration jobModel);
    }
}
