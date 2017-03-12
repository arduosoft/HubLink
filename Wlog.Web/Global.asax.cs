﻿//******************************************************************************
// <copyright file="license.md" company="Wlog project  (https://github.com/arduosoft/wlog)">
// Copyright (c) 2016 Wlog project  (https://github.com/arduosoft/wlog)
// Wlog project is released under LGPL terms, see license.md file.
// </copyright>
// <author>Daniele Fontani, Emanuele Bucaelli</author>
// <autogenerated>true</autogenerated>
//******************************************************************************
using AutoMapper;
using NLog;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Wlog.Library.BLL.Configuration;
using Wlog.Library.BLL.Reporitories;
using Wlog.Library.BLL.Utils;
using Wlog.Library.Scheduler;
using Wlog.Web.Code.Mappings;
using NLog;
using System.Configuration;

namespace Wlog.Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private bool installed = "True".Equals(ConfigurationManager.AppSettings["WlogInstalled"], StringComparison.InvariantCultureIgnoreCase);

        protected void Application_Start()
        {
           
                try

            {
                    _logger.Info("Application starts");

                    _logger.Info("Registering configuration");
                    AreaRegistration.RegisterAllAreas();

                    WebApiConfig.Register(System.Web.Http.GlobalConfiguration.Configuration);
                    FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                    RouteConfig.RegisterRoutes(RouteTable.Routes);
                    BundleConfig.RegisterBundles(BundleTable.Bundles);

                Mapper.Initialize(cfg => cfg.AddProfile(new ApplicationProfile()));

                _logger.Info("Apply schema changes");
                SystemDataInitialisation.Instance.ApplySchemaChanges();

                _logger.Info("Setup info config");

                InfoPageConfigurator.Configure(c =>
                {
                    c.ApplicationName = "Wlog";

                });


                if (installed)
                {
                    //TODO: move this in installation process

                    _logger.Info("Apply schema changes");
                    RepositoryContext.Current.System.ApplySchemaChanges();

                  

                    _logger.Info("Start background jobs");

                    HangfireBootstrapper.Instance.Start();

                    _logger.Info("Setup index configuration");
                    IndexRepository.BasePath = HttpContext.Current.Server.MapPath("~/App_Data/Index/");


                _logger.Info("install missing data");
                SystemDataHelper.InsertRolesAndProfiles();
                SystemDataHelper.EnsureSampleData();
                SystemDataHelper.InsertJobsDefinitions();

                _logger.Info("application started");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        protected void Application_End(object sender, EventArgs e)
        {
            _logger.Info("Application end");

            _logger.Info("Stopping HangfireBootstrapper");
            HangfireBootstrapper.Instance.Stop();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (!installed)
            {
                var path = HttpContext.Current.Request.RawUrl;
                var allowed = (path.StartsWith("/install", StringComparison.InvariantCultureIgnoreCase)) ||
                    (path.StartsWith("/Scripts", StringComparison.InvariantCultureIgnoreCase)) ||
                    (path.StartsWith("/Images", StringComparison.InvariantCultureIgnoreCase) )||
                    (path.StartsWith("/Content", StringComparison.InvariantCultureIgnoreCase)) ||
                    (path.EndsWith(".ico", StringComparison.InvariantCultureIgnoreCase));
                if (!allowed)
                {
                    HttpContext.Current.RewritePath("~/install");
                }
            }
        }
    }
}
