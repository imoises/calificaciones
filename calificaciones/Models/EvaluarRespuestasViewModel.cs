using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using calificaciones.Entidades;

namespace calificaciones.Models
{
    public class EvaluarRespuestasViewModel
    {
        public Pregunta Pregunta { get; set; }
        public List<RespuestaAlumno> ListaRespuestaAlumno { get; set; }
    }
}