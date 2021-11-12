using System.Web;
using System.Web.Optimization;

namespace Quiver
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.unobtrusive*",
                "~/Scripts/jquery.validate*",
                 "~/Scripts/methods_pt.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/Custom/utils.js",
                "~/Scripts/Utils/calculos.js",
                "~/Scripts/respond.js"));



            bundles.Add(new ScriptBundle("~/bundles/login").Include(
                "~/Scripts/template/waves/waves.js",
                "~/Scripts/template/sweetAlert/sweetalert-dev.js",
                "~/Scripts/template/functions.js"));

            bundles.Add(new StyleBundle("~/Content/login").Include(
                "~/Content/animated/animate.css",
                "~/Content/site.css",
                "~/Content/template/material-design-color-palette/material-design-color-palette.css",
                "~/Content/template/sweetAlert/sweetalert.css",
                "~/Content/template/waves/waves.css",
                "~/Content/material-icon/material-design-iconic-font.css"));

            bundles.Add(new ScriptBundle("~/bundles/template").Include(
                "~/Scripts/Template/easyPie/jquery.easypiechart.js",
                "~/Scripts/Template/malihu-custom-scrollbar-plugin/jquery.mCustomScrollbar.js",
                "~/Scripts/Template/charts.js",
                "~/Scripts/moment.js",
                "~/Scripts/Template/sweetAlert/sweetalert-dev.js",
                "~/Scripts/Template/jquery.bootgrid.js",
                "~/Scripts/Template/lightGallery/lightGallery.js",
                "~/Scripts/Template/datetimepicker/bootstrap-datetimepicker.js",
                "~/Scripts/locale/pt-br.js",
                "~/Scripts/Template/fullCalendar/fullcalendar.js",
                "~/Scripts/Template/waves/waves.js",
                "~/Scripts/Template/fullCalendar/lang/pt-br.js",
                "~/Scripts/Custom/waiting.js",
                "~/Scripts/Custom/account.js",
                "~/Scripts/Template/bootstrap-select.js",
                "~/Scripts/Template/chosen.jquery.js",
                "~/Scripts/Template/functions.js"));

            bundles.Add(new ScriptBundle("~/bundles/charts").Include(
                "~/Scripts/Template/flot/jquery.flot.js",
                "~/Scripts/Template/flot/jquery.flot.axislabels.js",
                "~/Scripts/Template/flot/jquery.flot.time.js",
                "~/Scripts/Template/flot/jquery.flot.tooltip.js"));

            bundles.Add(new ScriptBundle("~/bundles/flotCharts").Include(
                "~/Scripts/Template/flotCharts/bar-chart.js",
                "~/Scripts/Template/flotCharts/curved-line-chart.js",
                "~/Scripts/Template/flotCharts/line-chart.js",
                "~/Scripts/Template/flotCharts/pie-chart.js",
                "~/Scripts/Template/flotCharts/curved-line-chart.js"));

            bundles.Add(new StyleBundle("~/Style/template").Include(
                "~/Content/template/waves/waves.css",
                "~/Content/template/app.css",
                "~/Content/animated/animate.css",
                "~/Content/template/jquery.mCustomScrollbar.css",
                "~/Content/template/fullCalendar/fullcalendar.css",
                "~/Content/template/sweetAlert/sweetalert.css",
                "~/Content/template/light-gallery/lightGallery.css",
                "~/Content/template/datetimepicker/bootstrap-datetimepicker.css",
                "~/Content/template/bootstrap-select/bootstrap-select.css",
                "~/Content/material-icon/material-design-iconic-font.css",
                "~/Content/template/material-design-color-palette/material-design-color-palette.css",
                "~/Content/animated/bolinha-animada.css",
                "~/Content/template/jquery.bootgrid.css",
                "~/Content/template/chosen.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/waiting.css",
                "~/Content/site.css"));

            // HOT-SITE
            bundles.Add(new StyleBundle("~/Style/hotsite").Include(
                "~/Content/bootstrap.css",
                "~/Content/HotSite/hotsite.css",
                "~/Content/HotSite/agency.css",
                "~/Content/HotSite/skin-blue.css",
                "~/Content/HotSite/timeline.css",
                "~/Content/animated/animate.css",
                "~/Content/custom/modalFullScreen/modalFullScreen.css",
                "~/Content/material-icon/material-design-iconic-font.css"));

            bundles.Add(new ScriptBundle("~/bundles/hotsite").Include(
                "~/Scripts/HotSite/hotsite.js",
                "~/Scripts/HotSite/agency.js",
                "~/Scripts/HotSite/modalFullScreen.js",
                "~/Scripts/HotSite/cbpAnimatedHeader.js"));

            //color selector
            bundles.Add(new ScriptBundle("~/bundles/colorselector").Include(
                "~/Scripts/bootstrap-colorselector.js"));

            bundles.Add(new StyleBundle("~/Content/colorselector").Include(
                    "~/Content/bootstrap-colorselector.css"));

            // jquery moneyMask
            bundles.Add(new ScriptBundle("~/bundles/moneyMask").Include("~/Scripts/jquery.moneymask.js"));


            //signalR
            bundles.Add(new ScriptBundle("~/bundles/signalR").Include(
                     "~/Scripts/signalr/hubs",
                     "~/Scripts/jquery.signalR-{version}.js"));
        }
    }
}
