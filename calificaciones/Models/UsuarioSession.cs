using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace calificaciones.Models
{
    public class UsuarioSession
    {
        public int Id { get; set; }
        public String Nombre { get; set; }
        public String Apellido { get; set; }
        public String Rol { get; set; }
    }
}