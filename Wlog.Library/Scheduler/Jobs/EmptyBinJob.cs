using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wlog.Library.BLL.Reporitories;

namespace Wlog.Library.Scheduler.Jobs
{
    /// <summary>
    /// This job physically delete entries, it will take as input: #dtk=number of days to keep.
    /// Records with SourceDate<today-#dtk will be deleted default values : #dtk: 10 
    /// </summary>
    public class EmptyBinJob : Job
    {
        private Logger _logger => LogManager.GetCurrentClassLogger();

        private int _rowsToKeep { get; set; }

        private int _daysToKeep { get; set; }

        public EmptyBinJob(int rowsToKeep, int daysToKeep)
        {
            _rowsToKeep = rowsToKeep;
            _daysToKeep = daysToKeep;
        }

        public EmptyBinJob()
        {
            _rowsToKeep = 1000000;
            _daysToKeep = 30;
        }

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