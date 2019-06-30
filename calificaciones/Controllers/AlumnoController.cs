using calificaciones.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using calificaciones.Models;

namespace calificaciones.Controllers
{
    public class AlumnoController : Controller
    {
        AlumnoService alumnoService = new AlumnoService();

        // GET: Alumno
        public ActionResult Inicio()
        {
            InicioAlumnoViewModel modelo = new InicioAlumnoViewModel();
            
            modelo.UltimasPreguntasCorregidas = alumnoService.ObtenerPreguntasUltimaClase();

            modelo.TablaDePosiciones = alumnoService.ObtenerLosDoceAlumnosConMejorPuntajeTotal();

            return View(modelo);
        }
    }
}
        