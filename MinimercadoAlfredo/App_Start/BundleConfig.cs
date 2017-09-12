using System.Web;
using System.Web.Optimization;

namespace MinimercadoAlfredo
{
    public class BundleConfig
    {
        // Para obtener más información sobre Bundles, visite http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;
            ScriptBundle scriptBundle = new ScriptBundle("~/js");
            string[] scriptArray =
            {
                
                "~/Scripts/jquery-1.12.4.min.js",
                "~/Scripts/jquery-ui-1.12.0.min.js",
                "~/Scripts/jquery.unobtrusive-ajax.min.js",
                "~/Scripts/bootstrap.min.js",
                "~/Scripts/datatables/jquery.datatables.min.js",
                "~/Scripts/datatables/datatables.bootstrap.min.js",
                "~/Scripts/bootstrap.min.js",
                "~/Scripts/respond.min.js",
                "~/Scripts/bootbox.min.js",
                "~/Scripts/toastr.min.js",
                "~/Scripts/bootstrap-select.min.js",
                "~/Scripts/solonumeros.js"
            };

            scriptBundle.Include(scriptArray);
            scriptBundle.IncludeDirectory("~/Scripts/", "*.js");
            bundles.Add(scriptBundle);


            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // preparado para la producción y podrá utilizar la herramienta de compilación disponible en http://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap-spacelab.css",
                      "~/Content/datatables/css/datatables.bootstrap.min.css",
                      "~/Content/datatables/css/jquery.dataTables.min.css",
                      "~/content/toastr.css",
                      "~/Content/site.css", "~/Content/bootstrap-select.min.css",
                       "~/Content/bootstrap-select.min.css.map"));
        }
    }
}
