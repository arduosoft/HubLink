using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;

namespace Wlog.Web.Code.Helpers
{
    public static class WlogLogger
    {
        private static Logger _logger;
        public static Logger Current
        {
            get
            {
                if (_logger == null)
                    _logger = LogManager.GetCurrentClassLogger();
                return _logger;
            }
        }
    }
}