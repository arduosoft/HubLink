using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wlog.TestApp.Test
{
    public class FileTest:TestInstance
    {
        static Logger logger = LogManager.GetLogger("file");

        public void Execute()
        {
            logger.Info("THIS IS A SAMPLE MESSEGE TO LOG.");
        }
    }
}
