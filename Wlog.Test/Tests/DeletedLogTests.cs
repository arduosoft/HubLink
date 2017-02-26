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

            using (var tx = Session.BeginTransaction())
            {
                id = Session.Save(new DeletedLogEntity
                {
                    Level = "1",
                    Message = message,
                    CreateDate = DateTime.Now
                });

                tx.Commit();
            }

            Session.Clear();


            using (var tx = Session.BeginTransaction())
            {
                var deletedLog = Session.Get<DeletedLogEntity>(id);

                Assert.Equal(deletedLog.Level, "1");
                Assert.Equal(deletedLog.Message, message);
                tx.Commit();
            }
        }
    }
}
