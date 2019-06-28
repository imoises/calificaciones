using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace calificaciones.Models
{
    public class Respuesta
    {
        public int IdRespuestaAlumno { get; set; }
        public int IdPregunta { get; set; }
        public int IdAlumno { get; set; }
        public DateTime FechaHoraRespuesta { get; set; }
        public string RespuestaTxt { get;set;}
        public int Orden { get; set; }
        public int IdProfesorEvaluador { get; set; }
        public DateTime FechaHoraEvaluacion { get; set; }
        public int IdResultadoEvaluacion { get; set; }
        public int RespuestasCorrectasHastaElMomento { get; set; }
        public Int32 Puntos { get; set; }
        public Boolean MejorRespuesta { get; set; } 
    }
}