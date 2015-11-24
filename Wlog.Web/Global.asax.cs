using Hangfire;
using Hangfire.MemoryStorage;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Wlog.Web.Code.Classes;

namespace Wlog.Web
{
    // Nota: per istruzioni su come abilitare la modalità classica di IIS6 o IIS7, 
    // visitare il sito Web all'indirizzo http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        //Todo: Move nhibernate init stuff in a dedicated class. Implement a DataContext or Operation manager pattern to wrap data access
        public static ISessionFactory CurrentSessionFactory;
        public static Configuration cfg;
        private BackgroundJobServer _backgroundJobServer;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(System.Web.Http.GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            
           cfg  = new Configuration();
          
           

           



            ModelMapper mapper = new ModelMapper();
            mapper.AddMappings(Assembly.GetExecutingAssembly().GetExportedTypes());
            HbmMapping domainMapping =
              mapper.CompileMappingForAllExplicitlyAddedEntities();
            cfg.AddMapping(domainMapping);


            cfg.Configure();
            CurrentSessionFactory = cfg.BuildSessionFactory();

            //TODO: Expot this snippest inside DbUpgrader util class, and make it conservative ( upgrades musk apply only the delta)
            SchemaMetadataUpdater.QuoteTableAndColumns(cfg);
            NHibernate.Tool.hbm2ddl.SchemaExport schema = new NHibernate.Tool.hbm2ddl.SchemaExport(cfg);

            schema.Create(false, true);
            
            JobStorage.Current=new MemoryStorage();
            //Hangfire.GlobalConfiguration.Configuration.UseNLogLogProvider();
            _backgroundJobServer = new BackgroundJobServer();
           
      
            
            

            BackgroundJob.Schedule(() => LogQueue.Current.Run(), TimeSpan.FromSeconds(1));


        }

        protected void Application_End(object sender, EventArgs e)
        {
            _backgroundJobServer.Dispose();
        }
    }
}