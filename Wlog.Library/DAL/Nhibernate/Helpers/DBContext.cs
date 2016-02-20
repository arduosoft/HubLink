using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Wlog.DAL.NHibernate.Helpers
{
    internal class DBContext
    {
        /// <summary>
        /// Get a configuration using settings in web.config and map entities in this assembly
        /// </summary>
        /// <returns></returns>
        public static Configuration GetConfiguration()
        {

            Configuration cfg = new Configuration();

            ModelMapper mapper = new ModelMapper();
            mapper.AddMappings(Assembly.GetExecutingAssembly().GetExportedTypes());

            HbmMapping domainMapping =
              mapper.CompileMappingForAllExplicitlyAddedEntities();
            cfg.AddMapping(domainMapping);
            cfg.Configure();

            return cfg;
        }

        public Configuration Configuration { get; set; }
        public ISessionFactory SessionFactory { get; set; }

        private static DBContext CreateNewContext()
        {
            DBContext ctx = new DBContext();
            ctx.Configuration = GetConfiguration();
            ctx.SessionFactory = ctx.Configuration.BuildSessionFactory();
            return ctx;
        }

        public static void ApplySchemaChanges()
        {
            Configuration cfg = GetConfiguration();

            SchemaMetadataUpdater.QuoteTableAndColumns(cfg);
            //NHibernate.Tool.hbm2ddl.SchemaExport schema = new NHibernate.Tool.hbm2ddl.SchemaExport(cfg);
            
            //schema.Create(false, true);
            var update = new SchemaUpdate(cfg);
            
            update.Execute(true, true);

        }


        private static DBContext _Current;
        public static DBContext Current
        {
            get
            {
                return _Current ?? (_Current = CreateNewContext());
            }
        }

    }
}