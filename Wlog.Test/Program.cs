using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Wlog.BLL.Entities;

namespace Wlog.Test
{
    class Program
    {

        protected static IMongoClient client;
        protected static IMongoDatabase database;

       
        static void Main(string[] args)
        {
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
