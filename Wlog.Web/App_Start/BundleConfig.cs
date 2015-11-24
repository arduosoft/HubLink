using System.Web;
using System.Web.Optimization;

namespace Wlog.Web
{
    public class BundleConfig
    {
        // Per ulteriori informazioni sul Bundling, visitare il sito Web all'indirizzo http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.IgnoreList.Clear();
 
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));
 
            bundles.Add(new ScriptBundle("~/bundles/bootstrapjs").Include(
                        "~/Scripts/bootstrap.min.js"));
 
            bundles.Add(new StyleBundle("~/Content/bootstrapcss").Include(
                        "~/Content/bootstrap.min.css",
                        "~/Content/bootstrap-responsive.min.css"));

            bundles.Add(new StyleBundle("~/Content/SiteCss").Include(
            "~/Content/Site.css"));


            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Utilizzare la versione di sviluppo di Modernizr per eseguire attività di sviluppo e formazione. Successivamente, quando si è
            // pronti per passare alla produzione, utilizzare lo strumento di compilazione disponibile all'indirizzo http://modernizr.com per selezionare solo i test necessari.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

        }
    }
}