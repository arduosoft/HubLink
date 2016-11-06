using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wlog.Library.BLL.Reporitories;

namespace Wlog.Library.Scheduler.Jobs
{
    /// <summary>
    /// This job will take as input: #rtk=number of rows to keep, #dtk=number of days to keep.
    /// Only first #rtk records with SourceDate>today-#dtk have to been left into original table. 
    /// default values : #rtk:100 000 #dtk: 30 
    /// </summary>
    public class MoveToBinJob : Job
    {
        public MoveToBinJob()
        {
            //TODO: Take from configuration
            _rowsToKeep = 1000000;
            _daysToKeep = 30;
        }
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private int _rowsToKeep { get; set; }

        private int _daysToKeep { get; set; }

        public MoveToBinJob(int rowsToKeep, int daysToKeep)
        {
            _rowsToKeep = rowsToKeep;
            _daysToKeep = daysToKeep;
        }

        public override bool Execute()
        {
            try
            {
                return RepositoryContext.Current.Logs.ExecuteMoveToBinJob(_daysToKeep, _rowsToKeep);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);

                return false;
            }
        }
    }
}