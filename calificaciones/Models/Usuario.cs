using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace calificaciones.Models
{
    public class Usuario
    {
        [Required(ErrorMessage = "*Debe ingresar un email")]
        [EmailAddress(ErrorMessage = "El email que esta ingresando no es válido")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "*Debe ingresar un password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool SoyProfesor { get; set; }
    }
}