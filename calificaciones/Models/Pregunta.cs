using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace calificaciones.Models
{
    public class Pregunta
    {
        public int Nro { get; set; }
        public int IdClase { get; set; }
        public int IdTema { get; set; }
        public DateTime FechaDisponibleDesde { get; set; }
        public DateTime FechaDisponibleHasta { get; set; }
        public string PreguntaTxt { get; set; }
        public int IdProfesorCreacion { get; set; }
        public DateTime FechaHoraCreacion { get; set; }
        public int IdProfesorModificacion { get; set; }
        public DateTime FechaHoraModificacion { get; set; }
    }
}