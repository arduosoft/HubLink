using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wlog.TestApp.Test
{
    public class WlogTest : TestInstance
    {
        static Logger logger = LogManager.GetLogger("wlog.logger");

        public override void Execute()
        {
            logger.Info("THIS IS A SAMPLE MESSEGE TO LOG.");
        }
    }
}
