using calificaciones.Services;
using calificaciones.Entidades;
using calificaciones.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace calificaciones.Controllers
{
    public class ProfesorController : Controller
    {
        PreguntaService preguntasService = new PreguntaService();
        TemaClaseService temaClaseService = new TemaClaseService();
        RespuestaService respuestaService = new RespuestaService();
        // GET: Profesor 

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AdminPreguntas()
        {
            var ListaPreguntas = preguntasService.ObtenerTodasLasPreguntas();
            return View(ListaPreguntas);
        }

        [HttpGet]
        public ActionResult CrearPregunta()
        {
            //Tema_ClaseViewModel Tyc = new Tema_ClaseViewModel();
            ViewData["Nro"] = preguntasService.ObtenerNroUltimaPregunta() + 1;
            ViewData["Tema"] = preguntasService.ObtenerTemaTodos();
            ViewData["Clase"] = preguntasService.ObtenerClasesTodas();
            return View();
        }

        [HttpPost]
        public ActionResult CrearPregunta(Pregunta pregunta)
        {
            pregunta.IdProfesorCreacion = Convert.ToInt32(Session["Id"]);
            preguntasService.PreguntaAlta(pregunta);
            return RedirectToAction("CrearPregunta");
        }

        [HttpGet]
        public ActionResult ModificarPregunta(int Nro, int Clase)
        {
            var pregunta = preguntasService.ObtenerUnaPreguntaNroClase(Nro, Clase);
            if (pregunta.RespuestaAlumnoes.Any())
                ViewData["AvisoModificacion"] = "Ya se recibieron respuestas a esta pregunta, evite hacer modificaciones que puedan repercutir en las respuestas recibidas.";
            ViewData["Tema"] = preguntasService.ObtenerTemaTodos();
            ViewData["Clase"] = preguntasService.ObtenerClasesTodas();
            return View(pregunta);
        }

        [HttpPost]
        public ActionResult ModificarPregunta(Pregunta pregunta)
        {
            //var pregunta = preguntasService.ObtenerUnaPorID(id);
            preguntasService.ModificarPregunta(pregunta);
            return RedirectToAction("ModificarPregunta", "Profesor", new { Nro = pregunta.Nro, Clase = pregunta.IdClase });
        }

        public ActionResult EvaluarRespuestas(int id)
        {
            var preguntaRespuesta = preguntasService.ObtenerPreguntasConRespuestas(id);
            return View(preguntaRespuesta);
            //return View();
        }
    }
}
