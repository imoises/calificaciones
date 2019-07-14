using calificaciones.Services;
using calificaciones.Entidades;
using calificaciones.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using calificaciones.Session;

namespace calificaciones.Controllers
{
    [AuthorizeProfesor]
    public class ProfesorController : Controller
    {
        PreguntaService preguntasService = new PreguntaService();
        TemaClaseService temaClaseService = new TemaClaseService();
        RespuestaService respuestaService = new RespuestaService();
        ProfesorService profesorService = new ProfesorService();
        // GET: Profesor 

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Preguntas()
        {
            var ListaPreguntas = preguntasService.ObtenerTodasLasPreguntas();
            return View(ListaPreguntas);
        }

        [HttpGet]
        public ActionResult Crear()
        {
            ViewData["Nro"] = preguntasService.ObtenerNroUltimaPregunta() + 1;
            ViewData["Tema"] = temaClaseService.ObtenerTodosLosTemas();
            ViewData["Clase"] = temaClaseService.ObtenerTodasLasClase();
            return View();
        }

        [HttpPost]
        public ActionResult Crear(Pregunta pregunta)
        {
            if (ModelState.IsValid)
            {
                pregunta.IdProfesorCreacion = SessionManagement.IdUsuario;
                if (preguntasService.PreguntaAlta(pregunta))
                {
                    TempData["Mensaje"] = "<p class='mb-0 text-success'> La pregunta se creó correctamente </p>";
                }
                else
                {
                    TempData["Mensaje"] = "<p class='mb-0 text-danger'> Ya existe una pregunta con el Nro: " + pregunta.Nro + " y Clase: " + pregunta.IdClase + " </p>";
                }
            }           
            return RedirectToAction("Crear");
        }

        [HttpGet]
        public ActionResult ModificarPregunta(int nro, int clase)
        {
            var pregunta = preguntasService.ObtenerUnaPreguntaNroClase(nro, clase);
            if (ModelState.IsValid)
            {
                TempData["IdPregunta"] = pregunta.IdPregunta;
                ViewData["Tema"] = temaClaseService.ObtenerTodosLosTemas();
                ViewData["Clase"] = temaClaseService.ObtenerTodasLasClase();
            }
            return View(pregunta);
        }

        [HttpPost]
        public ActionResult ModificarPregunta(Pregunta pregunta)
        {
            var IdPregunta = Convert.ToInt32(TempData["IdPregunta"].ToString());
            if (ModelState.IsValid)
            {
                if (preguntasService.ModificarPreguntaId(pregunta, IdPregunta))
                {
                    TempData["Mensaje"] = "<p class='mb-0 text-info'> La pregunta se modificó correctamente </p>";
                }
                else
                {
                    TempData["Mensaje"] = "<p class='mb-0 text-danger'> Ya existe una pregunta con el Nro: " + pregunta.Nro + " y Clase: " + pregunta.IdClase + " </p>";
                }
            }
            return RedirectToAction("ModificarPregunta", "Profesor", new { nro = pregunta.Nro, clase = pregunta.IdClase });
        }

        [HttpGet]
        public ActionResult Respuestas(String tipo) // Id contiene el valor tipo: Todas, SinCorregir, Correctas, Regular o Mal
        {
            var respuestaAlumnos = respuestaService.ObtenerRespuestasTodas();
            var listaRespuestaAlumno = respuestaService.FiltroRespuesta(respuestaAlumnos, tipo);
            return View(listaRespuestaAlumno);
        }

        [HttpGet]
        public ActionResult EvaluarRespuestas(int nro, string clase, String tipo)
        {
            var respuestaAlumnos = preguntasService.ObtenerPreguntasConRespuestas(nro, clase);
            var idPregunta = respuestaAlumnos.IdPregunta;
            var listaRespuestaAlumno = respuestaService.FiltroRespuesta(respuestaAlumnos, tipo);
            TempData["IdPregunta"] = idPregunta;
            ViewData["SinEvaluar"] = respuestaService.ObtenerSinEvaluar(idPregunta);
            ViewData["MejorPregunta"] = respuestaService.ObtenerSiMejor(idPregunta);
            return View(listaRespuestaAlumno);
        }

        [HttpGet]
        public ActionResult EvaluarRespuestasGo(int respuesta, int valor)//int respuesta es el id de la respuesta... :( | int valor es la valoración de la respuesta: Correcta/Regular/Mal
        {
            var profesor = SessionManagement.IdUsuario;
            var resultado = preguntasService.RespuestaValorar(respuesta, valor, profesor);
            if (resultado)
            {
                TempData["Mensaje"] = "Se calificó la respuesta";
            }
            else
            {
                TempData["Mensaje"] = "Hubo un error";
            }
            var IdPregunta = Convert.ToInt32(TempData["IdPregunta"].ToString());
            var preguntaBuscada = preguntasService.ObtenerUnaPreguntaId(IdPregunta);
            return RedirectToAction("EvaluarRespuestas", "Profesor", new { nro = preguntaBuscada.Nro, clase = preguntaBuscada.Clase.Nombre });
        }

        [HttpGet]
        public ActionResult MejorRespuesta(int respuesta)
        {
            var resultado = preguntasService.valorarMejorRespuesta(respuesta);
            if (resultado)
            {
                TempData["Mensaje"] = "<p class='mb-0 text-info'> La mejor respuesta se asignó correctamente </p>";
            }
            else
            {
                TempData["Mensaje"] = "<p class='mb-0 text-danger'> No se pudo realizar la acción </p>";
            }
            var IdPregunta = Convert.ToInt32(TempData["IdPregunta"].ToString());
            var preguntaBuscada = preguntasService.ObtenerUnaPreguntaId(IdPregunta);
            return RedirectToAction("EvaluarRespuestas", "Profesor", new { nro = preguntaBuscada.Nro, clase = preguntaBuscada.Clase.Nombre });
        }

        [HttpGet]
        public ActionResult EliminarPregunta(int nro, string clase)
        {
            var respuestaAlumnos = preguntasService.ObtenerPreguntasConRespuestas(nro, clase);
            var contieneRespuestas = preguntasService.EliminarPregunta(nro, clase);
            if (contieneRespuestas)
            {
                TempData["MensajeError"] = "No se pudo eliminar la pregunta Nro: " + nro + " de  La Clase: " + clase + ", porque ya contiene respuestas";
            }
            return RedirectToAction("Preguntas");
        }

        public ActionResult AcercaDe()
        {
            return View();
        }

    }
}
