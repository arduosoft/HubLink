using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wlog.Web.Code.Jobs
{
    public class EmptyBinJob : Job
    {
        public override bool Execute()
        {
            throw new NotImplementedException();
        }
    }
}