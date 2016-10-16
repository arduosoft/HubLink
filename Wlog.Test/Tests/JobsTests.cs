namespace Wlog.Test.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BLL.Entities;
    using Library.BLL.Reporitories;
    using Web.Code.Jobs;
    using Xunit;

    public class JobsTests : IDisposable
    {
        private List<LogEntity> _logs;

        public JobsTests()
        {
            _logs = new List<LogEntity>();

            // insert test logs in DB
            InsertTestLogsInDB();

            // run MoveToBinJob
            ExecuteMoveToBin();
        }

        private void InsertTestLogsInDB()
        {
            _logs.Add(new LogEntity { CreateDate = DateTime.Now.AddDays(-40), SourceDate = DateTime.Now, UpdateDate = DateTime.Now, Message = "Message 1" });
            _logs.Add(new LogEntity { CreateDate = DateTime.Now.AddDays(-40), SourceDate = DateTime.Now.AddDays(-48), UpdateDate = DateTime.Now, Message = "Message 2" });
            _logs.Add(new LogEntity { CreateDate = DateTime.Now.AddDays(-40), SourceDate = DateTime.Now.AddDays(-60), UpdateDate = DateTime.Now, Message = "Message 3" });

            foreach (var log in _logs)
            {
                RepositoryContext.Current.Logs.Save(log);
            }
        }

        private void ExecuteMoveToBin()
        {
            MoveToBinJob moveToBin = new MoveToBinJob(100000, 30);
            moveToBin.Execute();
        }


        [Fact]
        public void MoveToBinJob_CheckLogsTable_Success()
        {
            var allLogs = RepositoryContext.Current.Logs.GetAllLogEntities();

            Assert.True(allLogs.Any(x => x.Uid == _logs[0].Uid));
            Assert.False(allLogs.Any(x => x.Uid == _logs[1].Uid));
            Assert.False(allLogs.Any(x => x.Uid == _logs[2].Uid));
        }

        [Fact]
        public void MoveToBinJob_CheckDeletedLogsTable_Success()
        {
            var allDeletedLogs = RepositoryContext.Current.Logs.GetAllDeletedLogEntities();

            Assert.False(allDeletedLogs.Any(x => x.Uid == _logs[0].Uid));
            Assert.True(allDeletedLogs.Any(x => x.LogId == _logs[1].Uid));
            Assert.True(allDeletedLogs.Any(x => x.LogId == _logs[2].Uid));
        }

        /// <summary>
        /// Clean Up after the UnitTest
        /// </summary>
        public void Dispose()
        {
            // get deleted logs for clean up
            var deletedLogs = RepositoryContext.Current.Logs.GetAllDeletedLogEntities().Where(x => _logs.Any(u => u.Uid == x.LogId));

            RepositoryContext.Current.Logs.RemoveLogEntity(_logs[0]);

            if (deletedLogs.Any())
            {
                // if tests passed clean up the deleted Log table
                foreach (var deletedLog in deletedLogs)
                {
                    RepositoryContext.Current.Logs.RemoveDeletedLogEntity(deletedLog);
                }
            }
            else
            {
                // if tests failed and nothing iserted
                RepositoryContext.Current.Logs.RemoveLogEntity(_logs[1]);
                RepositoryContext.Current.Logs.RemoveLogEntity(_logs[2]);
            }
        }
    }
}
