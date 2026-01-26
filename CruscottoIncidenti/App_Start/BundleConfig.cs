using System.Web.Optimization;

namespace CruscottoIncidenti
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery.unobtrusive-ajax.min.js",
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.bundle.min.js",
                      "~/Scripts/bootstrap-select.min.js"));

            bundles.Add(new Bundle("~/bundles/toastr").Include(
                      "~/Scripts/toastr.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                      "~/Scripts/custom.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/fontawesome-all.min.css",
                      "~/Content/bootstrap.min.css",
                      "~/Content/bootstrap-select.min.css",
                      "~/Content/toastr.min.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/dataTables").Include(
                "~/Scripts/dataTables.min.js",
                "~/Scripts/dataTables.bootstrap5.min.js",
                "~/Scripts/dataTables.buttons.min.js",
                "~/Scripts/dataTables.select.min.js",
                "~/Scripts/buttons.html5.min.js"));

            bundles.Add(new StyleBundle("~/Content/dataTables").Include(
                "~/Content/dataTables.bootstrap5.min.css",
                "~/Content/select.bootstrap.min.css",
                "~/Content/buttons.bootstrap.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/datatables/users").Include(
                "~/Scripts/custom/DataTable/common.js",
                "~/Scripts/custom/DataTable/users.datatable.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatables/incidents").Include(
                "~/Scripts/custom/DataTable/common.js",
                "~/Scripts/custom/DataTable/incidents.datatable.js"));
        }
    }
}
