using calificaciones.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using calificaciones.Models;
using calificaciones.Entidades;

namespace calificaciones.Controllers
{
    public class AlumnoController : Controller
    {
        AlumnoService alumnoService = new AlumnoService();

        PreguntaService preguntaService = new PreguntaService();

        // GET: Alumno
        public ActionResult Inicio()
        {
            if (Session["Id"]!=null && Session["Nombre"]!=null && Session["Rol"] != null)
            {
                InicioAlumnoViewModel modelo = new InicioAlumnoViewModel();

                modelo.UltimasPreguntasCorregidas = alumnoService.ObtenerPreguntasUltimaClase();

                modelo.TablaDePosiciones = alumnoService.ObtenerLosDoceAlumnosConMejorPuntajeTotal();

                return View(modelo);
            }

            return Redirect("~/");
            
        }

        [HttpGet]
        public ActionResult Preguntas(String Id)
        {
            // ObtenerPreguntasTipo (Todas, Sin Corregir, Correctas,Regular ó Mal)
            List<Pregunta> preguntas = preguntaService.ObtenerPreguntasTipo(Id);

            return View(preguntas);
        }

        public ActionResult AcercaDe()
        {
            return View();
        }

        [HttpPost]
        public ActionResult VerRespuesta(int IdRespuesta)
        {
            return View();
        }


        [HttpPost]
        public ActionResult Responder(int IdPregunta)
        {
            return View();
        }
    }
}
        