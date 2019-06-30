using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using calificaciones.Entidades;

namespace calificaciones.Models
{
    public class InicioAlumnoViewModel
    {
        public List<PreguntasUltimaClase> UltimasPreguntasCorregidas { get; set; }
        public List<Alumno> TablaDePosiciones { get; set; }
    }
}