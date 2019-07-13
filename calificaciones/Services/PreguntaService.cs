using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using calificaciones.Entidades;
using calificaciones.Models;

namespace calificaciones.Services
{
    public class PreguntaService
    {
        Contexto bdContexto = new Contexto();
        AlumnoService alumnoService = new AlumnoService();
        RespuestaService respuestaService = new RespuestaService();

        //Alta
        public bool PreguntaAlta(Pregunta pregunta)
        {
            var preguntaExistente = this.ObtenerUnaPreguntaNroClase(pregunta.Nro, pregunta.IdClase);
            if(preguntaExistente == null)
            {
                var profesorService = new ProfesorService();
                var profesor = profesorService.ObtenerUnProfesor(pregunta.IdProfesorCreacion);
                pregunta.FechaHoraCreacion = DateTime.Today;
                bdContexto.Preguntas.Add(pregunta);
                bdContexto.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
            
        }

        //Obtener
        public List<Pregunta> ObtenerTodasLasPreguntas()
        {
            return bdContexto.Preguntas.Include("Clase").Include("Tema").ToList();
        }

        public Pregunta ObtenerUnaPreguntaNroClase(int Nro, int Clase)
        {
            var query = from p in bdContexto.Preguntas.Include("RespuestaAlumnoes").Include("Profesor") where p.Nro == Nro && p.IdClase == Clase select p;
            var preguntaBuscada = query.FirstOrDefault();
            return preguntaBuscada;
        }

        public Pregunta ObtenerUnaPreguntaId(int idPregunta)
        {
            var query = from p in bdContexto.Preguntas.Include("RespuestaAlumnoes") where p.IdPregunta == idPregunta select p;
            var preguntaBuscada = query.FirstOrDefault();
            return preguntaBuscada;
        }

        public Pregunta ObtenerPreguntasConRespuestas(int nro, string clase)
        {
            var query = from p in bdContexto.Preguntas.Include("Clase") where p.Nro == nro && p.Clase.Nombre == clase select p;
            var preguntaRespuesta = query.FirstOrDefault();
            return preguntaRespuesta;
        }

        public int ObtenerNroUltimaPregunta()
        {
            var query = from p in bdContexto.Preguntas orderby p.Nro descending select p;
            var nroPregunta = query.First().Nro;
            return nroPregunta;
        }


        //Modificar
        public void ModificarPregunta(Pregunta pregunta)
        {
            var preguntaModif = this.ObtenerUnaPreguntaNroClase(pregunta.Nro, pregunta.IdClase);
            preguntaModif.Nro = pregunta.Nro;
            preguntaModif.IdTema = pregunta.IdTema;
            preguntaModif.FechaDisponibleDesde = pregunta.FechaDisponibleDesde;
            preguntaModif.FechaDisponibleHasta = pregunta.FechaDisponibleHasta;
            preguntaModif.Pregunta1 = pregunta.Pregunta1;
            bdContexto.SaveChanges();
        }

        public bool ModificarPreguntaId(Pregunta pregunta, int idPregunta)
        {
            var preguntaExistente = this.ObtenerUnaPreguntaNroClase(pregunta.Nro, pregunta.IdClase);
            var preguntaModif = this.ObtenerUnaPreguntaId(idPregunta);
            if (preguntaExistente == null || preguntaExistente.IdPregunta == preguntaModif.IdPregunta)
            {
                preguntaModif.Nro = pregunta.Nro;
                preguntaModif.IdTema = pregunta.IdTema;
                preguntaModif.FechaDisponibleDesde = pregunta.FechaDisponibleDesde;
                preguntaModif.FechaDisponibleHasta = pregunta.FechaDisponibleHasta;
                preguntaModif.Pregunta1 = pregunta.Pregunta1;
                bdContexto.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        //Eliminar
        public bool EliminarPregunta(int Nro, int Clase)
        {
            var preguntaEliminar = this.ObtenerUnaPreguntaNroClase(Nro, Clase);
            var contieneRespuestas = preguntaEliminar.RespuestaAlumnoes.Any();
            if (!contieneRespuestas)
            {
                bdContexto.Preguntas.Remove(preguntaEliminar);
                bdContexto.SaveChanges();
            }
            
            return contieneRespuestas;
        }

        //Otras
        public ResultadoEvaluacion ObtenerResultadoEvaluacionId(int valor)
        {
            var query = from e in bdContexto.ResultadoEvaluacions where e.IdResultadoEvaluacion == valor select e;
            var resultadoEvaluacion = query.FirstOrDefault();
            return resultadoEvaluacion;
        }

        public bool valorarMejorRespuesta(int respuesta)
        {
            int puntajeMax = Convert.ToInt32(WebConfigurationManager.AppSettings["PuntajeMaximoPorRespuestaCorrecta"]);
            var respuestaAlumno = respuestaService.ObtenerUnaRespuestaId(respuesta);
            if(respuestaAlumno != null)
            {
                var alumno = respuestaAlumno.Alumno;
                respuestaAlumno.MejorRespuesta = true;
                alumno.CantidadMejorRespuesta += 1;
                alumno.PuntosTotales += puntajeMax / 2;
                bdContexto.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        //QUIZÁ DEBERÍA IR EN OTRO SERVICE
        public bool RespuestaValorar(int respuesta, int valor, int profesor)
        {
            var respuestaAlumno = respuestaService.ObtenerUnaRespuestaId(respuesta);
            var resultadoEvaluacion = this.ObtenerResultadoEvaluacionId(valor);
            var alumno = respuestaAlumno.Alumno;
            //si no devuelve respuesta ni evaluacion null, entonces se procede a actualizar la tabla RespuestaAlumno
            if(respuestaAlumno != null && resultadoEvaluacion != null)
            {   
                //se inicializan las variables a utilizar
                int puntajeMax = Convert.ToInt32(WebConfigurationManager.AppSettings["PuntajeMaximoPorRespuestaCorrecta"]);
                int cupo = Convert.ToInt32(WebConfigurationManager.AppSettings["CupoMaximoRespuestasCorrectas"]);
                var cantidadRespuestasCorrecta = respuestaService.ObtenerRespuestasCorrectas(respuestaAlumno.IdPregunta).Count();
                int puntajeObtenido = 0;

                //cálculo de puntaje por respuesta
                switch (valor)
                {
                    case 1:
                        if (cantidadRespuestasCorrecta <= 10)
                        {
                            puntajeObtenido = puntajeMax - (puntajeMax / cupo * cantidadRespuestasCorrecta);
                            alumno.CantidadRespuestasCorrectas += 1;
                        }
                        else
                        {
                            puntajeObtenido = puntajeMax / cupo;
                        }
                    break;
                    case 2:
                        if (cantidadRespuestasCorrecta <= 10)
                        {
                            puntajeObtenido = (puntajeMax - (puntajeMax / cupo * cantidadRespuestasCorrecta))/2;
                            alumno.CantidadRespuestasRegular += 1;
                        }
                        else
                        {
                            puntajeObtenido = puntajeMax / cupo / 2;
                        }
                    break;
                    case 3:
                            alumno.CantidadRespuestasMal += 1;
                    break;
                }
                //se procede a actualizar la tabla RespuestaAlumno
                respuestaAlumno.IdProfesorEvaluador = profesor;
                respuestaAlumno.FechaHoraEvaluacion = DateTime.Today;
                respuestaAlumno.IdResultadoEvaluacion = resultadoEvaluacion.IdResultadoEvaluacion;
                respuestaAlumno.RespuestasCorrectasHastaElMomento = cantidadRespuestasCorrecta;
                respuestaAlumno.Puntos = puntajeObtenido;


                alumno.PuntosTotales += puntajeObtenido;
                bdContexto.SaveChanges();  
                respuestaService.Guardar();

                return true;
            }
            else
            {
                return false;
            }
        }

        public Paginador<Pregunta> PaginadorPreguntas(int pagina)
        {
            pagina = 1;
            int RegistrosPorPagina = 10;
            int TotalRegistros = 0;
            List<Pregunta> ListaPreguntas = null;
            Paginador<Pregunta> pagPreg = null;
            TotalRegistros = bdContexto.Preguntas.Count();
            ListaPreguntas = bdContexto.Preguntas.OrderBy(p => p.Nro)
                .Skip((pagina - 1) * RegistrosPorPagina)
                .Take(RegistrosPorPagina)
                .ToList();
            var TotalPaginas = (int) Math.Ceiling((double)TotalRegistros / RegistrosPorPagina);
            pagPreg = new Paginador<Pregunta>()
            {
                RegistrosPorPagina = RegistrosPorPagina,
                TotalRegistros = TotalRegistros,
                TotalPaginas = TotalPaginas,
                PaginaActual = pagina,
                Resultado = ListaPreguntas
            };
            return pagPreg;
        }
    }
}

