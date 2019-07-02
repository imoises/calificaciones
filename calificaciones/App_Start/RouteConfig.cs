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
               url: "{controller}/{action}/{id}", //si esta url no la satisface, pasa a la siguiente
               defaults: new { controller = "Home", action = "Ingresar", id = UrlParameter.Optional }
           );   

            routes.MapRoute(
                name: "Profesor",
                url: "Profesor/{controller}/{action}/{id}", //Entonces todas las url que comiencen con Profesor/Preguntas
                defaults: new { controller = "Profesor", action = "Preguntas", id = UrlParameter.Optional } //van a usar ProfesorController
            );

           

            
        }
    }
}
