using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            var profesor = profesorService.ObtenerUnProfesor(1);
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

        public Pregunta ObtenerPreguntasConRespuestas(int id)
        {
            var query = from p in bdContexto.Preguntas.Include("Tema").Include("Clase") where p.IdPregunta == id select p;
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

