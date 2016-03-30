using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wlog.Library.BLL.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        void BeginTransaction();
        void Commit();
        void SaveOrUpdate(IEntityBase entity);
        void Delete(IEntityBase entity);
        IQueryable<T> Query<T>();
    }
}
