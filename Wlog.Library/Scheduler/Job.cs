using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wlog.Library.Scheduler
{

    /// <summary>
    /// Abstrat class that each job should implement
    /// </summary>
    public abstract class Job
    {
        public abstract bool Execute();
    }
}