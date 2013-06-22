using System.Web;
using System.Web.Optimization;

namespace August2008
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;
#if DEBUG
            BundleTable.EnableOptimizations = false;
#endif
            bundles.Add(new ScriptBundle("~/bundles/hero").Include(
                "~/Scripts/August2008.js",
                "~/Scripts/Hero.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-1.9.1.js",
                "~/Scripts/jquery-ui-1.10.3.custom.js",
                "~/Scripts/jquery.jqote2.js"));

            bundles.Add(new ScriptBundle("~/bundles/security").Include(
                "~/Scripts/Security.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/content/css").Include(
                "~/content/site.css",
                "~/content/jquery-ui-1.10.3.custom.css"));
        }
    }
}