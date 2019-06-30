﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using calificaciones.Entidades;

namespace calificaciones.Services
{
    public class PreguntaService
    {
        Contexto bdContexto = new Contexto();

        //Alta
        public void PreguntaAlta(Pregunta pregunta)
        {
            var profesorService = new ProfesorService();
            var profesor = profesorService.ObtenerUnProfesor(pregunta.IdProfesorCreacion);
            pregunta.FechaHoraCreacion = DateTime.Today;
            bdContexto.Preguntas.Add(pregunta);
            bdContexto.SaveChanges();
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

        public Pregunta ObtenerUnaPreguntaClase(Pregunta pregunta)
        {
            var query = from p in bdContexto.Preguntas.Include("RespuestaAlumnoes") where p.IdPregunta == pregunta.IdPregunta select p;
            var preguntaBuscada = query.FirstOrDefault();
            return preguntaBuscada;
        }

        public Pregunta ObtenerUnaPreguntaId(int idPregunta)
        {
            var query = from p in bdContexto.Preguntas.Include("RespuestaAlumnoes") where p.IdPregunta == idPregunta select p;
            var preguntaBuscada = query.FirstOrDefault();
            return preguntaBuscada;
        }
        //no lo uso
        public Pregunta ObtenerPreguntasConRespuestas(int nro, int clase)
        {
            var query = from p in bdContexto.Preguntas where p.Nro == nro && p.IdClase == clase select p;
            var preguntaRespuesta = query.FirstOrDefault();
            return preguntaRespuesta;
        }

        public List<RespuestaAlumno> ObtenerRespuestas(int nro, int clase)
        {
            var query = from r in bdContexto.RespuestaAlumnoes where r.Pregunta.Nro == nro && r.Pregunta.IdClase == clase select r;
            var respuestas = query.ToList();
            return respuestas;
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

        public void ModificarPreguntaId(Pregunta pregunta, int idPregunta)
        {
            var preguntaModif = this.ObtenerUnaPreguntaId(idPregunta);
            preguntaModif.Nro = pregunta.Nro;
            preguntaModif.IdTema = pregunta.IdTema;
            preguntaModif.FechaDisponibleDesde = pregunta.FechaDisponibleDesde;
            preguntaModif.FechaDisponibleHasta = pregunta.FechaDisponibleHasta;
            preguntaModif.Pregunta1 = pregunta.Pregunta1;
            bdContexto.SaveChanges();
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

        public RespuestaAlumno ObtenerUnaRespuestaId(int respuesta)
        {
            //var query = from p in bdContexto.Preguntas.Include("RespuestaAlumnoes").Include("Profesor") where p.Nro == Nro && p.IdClase == Clase select p;
            var query = from r in bdContexto.RespuestaAlumnoes where r.IdRespuestaAlumno == respuesta select r;
            var respuestaBuscada = query.FirstOrDefault();
            return respuestaBuscada;
        }

        public ResultadoEvaluacion ObtenerResultadoEvaluacionId(int valor)
        {
            var query = from e in bdContexto.ResultadoEvaluacions where e.IdResultadoEvaluacion == valor select e;
            var resultadoEvaluacion = query.FirstOrDefault();
            return resultadoEvaluacion;
        }

        public List<RespuestaAlumno> ObtenerRespuestasCorrectas(int idPregunta)
        {
            var query = from r in bdContexto.RespuestaAlumnoes where r.IdResultadoEvaluacion == 1 && r.IdPregunta == idPregunta select r;
            var respuestasCorrecta = query.ToList();
            return respuestasCorrecta;
        }

        public bool RespuestaValorar(int respuesta, int valor, int profesor)
        {
            var respuestaAlumno = this.ObtenerUnaRespuestaId(respuesta);
            var resultadoEvaluacion = this.ObtenerResultadoEvaluacionId(valor);

            //si no devuelve respuesta ni evaluacion null, entonces se procede a actualizar la tabla RespuestaAlumno
            if(respuestaAlumno != null && resultadoEvaluacion != null)
            {   
                //se inicializan las variables a utilizar
                int puntajeMax = Convert.ToInt32(WebConfigurationManager.AppSettings["PuntajeMaximoPorRespuestaCorrecta"]);
                int cupo = Convert.ToInt32(WebConfigurationManager.AppSettings["CupoMaximoRespuestasCorrectas"]);
                var cantidadRespuestasCorrecta = this.ObtenerRespuestasCorrectas(respuestaAlumno.IdPregunta).Count();
                int puntajeObtenido = 0;

                //cálculo de puntaje por respuesta
                switch (valor)
                {
                    case 1:
                        if (cantidadRespuestasCorrecta <= 10)
                        {
                            puntajeObtenido = puntajeMax - (puntajeMax / cupo * cantidadRespuestasCorrecta);
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
                        }
                        else
                        {
                            puntajeObtenido = puntajeMax / cupo / 2;
                        }
                    break;
                }
                //se procede a actualizar la tabla RespuestaAlumno
                respuestaAlumno.IdProfesorEvaluador = profesor;
                respuestaAlumno.FechaHoraEvaluacion = DateTime.Today;
                respuestaAlumno.IdResultadoEvaluacion = resultadoEvaluacion.IdResultadoEvaluacion;
                respuestaAlumno.RespuestasCorrectasHastaElMomento = cantidadRespuestasCorrecta;
                respuestaAlumno.Puntos = puntajeObtenido;
                bdContexto.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Tema> ObtenerTemaTodos()
        {
            var temaLista = bdContexto.Temas.ToList();
            return temaLista;
        }
        public List<Clase> ObtenerClasesTodas()
        {
            var claseLista = bdContexto.Clases.ToList();
            return claseLista;
        }

    }
}

