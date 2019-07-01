using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using calificaciones.Entidades;

namespace calificaciones.Services
{
    public class RespuestaService
    {
        Contexto bdContexto = new Contexto();

        public List<RespuestaAlumno> ObtenerRespuestasPorId(int Id)
        {
            return bdContexto.RespuestaAlumnoes.Include("Alumno")
                .Include("ResultadoEvaluacion").Where(r => r.Pregunta.Nro == Id).ToList();
        }

        public void AgregarRespuesta(int idPregunta, String respuesta, int idAlumno)
        {
            RespuestaAlumno respuestaAlumno = new RespuestaAlumno();

            respuestaAlumno.IdPregunta = idPregunta;
            respuestaAlumno.IdAlumno = idAlumno;
            respuestaAlumno.FechaHoraRespuesta = DateTime.Now;
            respuestaAlumno.Respuesta = respuesta;
            respuestaAlumno.Orden = bdContexto.RespuestaAlumnoes.Where(r => r.IdPregunta == idPregunta).ToList().Count + 1;

            bdContexto.RespuestaAlumnoes.Add(respuestaAlumno);

            bdContexto.SaveChanges();
        }

        public RespuestaAlumno ObtenerRespuestaPorIdRespuesta(int idRespuestaAlumno)
        {
            return bdContexto.RespuestaAlumnoes.Where(r => r.IdRespuestaAlumno == idRespuestaAlumno).FirstOrDefault();
        }
    }
}