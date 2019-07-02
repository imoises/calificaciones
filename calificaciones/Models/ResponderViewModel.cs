using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using calificaciones.Entidades;

namespace calificaciones.Models
{
    public class ResponderViewModel
    {
        public Pregunta Pregunta { get; set; }
        public RespuestaTextModel RespuestaTextModel { get; set; }
    }
}