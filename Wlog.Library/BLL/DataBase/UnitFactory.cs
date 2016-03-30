using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wlog.Library.BLL.Configuration;
using Wlog.Library.BLL.Interfaces;

namespace Wlog.Library.BLL.DataBase
{
    public class UnitFactory
    {
        private static Dictionary<string, Type> factory;

        public UnitFactory() 
        {
            if (factory==null || factory.Count == 0)
            {
                RepositoryConfiguration configuration = RepositoryConfiguration.GetConfig();
                RepositoryCollection col = configuration.RepositoryCollection;
                IEnumerable<Repository> Repo = col.Cast<Repository>();
                factory = new Dictionary<string, Type>();
                foreach (Repository item in Repo)
                {
                    Type type = Type.GetType(item.DataBase);
                    factory.Add(item.RepositoryName, type);
                }
            }
        }

        public IUnitOfWork GetUnit(IRepository repo)
        {
            Type tipo = factory[repo.GetType().Name];
            return (IUnitOfWork)Activator.CreateInstance(tipo);
        }
    }
}
