using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace calificaciones.Session
{
    public class AuthorizeAlumno : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var esAutorizado = base.AuthorizeCore(httpContext);
            if (!esAutorizado)
            {
                return false;
            } else
            {
                return true;
            }
            
        }
    }
}