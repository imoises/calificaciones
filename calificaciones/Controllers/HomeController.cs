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

        /*[HttpPost]
        public ActionResult Login(Usuario usuario)
        {
            Boolean esAlumno;
            Boolean esProfesor;
            Alumno alumno = null;
            Profesor profesor = null;
            if (!usuario.SoyProfesor)
            {
                if(!alumnoService.verificarAlumnoRegistrado(alumno))
                {
                    ViewBag.MessageFailed = "Email y/o Contraseña inválidos";
                    return View();
                }
                else
                {
                    esAlumno = alumnoService.verificarAlumnoRegistrado(alumno);
                    return RedirectToAction("AlumnoIndex", esAlumno);
                }
            }
            else
            {
                if (profesorService.verificarProfesorRegistrado(profesor))
                {
                    ViewBag.MessageFailed = "Email y/o Contraseña inválidos";
                    return View();
                }
                else
                {
                    esProfesor = profesorService.verificarProfesorRegistrado(profesor);
                    return RedirectToAction("ProfesorIndex", esProfesor);
                }
            }
        }*/

        public ActionResult Error()
        {
            return View();
        }

       [HttpPost]
       public ActionResult LoginSession(Usuario usuario,string returnUrl)
        {
            var sessionService = new SessionService();
            FormsAuthentication.SetAuthCookie(usuario.Email, true);
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
                        return RedirectToAction("AdminPreguntas", "Profesor");
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
