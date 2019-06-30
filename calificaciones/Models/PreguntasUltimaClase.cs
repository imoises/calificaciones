using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace calificaciones.Models
{
    public class PreguntasUltimaClase
    {
        public String NombreUltimaClase { get; set; }
        public String Pregunta { get; set; }
        public List<PosAlumnoPuntos> PosAlumnosPuntos = new List<PosAlumnoPuntos>();
    }
}