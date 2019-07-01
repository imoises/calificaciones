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

        RespuestaService respuestaServide = new RespuestaService();

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
            RespuestaAlumno respuesta = respuestaServide.ObtenerRespuestaPorIdRespuesta(IdRespuesta);

            Pregunta pregunta = preguntaService.ObtenerUnaPreguntaId(respuesta.IdPregunta);

            TempData["Respuesta"] = respuesta.Respuesta;

            return View(pregunta);
        }

        public ActionResult VerRespuesta()
        {
            return RedirectToAction("Preguntas");
        }


        [HttpPost]
        public ActionResult Responder(int IdPregunta)
        {
            Pregunta pregunta = preguntaService.ObtenerUnaPreguntaId(IdPregunta);

            return View(pregunta);
        }

        public ActionResult Responder()
        {
            return RedirectToAction("Preguntas");
        }

        [HttpPost]
        public ActionResult VerificarRespuesta(int IdPregunta, String respuesta)
        {
            Pregunta pregunta = preguntaService.ObtenerUnaPreguntaId(IdPregunta);

            if (pregunta.FechaDisponibleHasta.Value >= DateTime.Now)
            {
                respuestaServide.AgregarRespuesta(IdPregunta, respuesta, Convert.ToInt32(Session["Id"]));
                return Redirect("~/Alumno/Preguntas");
            }
            else
            {
                TempData["Respuesta"] = respuesta;
                TempData["Mensaje"] = "Ya paso la fecha de entrega";

                return View("Responder", pregunta);
            }
        }

    }
}
        