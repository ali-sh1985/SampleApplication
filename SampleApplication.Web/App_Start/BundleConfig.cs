using System.Web;
using System.Web.Optimization;

namespace SampleApplication.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                         "~/Scripts/lib/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/ui-grid-stable.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                        "~/Scripts/app/app.js",
                        "~/Scripts/app/controllers.js",
                        "~/Scripts/app/services.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                        "~/Scripts/lib/angular.js",
                        "~/Scripts/lib/angular-route.js",
                        "~/Scripts/lib/ui-bootstrap-tpls-{version}.js",
                        "~/Scripts/lib/ui-grid-stable.js",
                        "~/Scripts/lib/angular-local-storage.js"
                        ));
        }
    }
}
