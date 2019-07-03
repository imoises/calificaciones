using calificaciones.Models;
using calificaciones.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using calificaciones.Entidades;
using System.Web.Security;

namespace calificaciones.Controllers
{
    public class HomeController : Controller
    {
        AlumnoService alumnoService = new AlumnoService();
        ProfesorService profesorService = new ProfesorService();

        [Authorize]
        [AllowAnonymous]
        public ActionResult Ingresar()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginSession(Usuario usuario,string returnUrl)
        {
            var sessionService = new SessionService();
            FormsAuthentication.SetAuthCookie(usuario.Email, false);
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            if (ModelState.IsValid)
            {
                if (usuario.SoyProfesor)
                {
                    var profesor = sessionService.IniciarProfesor(usuario);
                    if (profesor != null)
                    {
                        Session["Id"] = profesor.IdProfesor;
                        Session["Nombre"] = profesor.Apellido + ", " + profesor.Nombre;
                        Session["Rol"] = "Profesor";
                        return RedirectToAction("Preguntas", "Profesor");
                    }
                }
                else
                {
                    var alumno = sessionService.IniciarAlumno(usuario);
                    if (alumno != null)
                    {
                        Session["Id"] = alumno.IdAlumno;
                        Session["Nombre"] = alumno.Apellido + ", " + alumno.Nombre;
                        Session["Rol"] = "Alumno";
                        return RedirectToAction("Inicio", "Alumno");
                    }
                }
                TempData["MensajeError"] = "Email y/o Contraseña inválidos";
            }
            return RedirectToAction("Ingresar", "Home");
        }

        public ActionResult LoginSessionOut()
        {
            Session.Clear();
            return RedirectToAction("Ingresar", "Home");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
