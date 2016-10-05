using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wlog.BLL.Entities;
using Wlog.Library.BLL.Interfaces;
using Xunit;

namespace Wlog.Test.Tests
{
    public class JobsTests
    {
        private List<LogEntity> _logs;

        public JobsTests()
        {
            InsertLogsForTesting();
        }

        [Fact]
        public void MoveToBinJob_Success()
        {
         
        }

        public void InsertLogsForTesting()
        {
            _logs = new List<LogEntity>();
            _logs.Add(new LogEntity { CreateDate = DateTime.Now, Message = "Test Message 1", Uid = Guid.NewGuid() });
            _logs.Add(new LogEntity { CreateDate = DateTime.Now.AddDays(-10), Message = "Test Message 2", Uid = Guid.NewGuid() });
            _logs.Add(new LogEntity { CreateDate = DateTime.Now.AddDays(-10), Message = "Test Message 2", Uid = Guid.NewGuid() });
            _logs.Add(new LogEntity { CreateDate = DateTime.Now.AddDays(-10), Message = "Test Message 2", Uid = Guid.NewGuid() });
        }
    }
}
