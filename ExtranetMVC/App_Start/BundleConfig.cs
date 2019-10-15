using System.Web;
using System.Web.Optimization;

namespace ExtranetMVC
{
    public class BundleConfig
    {
        // Per altre informazioni sulla creazione di bundle, vedere https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

                bundles.Add(new StyleBundle("~/Content/Jstree").Include(
                    "~/Content/jstree.css"));
            bundles.Add(new StyleBundle("~/Content/pianoFornitore").Include(
                   "~/Content/pianoFornitore.css"));

            // Utilizzare la versione di sviluppo di Modernizr per eseguire attività di sviluppo e formazione. Successivamente, quando si è
            // pronti per passare alla produzione, usare lo strumento di compilazione disponibile all'indirizzo https://modernizr.com per selezionare solo i test necessari.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/umd/popper.min.js",
                      "~/Scripts/umd/popper-utils.min.js",
                      "~/Scripts/bootstrap.min.js"));


            bundles.Add(new ScriptBundle("~/bundles/jstree").Include(
                      "~/Scripts/jstree.js"));

            //bundles.Add(new ScriptBundle("~/bundles/cookies").Include(
            //    "~/Scripts/js.cookie.min.js",
            //          "~/Scripts/cookiealert.js"));
            bundles.Add(new ScriptBundle("~/bundles/jQueryBinary").Include(
                      "~/Scripts/js-jquery-master/BinaryTransport/jquery.binarytransport.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                                  "~/Scripts/DataTables/jquery.dataTables.min.js",
                                  "~/Scripts/DataTables/dataTables.bootstrap4.min.js",
                                  "~/Scripts/DataTables/dataTables.buttons.min.js",
                                  "~/Scripts/pdfmake.min.js",
                                  "~/Scripts/vfs_fonts.js",
                                  "~/Scripts/buttons.html5.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            //bundles.Add(new StyleBundle("~/Content/cookies").Include(
            //          "~/Content/cookiealert.css"));

            //bundles.Add(new StyleBundle("~/Content/datatables").Include(
            //                    "~/Content/DataTables/css/dataTables.bootstrap4.min.css"));
        }
    }
}
