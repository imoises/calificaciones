using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using calificaciones.Entidades;

namespace calificaciones.Services
{
    public class ProfesorService
    {
        Contexto bdContexto = new Contexto();

        public Profesor ObtenerUnProfesor(int id)
        {
            return bdContexto.Profesors.FirstOrDefault(profesor => profesor.IdProfesor == id);
        }

        public List<Profesor> ObtenerTodosLosProfesores()
        {
            return bdContexto.Profesors.ToList();
        }

        public bool verificarProfesorRegistrado(Profesor profesor)
        {
            var profesorRegistrado = bdContexto.Profesors.Where(prof => prof.Email == profesor.Email).FirstOrDefault();
            if (profesorRegistrado != null)
            {
                return true;
            }
            return false;
        }

        public bool verificarEmailExistenteDeUnProfesor(Profesor profesor)
        {
            Profesor profesorEmail = bdContexto.Profesors.Where(prof => prof.Email == profesor.Email).FirstOrDefault();
            if (profesorEmail != null)
            {
                return true;
            }
            return false;
        }
    }
}