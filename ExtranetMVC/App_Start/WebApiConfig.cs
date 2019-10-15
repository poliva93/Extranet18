using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Routing;

namespace ExtranetMVC
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            IHttpRoute defaultRoute = config.Routes.CreateRoute(
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional },
                null
            );

            // Add route
            //20190502 AGGIUNTO CAUSA LINQ CON REFERENCESLOOP
            config.Routes.Add("DefaultApi", defaultRoute);
            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            config.Formatters.Remove(config.Formatters.XmlFormatter);
        }
    }
}
