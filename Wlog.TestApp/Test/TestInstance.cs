using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wlog.TestApp.Test
{
    public abstract class TestInstance
    {
        public double Avg { get; set; }
        public abstract void Execute();
    }
}
