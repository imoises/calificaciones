using calificaciones.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using calificaciones.Models;
using calificaciones.Entidades;
using calificaciones.Session;

namespace calificaciones.Controllers
{
    [AuthorizeAlumno]
    public class AlumnoController : Controller
    {
        AlumnoService alumnoService = new AlumnoService();

        PreguntaService preguntaService = new PreguntaService();

        RespuestaService respuestaServide = new RespuestaService();

        PreguntasAlumnoService preguntaAlumnoService = new PreguntasAlumnoService();

        // GET: Alumno
        public ActionResult Inicio()
        {
            if(SessionManagement.IdUsuario != null && SessionManagement.Nombre != null && SessionManagement.Rol != null)
            {
                InicioAlumnoViewModel modelo = new InicioAlumnoViewModel();

                modelo.UltimasPreguntasCorregidas = alumnoService.ObtenerPreguntasUltimaClase();

                modelo.TablaDePosiciones = alumnoService.ObtenerLosDoceAlumnosConMejorPuntajeTotal();

                modelo.PreguntasSinResponder = preguntaAlumnoService.ObtenerTodasLasPreguntasSinResponder(Convert.ToInt32(Session["Id"]));

                return View(modelo);
            }
            return Redirect("~/");
        }
        
        public ActionResult Preguntas(String Id)
        {
            // ObtenerPreguntasTipo (Todas, Sin Corregir, Correctas,Regular ó Mal)
            List<Pregunta> preguntas = preguntaAlumnoService.ObtenerPreguntasTipo(Id, SessionManagement.IdUsuario);
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
            ResponderViewModel responderViewModel = new ResponderViewModel();
            responderViewModel.Pregunta = preguntaService.ObtenerUnaPreguntaId(IdPregunta);
            return View(responderViewModel);
        }

        public ActionResult Responder()
        {
            return RedirectToAction("Preguntas");
        }

        [HttpPost]
        public ActionResult VerificarRespuesta(int IdPregunta, ResponderViewModel respuesta)
        {
            Pregunta pregunta = preguntaService.ObtenerUnaPreguntaId(IdPregunta);
            if (pregunta.FechaDisponibleHasta.Value >= DateTime.Now && respuesta.RespuestaTextModel.Respuesta != null)
            {
                respuestaServide.AgregarRespuesta(IdPregunta, respuesta.RespuestaTextModel.Respuesta, SessionManagement.IdUsuario);
                respuestaServide.EnviarEmailRespuestaDelAlumno(pregunta, respuesta.RespuestaTextModel.Respuesta, SessionManagement.IdUsuario);
                return Redirect("~/Alumno/Preguntas");
            }
            else
            {
                ResponderViewModel responderViewModel = new ResponderViewModel();
                responderViewModel.Pregunta = pregunta;
                TempData["Respuesta"] = respuesta;
                TempData["Mensaje"] = "Ya paso la fecha de entrega";
                return View("Responder", responderViewModel);
            }
        }

    }
}
        