using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using calificaciones.Entidades;

namespace calificaciones.Services
{
    public class PreguntaService
    {
        Contexto bdContexto = new Contexto();

        public List<Pregunta> ObtenerTodasLasPreguntas()
        {
            return bdContexto.Preguntas.Include("Clase").Include("Tema").ToList();
        }
    }
}