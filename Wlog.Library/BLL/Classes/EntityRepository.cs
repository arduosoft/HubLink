using System;
using NLog;
using NLog.Config;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wlog.Library.BLL.DataBase;
using Wlog.Library.BLL.Interfaces;
using System.Linq.Expressions;
using Wlog.Library.BLL.Enums;
using PagedList;

namespace Wlog.Library.BLL.Classes
{
    /// <summary>
    /// Repository to store entities
    /// </summary>
    public abstract class EntityRepository<T> : IRepository where T : IEntityBase
    {
        private static UnitFactory unitFactory;

        public static  Logger logger { get { return LogManager.GetCurrentClassLogger(); } }

        public IUnitOfWork BeginUnitOfWork()
        {
            // I don't check if unitFactory is null prior to initialization since it's already done within its constructor
            unitFactory = new UnitFactory();
            return unitFactory.GetUnit(this);
        }

        public  Type GetEntityType()
        {
            return typeof(T);
        }

        public virtual T GetById(Guid id)
        {
            using (var uow = BeginUnitOfWork())
            {
                uow.BeginTransaction();
                return uow.Query<T>().FirstOrDefault(x => x.Id.Equals(id));
            }
        }

        /// <summary>
        /// Get a list of rows
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual List<T> QueryOver(Expression<Func<T, bool>> where)
        {
            return QueryOver(where, 0, 0, null, SortDirection.ASC);
        }

        /// <summary>
        /// Perform a full paged  query using lambda
        /// </summary>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="numberOfRow"></param>
        /// <param name="sortField"></param>
        /// <param name="sordDirection"></param>
        /// <returns></returns>
        public virtual List<T> QueryOver(Expression<Func<T,bool>> where, int startIndex, int numberOfRow, Expression<Func<T,object>> sortField, SortDirection sordDirection )
        {
            using (var uow = BeginUnitOfWork())
            {
                uow.BeginTransaction();
                IQueryable<T> query = GetQuery(where, startIndex, numberOfRow, sortField, sordDirection, uow);

                return query.ToList();// ToList have to be called inside  using (var uow = BeginUnitOfWork())
            }


        }


        public virtual IPagedList<T> Find(Expression<Func<T, bool>> where, int pageNumber, int pageSize, Expression<Func<T, object>> sortField, SortDirection sordDirection)
        {
            using (var uow = BeginUnitOfWork())
            {
                uow.BeginTransaction();
                IQueryable<T> query = GetQuery(where, pageNumber* pageSize, (pageNumber * pageSize)+ pageSize, sortField, sordDirection, uow);
                IQueryable<T> count = GetQuery(where, 0, 0, null, SortDirection.ASC, uow);//this avoid restrictions.
                int rowCount = count.Count();
                List<T> result = query.ToList();

                return new StaticPagedList<T>(result, pageNumber, pageSize, rowCount);
            }


        }

       
        /// <summary>
        /// give the count of record
        /// </summary>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="numberOfRow"></param>
        /// <param name="sortField"></param>
        /// <param name="sordDirection"></param>
        /// <returns></returns>
        public virtual int Count(Expression<Func<T, bool>> where)
        {
            using (var uow = BeginUnitOfWork())
            {
                uow.BeginTransaction();
                IQueryable<T> query = GetQuery(where, 0, 0, null, SortDirection.ASC, uow);

                return query.Count();// ToList have to be called inside  using (var uow = BeginUnitOfWork())
            }


        }

        /// <summary>
        /// Get first result for the query
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual T FirstOrDefault(Expression<Func<T, bool>> where)
        {
            return FirstOrDefault(where, null, SortDirection.ASC);
        }
        /// <summary>
        /// give the count of record
        /// </summary>
        /// <param name="where"></param>
        /// <param name="startIndex"></param>
        /// <param name="numberOfRow"></param>
        /// <param name="sortField"></param>
        /// <param name="sordDirection"></param>
        /// <returns></returns>
        public virtual T FirstOrDefault(Expression<Func<T, bool>> where, Expression<Func<T, object>> sortField, SortDirection sordDirection)
        {
            using (var uow = BeginUnitOfWork())
            {
                uow.BeginTransaction();
                IQueryable<T> query = GetQuery(where, 0, 0, sortField, sordDirection, uow);//0 force to do not apply take or skip
                return query.FirstOrDefault();// ToList have to be called inside  using (var uow = BeginUnitOfWork())
            }


        }

        private static IQueryable<T> GetQuery(Expression<Func<T, bool>> where, int startIndex, int numberOfRow, Expression<Func<T, object>> sortField, SortDirection sordDirection, IUnitOfWork uow)
        {
            var query = uow.Query<T>();
            if (where != null)
            {
                query = query.Where(where);
            }
            if (startIndex > 0)
            {
                query = query.Skip(startIndex);
            }

            if (numberOfRow > 0)
            {
                query = query.Skip(numberOfRow);
            }

            if (sortField != null && sordDirection == SortDirection.ASC)
            {
                query = query.OrderBy(sortField);

            }
            else if (sortField != null && sordDirection == SortDirection.DESC)
            {
                query = query.OrderByDescending(sortField);
            }

            return query;
        }

        

        public virtual  bool Save(T entityToSave)
        {
           return  Save(entityToSave, false);
        }

        public virtual bool Save(T entityToSave, bool trowOnError)
        {
            using (var uow = BeginUnitOfWork())
            {
                try
                {
                    uow.BeginTransaction();

                    uow.SaveOrUpdate(entityToSave);
                    uow.Commit();
                    return true;
                }
                catch (Exception err)
                {
                    logger.Error(err);
                    //transaction will be automatically rollbacked
                    if (trowOnError) throw err;
                    return false;
                }
            }
        }

        public virtual bool Delete(T entityToSave)
        {
            return Delete(entityToSave, false);
        }

        public virtual bool Delete(T entityToSave, bool trowOnError)
        {
            using (var uow = BeginUnitOfWork())
            {
                try
                {
                    uow.BeginTransaction();

                    uow.Delete(entityToSave);
                    uow.Commit();
                    return true;
                }
                catch (Exception err)
                {
                    logger.Error(err);
                    //transaction will be automatically rollbacked
                    if (trowOnError) throw err;
                    return false;
                }
            }
        }


        }
}
