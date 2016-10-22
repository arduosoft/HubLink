namespace Wlog.Test.Tests
{
    using BLL.Entities;
    using System;
    using Xunit;

    public class DeletedLogTests : InMemoryDatabase<DeletedLogEntity>
    {

        [Fact, Trait("Category", "ExcludedFromCI")]
        public void DeletedLog_SaveAndLoad_Success()
        {
            object id;
            var message = "Message 1";

            using (var tx = session.BeginTransaction())
            {
                id = session.Save(new DeletedLogEntity
                {
                    Level = "1",
                    Message = message,
                    CreateDate = DateTime.Now
                });

                tx.Commit();
            }

            session.Clear();


            using (var tx = session.BeginTransaction())
            {
                var deletedLog = session.Get<DeletedLogEntity>(id);

                Assert.Equal(deletedLog.Level, "1");
                Assert.Equal(deletedLog.Message, message);
                tx.Commit();
            }
        }
    }
}
