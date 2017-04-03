using NLog;
using System;
using Wlog.Library.BLL.Reporitories;
using Hangfire;

namespace Wlog.Library.Scheduler.Jobs
{
    /// <summary>
    /// This job physically delete entries, it will take as input: #dtk=number of days to keep.
    /// Records with SourceDate<today-#dtk will be deleted default values : #dtk: 10 
    /// </summary>
    public class EmptyBinJob : Job
    {
        private Logger _logger =LogManager.GetCurrentClassLogger();

        private int _rowsToKeep;
        private int _daysToKeep;

        public EmptyBinJob(int rowsToKeep, int daysToKeep)
        {
            _rowsToKeep = rowsToKeep;
            _daysToKeep = daysToKeep;
        }

        public EmptyBinJob()
        {
            _rowsToKeep = Settings.Default.EmptyBinJob_RowsToKeep;
            _daysToKeep = Settings.Default.EmptyBinJob_DaysToKeep;
        }

        [DisableConcurrentExecution(3600)]
        public override bool Execute()
        {
            try
            {
                return RepositoryContext.Current.DeletedLogs.ExecuteEmptyBinJob(_daysToKeep, _rowsToKeep);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);

                return false;
            }
        }
    }
}