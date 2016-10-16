using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wlog.Web.Code.Jobs
{
    /// <summary>
    /// This job physically delete entries, it will take as input: #dtk=number of days to keep.
    /// Records with SourceDate<today-#dtk will be deleted default values : #dtk: 10 
    /// </summary>
    public class EmptyBinJob : Job
    {
        public override bool Execute()
        {
            throw new NotImplementedException();
        }
    }
}