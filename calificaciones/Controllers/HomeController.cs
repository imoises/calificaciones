using calificaciones.Models;
using calificaciones.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using calificaciones.Entidades;

namespace calificaciones.Controllers
{
    public class HomeController : Controller
    {
        AlumnoService alumnoService = new AlumnoService();
        ProfesorService profesorService = new ProfesorService();

        public ActionResult Ingresar()
        {
            return View();
        }

        [HttpPost]
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
        }

        public ActionResult Error()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Sesion(bool soyProfesor)
        {

            if(soyProfesor)
            {
                return RedirectToAction("AdminPreguntas", "Profesor");
            }
            else
            {
                return RedirectToAction("Inicio", "Alumno");
            }
            
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