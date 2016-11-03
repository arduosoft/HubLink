using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wlog.Library.Scheduler
{
    public abstract class Job
    {
        public abstract bool Execute();
    }
}