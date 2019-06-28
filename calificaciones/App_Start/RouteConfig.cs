using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace calificaciones
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Ingresar", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Alumno",
                url: "{controller1}/{controller}/{action}/{id}",
                defaults: new { controller1 = "o", controller = "Alumno", action = "Inicio", id = UrlParameter.Optional }
            );
        }
    }
}
