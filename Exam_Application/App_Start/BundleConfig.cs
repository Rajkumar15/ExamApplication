using System.Web;
using System.Web.Optimization;

namespace Exam_Application
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/AdminFiles/JS/jquery-1.8.2.min.js",
                "~/AdminFiles/JS/html5shiv.js",
                "~/AdminFiles/JS/respond.min.js",
                "~/AdminFiles/JS/bootstrap.min.js",
                "~/AdminFiles/JS/jquery.validationEngine-en.js",
                "~/AdminFiles/JS/jquery.validationEngine.js",
                "~/AdminFiles/JS/superfish.js",
                "~/AdminFiles/JS/sidebar.js",
                "~/AdminFiles/JS/functions.js",
                "~/AdminFiles/JS/plugins.min.js",
                "~/AdminFiles/JS/revolution.min.js",
                "~/AdminFiles/JS/custom.min.js",
                "~/AdminFiles/JS/highcharts.js",
                "~/AdminFiles/RajCustom/common.js",
                "~/AdminFiles/RajCustom/DataTable/jquery.dataTables.js",
                "~/AdminFiles/JS/jquery.freeow.js",
                "~/AdminFiles/JS/ShowMessage.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include());

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/AdminFiles/CSS/bootstrap.min.css",
                "~/AdminFiles/CSS/style.css",
                "~/AdminFiles/CSS/style(1).css",
                "~/AdminFiles/CSS/fonts.css",
                "~/AdminFiles/CSS/plugins.css",
                "~/AdminFiles/CSS/responsive.css",
                "~/AdminFiles/CSS/settings.css",
                "~/AdminFiles/CSS/validationEngine.jquery.css",
                "~/AdminFiles/RajCustom/DataTable/jquery.dataTables.min.css",
                "~/Content/style/freeow/freeow.css"));
        }
    }
}
