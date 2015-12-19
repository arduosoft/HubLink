using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Linq;
using Wlog.Web.Code.Helpers;


namespace Wlog.Web.Code.Classes
{
    public interface IUnitOfWork
    {
        void BeginTransaction();
        void Commit();
    }

    public class UnitOfWork : IUnitOfWork,IDisposable
    {
        
        private ITransaction _transaction;

        public ISession Session { get; private set; }

        bool _uncommitted = true;

        

        public UnitOfWork()
        {
            Session = DBContext.Current.SessionFactory.OpenSession();
            BeginTransaction();
        }

        public void BeginTransaction()
        {
            _transaction = Session.BeginTransaction();
            _uncommitted = true;
        }

        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
            finally
            {
               
            }

            _uncommitted = false;
        }

        public void Dispose()
        {
            try
            {
                _transaction.Rollback();
            }
            catch
            {
            }
            finally
            {
                try
                {
                    Session.Close();
                }
                catch { }
            }
        }

        public void SaveOrUpdate(object entity)
        {
            Session.SaveOrUpdate(entity);
        }

        public IQueryable<T> Query<T>()
        {
            return Session.Query<T>();
        }
    }
}