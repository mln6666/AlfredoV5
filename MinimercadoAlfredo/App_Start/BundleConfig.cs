using System.Web;
using System.Web.Optimization;

namespace MinimercadoAlfredo
{
    public class BundleConfig
    {
        // Para obtener más información sobre Bundles, visite http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/lib").Include(
                          "~/Scripts/jquery-1.12.4.js",
                        "~/Scripts/jquery-1.12.4.min.js",
                         "~/Scripts/jquery-ui-1.12.0.js",
                        "~/Scripts/jquery-ui-1.12.0.min.js",
                         "~/Scripts/jquery.unobtrusive-ajax.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/datatables/jquery.datatables.js",
                        "~/Scripts/datatables/datatables.bootstrap.js",                        
                        "~/Scripts/bootstrap.min.js",
                        "~/Scripts/respond.js",
                        "~/Scripts/bootbox.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // preparado para la producción y podrá utilizar la herramienta de compilación disponible en http://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap-spacelab.css",
                      "~/Content/datatables/css/datatables.bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}
