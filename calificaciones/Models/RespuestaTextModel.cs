using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace calificaciones.Models
{
    public class RespuestaTextModel
    {
        [Required(ErrorMessage = "Tiene que ingresar un texto")]
        [AllowHtml]
        public string Respuesta { get; set; }
    }
}