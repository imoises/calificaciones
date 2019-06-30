using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace calificaciones.Models
{
    public class PosAlumnoPuntos
    {
        public int Posicion { get; set; }
        public String Nombre { get; set; }
        public String Apellido { get; set; }
        public long Puntos { get; set; }
    }
}