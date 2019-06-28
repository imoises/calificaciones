using calificaciones.Services;
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

        public ActionResult EvaluarRespuestas(int Id)
        {
            /*var RespuestaService = new RespuestaService();
            var ListaRespuestas = RespuestaService.ObtenerRespuestas(Id);
            return View(ListaRespuestas);*/
            return View();
        }
    }
}