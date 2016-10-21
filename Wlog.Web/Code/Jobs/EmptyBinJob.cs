using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wlog.Library.BLL.Reporitories;

namespace Wlog.Web.Code.Jobs
{
    /// <summary>
    /// This job physically delete entries, it will take as input: #dtk=number of days to keep.
    /// Records with SourceDate<today-#dtk will be deleted default values : #dtk: 10 
    /// </summary>
    public class EmptyBinJob : Job
    {
        private int _rowsToKeep { get; set; }

        private int _daysToKeep { get; set; }

        public EmptyBinJob(int rowsToKeep, int daysToKeep)
        {
            _rowsToKeep = rowsToKeep;
            _daysToKeep = daysToKeep;
        }

        public override bool Execute()
        {
            try
            {
                return RepositoryContext.Current.DeletedLogs.ExecuteMoveToBinJob(_daysToKeep, _rowsToKeep);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}