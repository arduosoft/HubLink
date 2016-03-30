using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Wlog.BLL.Entities;
using Wlog.Library.BLL.Reporitories;

namespace Wlog.Test
{
    class Program
    {

  
        protected static IMongoClient client;
        protected static IMongoDatabase database;

       
        static void Main(string[] args)
        {

            RepositoryContext.Current.Users.GetById(new Guid("{82bb4691-17fa-9346-918c-45dadd4e045d}"));

            client = new MongoClient();
            database = client.GetDatabase("test");
            var collection = database.GetCollection<ApplicationEntity>("documents");

            collection.InsertOne(new ApplicationEntity()
            {
                ApplicationName = "PROVA",
                IdApplication = Guid.NewGuid(),
                EndDate = DateTime.Now,
                IsActive = true,
                PublicKey = new Guid(),
                StartDate = DateTime.Now
            });

           
        }
    }
}
