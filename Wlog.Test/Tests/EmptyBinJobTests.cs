namespace Wlog.Test.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BLL.Entities;
    using Library.BLL.Reporitories;
    using Web.Code.Jobs;
    using Xunit;

    public class EmptyBinJobTests 
    {
        private readonly List<DeletedLogEntity> _logs = new List<DeletedLogEntity>();

        public EmptyBinJobTests()
        {
            InsertTestLogsInDB();
            ExecuteEmptyBinJob();
        }

        private void InsertTestLogsInDB()
        {
            _logs.Add(new DeletedLogEntity
            {
                LogId = Guid.NewGuid(),
                CreateDate = DateTime.Now.AddDays(-40),
                SourceDate = DateTime.UtcNow,
                UpdateDate = DateTime.Now,
                Message = "Message 1",
                DeletedOn = DateTime.UtcNow
            });
            _logs.Add(new DeletedLogEntity
            {
                LogId = Guid.NewGuid(),
                CreateDate = DateTime.Now.AddDays(-40),
                SourceDate = DateTime.UtcNow.AddDays(-48),
                UpdateDate = DateTime.Now,
                Message = "Message 2",
                DeletedOn = DateTime.UtcNow
            });
            _logs.Add(new DeletedLogEntity
            {
                LogId = Guid.NewGuid(),
                CreateDate = DateTime.Now.AddDays(-40),
                SourceDate = DateTime.UtcNow.AddDays(-60),
                UpdateDate = DateTime.Now,
                Message = "Message 3",
                DeletedOn = DateTime.UtcNow
            });

            foreach (var log in _logs)
            {
                RepositoryContext.Current.DeletedLogs.Save(log);
            }
        }

        private static void ExecuteEmptyBinJob()
        {
            var emptyBin = new EmptyBinJob(1, 30);
            emptyBin.Execute();
        }


        [Fact, Trait("Category", "ExcludedFromCI")]
        public void MoveToBinJob_CheckLogsTable_Success()
        {
            var allLogs = RepositoryContext.Current.DeletedLogs.GetAllDeletedLogEntities();

            Assert.True(allLogs.Any(x => x.Uid == _logs[0].Uid));
            Assert.False(allLogs.Any(x => x.Uid == _logs[1].Uid));
            Assert.False(allLogs.Any(x => x.Uid == _logs[2].Uid));
        }
    }
}
