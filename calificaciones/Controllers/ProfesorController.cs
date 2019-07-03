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
    [Authorize]
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

        public ActionResult Preguntas(int pagina = 1)
        {
            Paginador<Pregunta> paginador = preguntasService.PaginadorPreguntas(pagina);
            return View(paginador);
        }

        [HttpPost]
        public ActionResult Preguntas()
        {
            var ListaPreguntas = preguntasService.ObtenerTodasLasPreguntas();
            return View(ListaPreguntas);
        }

        [HttpGet]
        public ActionResult Crear()
        {
            //Tema_ClaseViewModel Tyc = new Tema_ClaseViewModel();
            ViewData["Nro"] = preguntasService.ObtenerNroUltimaPregunta() + 1;
            ViewData["Tema"] = temaClaseService.ObtenerTodosLosTemas();
            ViewData["Clase"] = temaClaseService.ObtenerTodasLasClase();
            return View();
        }

        [HttpPost]
        public ActionResult Crear(Pregunta pregunta)
        {
            pregunta.IdProfesorCreacion = Convert.ToInt32(Session["Id"]);
            if (preguntasService.PreguntaAlta(pregunta))
            {
                TempData["Mensaje"] = "<p class='mb-0 text-success'> La pregunta se creó correctamente </p>";
            }
            else
            {
                TempData["Mensaje"] = "<p class='mb-0 text-danger'> Ya existe una pregunta con el Nro: "+pregunta.Nro+" y Clase: "+pregunta.IdClase+" </p>";
            }
            return RedirectToAction("Crear");
        }

        [HttpGet]
        public ActionResult ModificarPregunta(int Nro, int Clase)
        {
            var pregunta = preguntasService.ObtenerUnaPreguntaNroClase(Nro, Clase);
            TempData["IdPregunta"] = pregunta.IdPregunta;
            ViewData["Tema"] = temaClaseService.ObtenerTodosLosTemas();
            ViewData["Clase"] = temaClaseService.ObtenerTodasLasClase();
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
        public ActionResult Respuestas()//lo agregué de última
        {
            var respuestaAlumnos = respuestaService.ObtenerRespuestasTodas();
            return View(respuestaAlumnos);
        }

        public ActionResult Respuestas(int pagina = 0)
        {
            Paginador<Pregunta> paginador = preguntasService.PaginadorPreguntas(pagina);
            return View(paginador);
        }

        [HttpPost]
        public ActionResult EvaluarRespuestas(int idPregunta, String tipo)
        {
            EvaluarRespuestasViewModel evaluarRespuestasViewModel = new EvaluarRespuestasViewModel();
            evaluarRespuestasViewModel.Pregunta = preguntasService.ObtenerUnaPreguntaId(idPregunta);
            evaluarRespuestasViewModel.ListaRespuestaAlumno = respuestaService.ObtenerRespuestasAlumnoTipo(idPregunta, tipo);
            
            ViewData["SinEvaluar"] = respuestaService.ObtenerRespuestasSinEvaluar(idPregunta);
            ViewData["MejorPregunta"] = respuestaService.ObtenerSiMejorRespuesta(idPregunta);

            return View(evaluarRespuestasViewModel);
        }

        [HttpGet]
        public ActionResult EvaluarRespuestasGo(int respuesta, int valor)//int respuesta es el id de la respuesta... :( | int valor es la valoración de la respuesta: Correcta/Regular/Mal
        {
            var profesor = Convert.ToInt32(Session["Id"]);
            var resultado = preguntasService.RespuestaValorar(respuesta, valor, profesor);
            if (resultado)
            {
                TempData["Mensaje"] = "Se calificó la respuesta";
            }
            else
            {
                TempData["Mensaje"] = "Hubo un error";
            }
            int idPregunta = respuestaService.ObtenerUnaRespuestaId(respuesta).IdPregunta;
            EvaluarRespuestasViewModel evaluarRespuestasViewModel = new EvaluarRespuestasViewModel();
            evaluarRespuestasViewModel.Pregunta = preguntasService.ObtenerUnaPreguntaId(idPregunta);
            evaluarRespuestasViewModel.ListaRespuestaAlumno = respuestaService.ObtenerRespuestasAlumnoTipo(idPregunta, "Todas");
            return View("EvaluarRespuestas", evaluarRespuestasViewModel);
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
            return RedirectToAction("EvaluarRespuestas", "Profesor", new { nro = preguntaBuscada.Nro, clase = preguntaBuscada.IdClase });
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
            return RedirectToAction("Preguntas");
        }

        public ActionResult AcercaDe()
        {
            return View();
        }

    }
}
