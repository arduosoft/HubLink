using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wlog.Web.Code.Jobs
{
    public abstract class Job
    {
        public abstract bool Execute();
    }
}