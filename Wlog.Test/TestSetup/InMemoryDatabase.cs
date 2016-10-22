namespace Wlog.Test.Tests
{
    using NHibernate;
    using NHibernate.Cfg;
    using NHibernate.Cfg.MappingSchema;
    using NHibernate.Dialect;
    using NHibernate.Driver;
    using NHibernate.Mapping.ByCode;
    using System;
    using System.Reflection;

    public class InMemoryDatabase<T> : IDisposable
    {
        private static Configuration _configuration;
        private static ISessionFactory _sessionFactory;
        protected ISession session;

        public InMemoryDatabase()
        {
            if (_configuration == null)
            {
                _configuration = new Configuration()
                    .SetProperty(NHibernate.Cfg.Environment.ReleaseConnections, "on_close")
                    .SetProperty(NHibernate.Cfg.Environment.Dialect, typeof(SQLiteDialect).AssemblyQualifiedName)
                    .SetProperty(NHibernate.Cfg.Environment.ConnectionDriver, typeof(SQLite20Driver).AssemblyQualifiedName)
                    .SetProperty(NHibernate.Cfg.Environment.ConnectionString, "data source=:memory:");

                _configuration.AddDeserializedMapping(GetMappings(), "NHSchemaTest");

                _sessionFactory = _configuration.BuildSessionFactory();
            }

            session = _sessionFactory.OpenSession();

            new NHibernate.Tool.hbm2ddl.SchemaExport(_configuration).Execute(true, true, false, session.Connection, Console.Out);
        }

        private static HbmMapping GetMappings()
        {
            var mapper = new ModelMapper();
            mapper.AddMappings(Assembly.GetAssembly(typeof(T)).GetExportedTypes());

            return mapper.CompileMappingForAllExplicitlyAddedEntities();
        }

        public void Dispose()
        {
            session.Dispose();
        }
    }
}
