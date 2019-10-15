using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ExtranetMVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }

            );
            
            routes.MapRoute(
                name: "PianoFornitore",
                url: "PianoFornitore/{action}/{id}",
                defaults: new { controller = "PianoFornitore", action = "Index", id = UrlParameter.Optional }

            );

            routes.MapRoute(
                name: "FileAPI",
                url: "api/file/download/{*fileName}",
                defaults: new { controller = "FileAPI", action = "Download", id = UrlParameter.Optional }
                );

        }
    }
}
