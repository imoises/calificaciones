using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace calificaciones
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Session_Start(Object sender, EventArgs e)
        {
            Session["Email"] = String.Empty;
            Session["SoyProfesor"] = String.Empty;
        }

        protected void Application_Error(Object sender, EventArgs e) 
        {
            Exception exc = Server.GetLastError();
            Response.Clear();
            HttpException httpException = exc as HttpException;
            int error = httpException != null ? httpException.GetHttpCode() : 0;
            Server.ClearError();
            Response.Redirect(String.Format("~/Error/Error/?error={0}", error, exc.Message));
        }
    }
}
