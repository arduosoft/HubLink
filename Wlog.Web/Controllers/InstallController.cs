using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using NHibernate.Cfg;
using Wlog.DAL.NHibernate.Helpers;
using Wlog.Library.BLL.Helpers;
using Wlog.Web.Models.Install;

namespace Wlog.Web.Controllers
{
    [AllowAnonymous]
    public class InstallController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            var newInstall = new InstallModel();

            return View("Install", newInstall);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult Install(string connectionString, string driver, string dialect)
        {
            string error = "";
            //Write config file here.
            var config  = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            var baseFolder = Server.MapPath("~/");
            //var backupFolder = Server.MapPath("~/App_Data/Backup/");
            var configFile = Path.Combine(baseFolder, "Web.config");



            XmlDocument doc = new XmlDocument();  
                                
            doc.Load(configFile);
            //get root element
            System.Xml.XmlElement Root = doc.DocumentElement;


            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("nh", "urn:nhibernate-configuration-2.2");

            Root.SelectNodes("//nh:property[@name='connection.driver_class']", nsmgr)[0].InnerText = driver;
            Root.SelectNodes("//nh:property[@name='connection.connection_string']", nsmgr)[0].InnerText = connectionString;
            Root.SelectNodes("//nh:property[@name='dialect']", nsmgr)[0].InnerText = dialect;

            Root.SelectNodes("//nh:property[@name='dialect']", nsmgr)[0].InnerText = dialect;

            Root.SelectNodes("//add[@key='WlogInstalled']")[0].Attributes["value"].Value = "True";


            doc.Save(configFile);
               
            

            return Json(error);
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult Reload()
        {

            HttpRuntime.UnloadAppDomain();
            return Json("OK");
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult Validate(string connectionString, string driver, string dialect )
        {
            List<string> errorResults = new List<string>();
            if (string.IsNullOrEmpty(connectionString))
            {
                errorResults.Add("<span class=\"label label-danger\">KO>Connection string is empty");
            }
            else
            {
                
                
                errorResults.Add("<span class=\"label label-success\">OK</span>Connection string is OK");
                
            }

            try
            {
                Type.GetType(driver + ",NHibernate");

                errorResults.Add("<span class=\"label label-success\">OK</span>Driver is  valid");

            }
            catch
            {
                errorResults.Add("<span class=\"label label-danger\">KO</span>Driver is not valid");
            }


            try
            {
                Type.GetType(dialect + ",NHibernate");
                errorResults.Add("<span class=\"label label-success\">OK</span>Dialect is  valid");
            }
            catch
            {
                errorResults.Add("<span class=\"label label-danger\">KO</span>Dialect is not valid");
            }


            try
            {
                
                var  basePath = this.HttpContext.Server.MapPath("~/App_Data/Index/");
                if (!Directory.Exists(basePath))
                {
                    Directory.CreateDirectory(basePath);
                }
                var rdm = DateTime.Now.ToString("yyyyMMddHHmmss");
                var testFile = System.IO.Path.Combine(basePath, rdm + "testfile.txt");
                System.IO.File.WriteAllText(testFile, "xxx");
                System.IO.File.Delete(testFile);

                var testFolder = System.IO.Path.Combine(basePath, rdm + "_test");
                System.IO.Directory.CreateDirectory(testFolder );
                System.IO.Directory.Delete(testFolder);

                errorResults.Add("<span class=\"label label-success\">OK</span>Index file is writable");

            }
            catch
            {

                errorResults.Add("<span class=\"label label-danger\">KO</span>App_Data missing write permission. Please change settings to store files in other directory after installation or give right permission to file system.");
            }

            try
            {


                var cfg = new Configuration();
                    cfg.Properties = (new Dictionary<string, string> {
                    {NHibernate.Cfg.Environment.ConnectionDriver, driver},
               //     {NHibernate.Cfg.Environment.ProxyFactoryFactoryClass, typeof (ProxyFactoryFactory).AssemblyQualifiedName},
                    {NHibernate.Cfg.Environment.Dialect, dialect},
                //    {NHibernate.Cfg.Environment.ConnectionProvider, typeof (DriverConnectionProvider).FullName},
                    {NHibernate.Cfg.Environment.ConnectionString, connectionString},
            });
            cfg.AddAssembly(Assembly.GetExecutingAssembly());
              

                //var cfg = new Configuration();
                
                //cfg.SetProperty("connection.connection_string", connectionString); // Alter the property
                //cfg.SetProperty("dialect", dialect);
                //cfg.SetProperty("connection.driver_class", driver);
                //cfg.SetProperty("current_session_context_class", "call");
                //cfg.Configure(); // Configure with this configuration
               var sf = cfg.BuildSessionFactory(); // Get a new ISessionFactory
               var conn= sf.OpenSession();
                conn.Close();

                errorResults.Add("<span class=\"label label-success\">OK</span>Connection to DB OK");

            }
            catch (Exception err)
            {
                errorResults.Add("<span class=\"label label-danger\">KO</span>Unable to conncet to database:" + err.Message+"");
            }

            return Json(errorResults);
        }



        public JsonResult GetDrivers()
        {
           
            List<SelectListItem> driversList = new List<SelectListItem>();
            

             driversList.Add(
                new SelectListItem()
                {
                    Text = "Oracle",
                    Value = "NHibernate.Driver.OracleClientDriver"

                });

            driversList.Add(
              new SelectListItem()
              {
                  Text = "PostgreSQL",
                  Value = "  NHibernate.Driver.NpgsqlDriver"

              });


            driversList.Add(
           new SelectListItem()
           {
               Text = "MySql",
               Value = "NHibernate.Driver.MySqlDataDriver"

           });



            driversList.Add(
           new SelectListItem()
           {
               Text = "Microsoft SQL Server",
               Value = "NHibernate.Driver.SqlClientDriver"

           });

            driversList.Add(
           new SelectListItem()
           {
               Text = "Microsoft SQL Server (2008)",
               Value = "NHibernate.Driver.Sql2008ClientDriver",
               Selected=true

           });
            

            return Json(driversList.OrderBy(x=>x.Text));
        }


        public JsonResult GetDialects(string Driver)
        {
            string dbName = Driver.Substring(Driver.LastIndexOf(".") + 1);
            dbName=dbName.Replace("Driver", "");
            dbName = dbName.Replace("Client", "");
            dbName = dbName.Replace("Data", "");
            if (dbName == "Npgsql") dbName = "PostgreSQL";


            List<SelectListItem> dialectsList = new List<SelectListItem>();

            Type[] dialects = ReflectionHelper.GetTypesInNamespace(typeof(NHibernate.EmptyInterceptor).Assembly, "NHibernate.Dialect");

            foreach (var d in dialects)
            {
                if (d.IsSubclassOf(typeof(NHibernate.Dialect.Dialect)))
                {
                    if (d.Name.Contains(dbName))
                    {
                        dialectsList.Add(new SelectListItem()
                        {
                            Text = d.Name,
                            Value = d.FullName
                        });
                    }
                }
            }
                    return Json(new SelectList(dialectsList, "Value", "Text"));
        }
    }
}