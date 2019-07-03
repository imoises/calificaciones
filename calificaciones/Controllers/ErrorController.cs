using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace calificaciones.Controllers
{
    public class ErrorController : Controller
    {
       public ActionResult Error()
       {
            int err = 0;
            switch (err)
            {
                case 400:
                    ViewBag.ErrorMensaje = "La página tiene sintaxis incorrecta";
                    ViewBag.Descripcion = "Por favor ingrese nuevamente";
                    break;
                case 401:
                    ViewBag.ErrorMensaje = "La página que ingreso tiene acceso restringido";
                    ViewBag.Descripcion = "Por favor ingrese nuevamente";
                    break;
                case 403:
                    ViewBag.ErrorMensaje = "Se ha denegado el acceso";
                    ViewBag.Descripcion = "Por favor ingrese nuevamente";
                    break;
                case 404:
                    ViewBag.ErrorMensaje = "La página que ingreso tiene un formato no válido";
                    ViewBag.Descripcion = "Por favor ingrese nuevamente";
                    break;
                case 406:
                    ViewBag.ErrorMensaje = "Código no interpretado por el navegador";
                    ViewBag.Descripcion = "Por favor ingrese nuevamente";
                    break;
                case 500:
                    ViewBag.ErrorMensaje = "Error interno...ya lo estamos solucionando";
                    ViewBag.Descripcion = "Por favor ingrese nuevamente";
                    break;
                case 504:
                    ViewBag.ErrorMensaje = "El tiempo de espera se ha agotado";
                    ViewBag.Descripcion = "Por favor ingrese nuevamente";
                    break;
                case 509:
                    ViewBag.ErrorMensaje = "Ha superado el límite de banda ancha disponible";
                    ViewBag.Descripcion = "Por favor ingrese nuevamente";
                    break;
                default:
                    ViewBag.ErrorMensaje = "La página que ingreso es erronea";
                    ViewBag.Descripcion = "Por favor ingrese nuevamente";
                    break;

            }
            
            return View();
       }
    }
}
