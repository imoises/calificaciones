using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using calificaciones.Entidades;
using calificaciones.Models;

namespace calificaciones.Services
{
    public class AlumnoService
    {
        Contexto bdContexto = new Contexto();

        public Alumno ObtenerUnAlumno(int id)
        {
            return bdContexto.Alumnoes.FirstOrDefault(alumno => alumno.IdAlumno == id);
        }

        public List<Alumno> ObtenerTodosLosAlumnos()
        {
            return bdContexto.Alumnoes.ToList();
        }

        public bool verificarAlumnoRegistrado(Alumno alumno)
        {
            var alumnoRegistrado = bdContexto.Alumnoes.Where(alu => alu.Email == alumno.Email).FirstOrDefault();
            if(alumnoRegistrado != null)
            {
                return true;
            }
            return false;
        }

        public bool verificarEmailExistenteDeUnAlumno(Alumno alumno)
        {
            Alumno alumnoEmail = bdContexto.Alumnoes.Where(alu => alu.Email == alumno.Email).FirstOrDefault();
            if(alumnoEmail != null)
            {
                return true;
            }
            return false;
        }

        public List<PreguntasUltimaClase> ObtenerPreguntasUltimaClase()
        {
            List<PreguntasUltimaClase> listPreguntasUltimaClase = new List<PreguntasUltimaClase>();

            var preguntas = bdContexto.Preguntas.Include("RespuestaAlumnoes").ToList();

            List<Pregunta> ultimasPreguntas = preguntas.OrderByDescending(p => p.FechaDisponibleDesde).Where(p => p.RespuestaAlumnoes.Where(r => r.Puntos != null).Count() > 0).Take(2).ToList(); //obtengo las dos ultimas las preguntas con respuestas corregidas. 

            foreach (var pregunta in ultimasPreguntas)
            {
                PreguntasUltimaClase ultimasPreguntascorregidas = new PreguntasUltimaClase();

                ultimasPreguntascorregidas.NombreUltimaClase = pregunta.Clase.Nombre;
                ultimasPreguntascorregidas.Pregunta = pregunta.Pregunta1;

                int posicion = 1;
                var respuestas = pregunta.RespuestaAlumnoes.OrderBy(r => r.FechaHoraRespuesta).Take(12).ToList(); //obtengo las 12 primeras respuestas para la ultima pregunta.
                foreach (var respuesta in respuestas)
                {
                    PosAlumnoPuntos pap = new PosAlumnoPuntos();
                    pap.Posicion = posicion;
                    pap.Nombre = respuesta.Alumno.Nombre;
                    pap.Apellido = respuesta.Alumno.Apellido;
                    pap.Puntos = respuesta.Puntos.Value;
                    posicion++;

                    ultimasPreguntascorregidas.PosAlumnosPuntos.Add(pap);
                }

                listPreguntasUltimaClase.Add(ultimasPreguntascorregidas);
            }

            return listPreguntasUltimaClase;
        }

        public List<Alumno> ObtenerLosDoceAlumnosConMejorPuntajeTotal()
        {
            return bdContexto.Alumnoes.OrderByDescending(a => a.PuntosTotales).Take(12).ToList();
        }

    }
}