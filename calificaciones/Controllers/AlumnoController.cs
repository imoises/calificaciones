using calificaciones.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace calificaciones.Controllers
{
    public class AlumnoController : Controller
    {
        AlumnoService alumnoService = new AlumnoService();

        // GET: Alumno
        public ActionResult Inicio()
        {
            var alumno = alumnoService.ObtenerTodosLosAlumnos();
            return View(alumno);
        }
    }
}
        