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

        public List<Pregunta> ObtenerTodasLasPreguntas()
        {
            return bdContexto.Preguntas.Include("Clase").Include("Tema").ToList();
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
