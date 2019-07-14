using calificaciones.Models;
using calificaciones.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using calificaciones.Entidades;
using System.Web.Security;
using calificaciones.Session;

namespace calificaciones.Controllers
{
    public class HomeController : Controller
    {
        AlumnoService alumnoService = new AlumnoService();
        ProfesorService profesorService = new ProfesorService();

      //  [AuthorizeProfesor]
      //  [AuthorizeAlumno]
       // [AllowAnonymous]
        public ActionResult Ingresar()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginSession(Usuario usuario)
        {
            var sessionService = new SessionService();
            if (ModelState.IsValid)
            {
                if (usuario.SoyProfesor)
                {
                    var profesor = sessionService.IniciarProfesor(usuario);
                    if (profesor != null)
                    {
                        SessionManagement.IdUsuario = profesor.IdProfesor;
                        SessionManagement.Nombre = profesor.Apellido + ", " + profesor.Nombre;
                        SessionManagement.Rol = RolesPermisos.ROL_PROFESOR;
                        FormsAuthentication.SetAuthCookie(usuario.Email, false);
                        var authTicket = new FormsAuthenticationTicket(1, usuario.Email, DateTime.Now, DateTime.Now.AddMinutes(20), false, SessionManagement.Rol);
                        string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                        var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                        HttpContext.Response.Cookies.Add(authCookie);
                        return RedirectToAction("Preguntas", "Profesor");
                    }
                }
                else
                {
                    var alumno = sessionService.IniciarAlumno(usuario);
                    if (alumno != null)
                    {
                        SessionManagement.IdUsuario = alumno.IdAlumno;
                        SessionManagement.Nombre = alumno.Apellido + ", " + alumno.Nombre;
                        SessionManagement.Rol = RolesPermisos.ROL_ALUMNO;
                        FormsAuthentication.SetAuthCookie(usuario.Email, false);
                        var authTicket = new FormsAuthenticationTicket(1, usuario.Email, DateTime.Now, DateTime.Now.AddMinutes(20), false, SessionManagement.Rol);
                        string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                        var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                        HttpContext.Response.Cookies.Add(authCookie);
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
