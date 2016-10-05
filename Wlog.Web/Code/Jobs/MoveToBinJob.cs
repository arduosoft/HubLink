using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wlog.Library.BLL.Reporitories;

namespace Wlog.Web.Code.Jobs
{
    /// <summary>
    /// This job will take as input: #rtk=number of rows to keep, #dtk=number of days to keep.
    /// Only first #rtk records with SourceDate>today-#dtk have to been left into original table. 
    /// default values : #rtk:100 000 #dtk: 30 
    /// </summary>
    public class MoveToBinJob : Job
    {
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
                var logsForBin = RepositoryContext.Current.Logs.GetLogsForBinJob(_daysToKeep, _rowsToKeep);

                if (logsForBin.Any())
                {
                    RepositoryContext.Current.Logs.MoveLogsToBin(logsForBin);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}