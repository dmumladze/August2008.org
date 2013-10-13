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
            bundles.Add(new ScriptBundle("~/bundles/august2008").Include(
                "~/Scripts/August2008.js",
                "~/Scripts/Security.js",
                "~/Scripts/Spinner.js",
                "~/Scripts/jquery.watermark.js"));

            bundles.Add(new ScriptBundle("~/bundles/hero").Include(
                "~/Scripts/Hero.js"));

            bundles.Add(new ScriptBundle("~/bundles/donations").Include(
                "~/Scripts/Donations.js"));

            bundles.Add(new ScriptBundle("~/bundles/bingmap").Include(
                "~/Scripts/BingMap.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-1.9.1.js",
                "~/Scripts/jquery-ui-1.10.3.custom.js",
                "~/Scripts/jquery.jqote2.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.unobtrusive*",
                "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/content/css").Include(
                "~/content/Bootstrap.css",
                "~/content/jquery-ui-1.10.3.custom.css",
                "~/content/Site.css"));

            bundles.Add(new StyleBundle("~/content/bingmap").Include(
                "~/content/BingMap.css"));
        }
    }
}