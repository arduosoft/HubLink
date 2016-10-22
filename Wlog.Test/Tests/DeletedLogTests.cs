namespace Wlog.Test.Tests
{
    using BLL.Entities;
    using Library.BLL.Interfaces;
    using Library.DAL.Nhibernate.Mappings;
    using NHibernate;
    using System;
    using System.Collections.Generic;
    using System.Data.SQLite;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using Wlog.Library.BLL.Reporitories;
    using Xunit;

    public class DeletedLogTests : InMemoryDatabase<DeletedLogEntity>
    {

        [Fact]
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
