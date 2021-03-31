using System.Web.Optimization;


namespace SampleCode
{
    public class BundleConfig
    {
        // 如需「搭配」的詳細資訊，請瀏覽 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/login").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.unobtrusive-ajax.js",
                "~/Scripts/jquery.validate*",
                "~/Scripts/bootstrap.js",
                "~/Scripts/string.format.js",
                "~/Scripts/basic.js"));

            bundles.Add(new StyleBundle("~/Content/login").Include(
                "~/Content/bootstrap.css",
                "~/Content/font-awesome.css",
                "~/Content/animate.css",
                "~/Content/style.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/themes/base/jquery-ui.css",
                "~/Content/dragtable.css",
                "~/Content/bootstrap.css",
                "~/Content/animate.css",
                "~/Content/style.css",
                "~/Scripts/bootstrap-table/bootstrap-table.css",
                "~/Content/bootstrap-datetimepicker.css",
                "~/Content/bootstrap-select.css",
                "~/Content/bootstrap-slider.css",
                "~/Content/bootstrap-switch.css",
                "~/Content/font-awesome.css",
                "~/Content/ladda-themeless.min.css",
                "~/Content/Site.css"));

            bundles.Add(new ScriptBundle("~/bundles/script").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery-ui-{version}.js",
                "~/Scripts/jquery.unobtrusive-ajax.js",
                "~/Scripts/jquery.slimscroll.js",
                "~/Scripts/metisMenu.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/moment-with-locales.js",
                "~/Scripts/bootstrap-datetimepicker.min.js",
                "~/Scripts/bootstrap-select.js",
                "~/Scripts/i18n/defaults-zh_TW.min.js",
                "~/Scripts/bootstrap-slider.js",
                "~/Scripts/bootstrap-switch.js",
                "~/Scripts/bootstrap-typeahead.js",
                "~/Scripts/jquery.actual.js",
                "~/Scripts/spin.min.js",
                "~/Scripts/ladda.min.js",
                "~/Scripts/inspinia.js",
                "~/Scripts/string.format.js",
                "~/Scripts/basic.js"));

            bundles.Add(new ScriptBundle("~/bundles/table").Include(
                "~/Scripts/jquery.dragtable.js",
                "~/Scripts/colResizable-1.6.js",
                "~/Scripts/bootstrap-table/bootstrap-table.js",
                "~/Scripts/bootstrap-table/extensions/reorder-columns/bootstrap-table-reorder-columns.js",
                "~/Scripts/bootstrap-table/extensions/resizable/bootstrap-table-resizable.js",
                "~/Scripts/bootstrap-table/locale/bootstrap-table-zh-TW.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"));
        }
    }
}