using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using calificaciones.Entidades;

namespace calificaciones.Services
{
    public class PreguntasAlumnoService
    {
        Contexto bdContexto = new Contexto();

        public List<Pregunta> ObtenerPreguntasTipo(String tipo, int idAlumno)
        {
            switch (tipo)
            {
                case "SinCorregir":
                    return ObtenerPreguntasConRespuestaSinCorregir(idAlumno);
                case "Correctas":
                    return ObtenerPreguntasConRespustaCorrecta(idAlumno);
                case "Regular":
                    return ObtenerPreguntasConRespustaRegular(idAlumno);
                case "Mal":
                    return ObtenerPreguntasConRespustaMal(idAlumno);
                default:
                    return ObtenerTodasLasPreguntasPublicadas();
            }
        }

        public List<Pregunta> ObtenerTodasLasPreguntasPublicadas()
        {
            List<Pregunta> listPreguntasPublicadas = new List<Pregunta>();

            listPreguntasPublicadas = bdContexto.Preguntas.Include("Clase").Include("Tema").Where(p => p.FechaDisponibleDesde != null).OrderByDescending(p => p.Nro).ToList();

            return listPreguntasPublicadas;
        }

        public List<Pregunta> ObtenerPreguntasConRespuestaSinCorregir(int idAlumno)
        {
            List<Pregunta> listaPreguntasSinCorregir = new List<Pregunta>();

            var varpreguntas = bdContexto.Preguntas.Where(p => p.FechaDisponibleDesde != null).OrderByDescending(p => p.Nro).ToList();

            listaPreguntasSinCorregir = varpreguntas.Where(p => p.RespuestaAlumnoes.Where(r => r.IdAlumno == idAlumno && r.Puntos == null).ToList().Count > 0).ToList();

            return listaPreguntasSinCorregir;
        }

        public List<Pregunta> ObtenerPreguntasConRespustaCorrecta(int idAlumno)
        {
            List<Pregunta> listaPreguntasCorrectas = new List<Pregunta>();

            var varRespuestas = bdContexto.RespuestaAlumnoes.Where(r => r.IdResultadoEvaluacion == 1 && r.IdAlumno == idAlumno).ToList();
            foreach (var item in varRespuestas)
            {
                listaPreguntasCorrectas.Add(item.Pregunta);
            }
            return listaPreguntasCorrectas;
        }

        public List<Pregunta> ObtenerPreguntasConRespustaRegular(int idAlumno)
        {
            List<Pregunta> listaPreguntasRegular = new List<Pregunta>();

            var varRespuestas = bdContexto.RespuestaAlumnoes.Where(r => r.IdResultadoEvaluacion == 2 && r.IdAlumno == idAlumno).ToList();
            foreach (var item in varRespuestas)
            {
                listaPreguntasRegular.Add(item.Pregunta);
            }
            return listaPreguntasRegular;
        }

        public List<Pregunta> ObtenerPreguntasConRespustaMal(int idAlumno)
        {
            List<Pregunta> listaPreguntasMal = new List<Pregunta>();
            var varRespuestas = bdContexto.RespuestaAlumnoes.Where(r => r.IdResultadoEvaluacion == 3 && r.IdAlumno== idAlumno).ToList();
            foreach (var item in varRespuestas)
            {
                listaPreguntasMal.Add(item.Pregunta);
            }

            return listaPreguntasMal;
        }

        public List<Pregunta> ObtenerTodasLasPreguntasSinResponder(int idAlumno)
        {
            List<Pregunta> preguntas = bdContexto.Preguntas.Where(p => p.RespuestaAlumnoes.Where(r => r.IdAlumno == idAlumno).Count() == 0).OrderByDescending(p => p.Nro).ToList();

            return preguntas;
        }
    }
}