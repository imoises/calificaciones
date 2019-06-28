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
    }
}