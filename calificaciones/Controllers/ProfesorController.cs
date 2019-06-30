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
            //if (pregunta.RespuestaAlumnoes.Any())
            //ViewData["AvisoModificacion"] = "Ya se recibieron respuestas a esta pregunta, evite hacer modificaciones que puedan repercutir en las respuestas recibidas.";
            TempData["IdPregunta"] = pregunta.IdPregunta;
            ViewData["Tema"] = preguntasService.ObtenerTemaTodos();
            ViewData["Clase"] = preguntasService.ObtenerClasesTodas();
            return View(pregunta);
        }

        [HttpPost]
        public ActionResult ModificarPregunta(Pregunta pregunta)
        {
            //var idPregunta = Convert.ToInt32(TempData["IdPregunta"].ToString());
            //var pregunta = preguntasService.ObtenerUnaPorID(idPregunta);
            pregunta.IdPregunta = Convert.ToInt32(TempData["IdPregunta"].ToString());
            preguntasService.ObtenerUnaPreguntaPorId(pregunta);
            return RedirectToAction("ModificarPregunta", "Profesor", new { nro = pregunta.Nro, clase = pregunta.IdClase });
        }

        [HttpGet]
        public ActionResult EvaluarRespuestas(int nro, int clase)
        {
            var respuestaAlumnos = preguntasService.ObtenerPreguntasConRespuestas(nro, clase);
            return View(respuestaAlumnos);
            //return ViewrespuestaAlumnos
        }

        [HttpGet]
        public ActionResult EliminarPregunta(int Nro, int Clase)
        {
            /*var pregunta = preguntasService.ObtenerUnaPreguntaNroClase(Nro, Clase);
            if (!pregunta.RespuestaAlumnoes.Any())
            {
                preguntasService.EliminarPregunta(pregunta);
            }
            else
            {
                TempData["MensajeError"] = "No se puedo eliminar la pregunta Nro: "+Nro+" de  La Clase: "+ Clase;
            }
            return RedirectToAction("AdminPreguntas");*/

            var contieneRespuestas = preguntasService.EliminarPregunta(Nro, Clase);
            if (contieneRespuestas)
            {
                TempData["MensajeError"] = "No se pudo eliminar la pregunta Nro: " + Nro + " de  La Clase: " + Clase + ", porque ya contiene respuestas";
            }
            return RedirectToAction("AdminPreguntas");
        }
    }
}
