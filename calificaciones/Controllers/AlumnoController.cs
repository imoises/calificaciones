﻿using calificaciones.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using calificaciones.Models;
using calificaciones.Entidades;

namespace calificaciones.Controllers
{
    [Authorize]
    public class AlumnoController : Controller
    {
        AlumnoService alumnoService = new AlumnoService();

        PreguntaService preguntaService = new PreguntaService();

        RespuestaService respuestaServide = new RespuestaService();

        PreguntasAlumnoService preguntaAlumnoService = new PreguntasAlumnoService();

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
            List<Pregunta> preguntas = preguntaAlumnoService.ObtenerPreguntasTipo(Id, Convert.ToInt32(Session["Id"]));

            return View(preguntas);
        }
        //[ActionName("Acerca-de")]
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
                respuestaServide.AgregarRespuesta(IdPregunta, respuesta.RespuestaTextModel.Respuesta, Convert.ToInt32(Session["Id"]));
                respuestaServide.EnviarEmailRespuestaDelAlumno(pregunta,respuesta.RespuestaTextModel.Respuesta, Convert.ToInt32(Session["Id"]));
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
        