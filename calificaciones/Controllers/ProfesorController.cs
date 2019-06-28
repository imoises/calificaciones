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

        public ActionResult CrearPregunta()
        {
            /*Tema_ClaseViewModel Tyc = new Tema_ClaseViewModel();
            Tyc.TemaListado = temaClaseService.ObtenerTodosLosTemas();
            Tyc.ClaseListado = Service.ObtenerClase();
            return View(Tyc);*/
            return View();
        }

        public ActionResult ModificarPregunta()
        {
            return View();
        }

        public ActionResult EvaluarRespuestas(int id)
        {
            var preguntaRespuesta = preguntasService.ObtenerPreguntasConRespuestas(id);
            return View(preguntaRespuesta);
            //return View();
        }
    }
}