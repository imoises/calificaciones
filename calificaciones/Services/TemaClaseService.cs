using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using calificaciones.Entidades;

namespace calificaciones.Services
{
    public class TemaClaseService
    {
        Contexto bdContexto = new Contexto();

        public List<Tema> ObtenerTodosLosTemas()
        {
            return bdContexto.Temas.ToList();
        }

        public List<Clase> ObtenerTodasLasClase()
        {
            return bdContexto.Clases.ToList();
        }
    }
}