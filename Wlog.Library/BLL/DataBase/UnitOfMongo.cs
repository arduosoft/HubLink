using MongoDB.Driver;
using System;
using System.Collections.Generic;
using MongoDB.Driver.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wlog.Library.BLL.Interfaces;
using MongoDB.Bson;
using Wlog.BLL.Entities;

namespace Wlog.Library.BLL.DataBase
{
    internal class UnitOfMongo:IUnitOfWork
    {
        private IMongoDatabase database;
        private IMongoCollection<IEntityBase> collection;

        public void Dispose()
        {

        }

        public UnitOfMongo()
        {
            BeginTransaction();
        }

        public void BeginTransaction()
        {
            database = MongoContext.Current.DataBase;
        }

        public void Commit()
        {
            
        }


        public void SaveOrUpdate(IEntityBase entity)
        {

            collection = GetCollection(entity);
            if (entity.Id == null || entity.Id==(new Guid("{00000000-0000-0000-0000-000000000000}")))
            {
                collection.InsertOne(entity);
            }
            else
            {
                FilterDefinition<IEntityBase> filter = Builders<IEntityBase>.Filter.Eq(x => x.Id, entity.Id);
                ReplaceOneResult result = collection.ReplaceOne(filter, entity);
            }
        }

        public void Delete(IEntityBase entity)
        {
            collection = GetCollection(entity);
            collection.DeleteOne(x=>x.Id==entity.Id);
        }

        public IQueryable<T> Query<T>()
        {

            return database.GetCollection<T>(typeof(T).Name).AsQueryable<T>();
        }

        private IMongoCollection<IEntityBase> GetCollection(IEntityBase entity)
        {
            Type type = entity.GetType();
            return collection = database.GetCollection<IEntityBase>(type.Name);
        }
    }
}
