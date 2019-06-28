using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using calificaciones.Entidades;

namespace calificaciones.Services
{
    public class AlumnoService
    {
        Contexto bdContexto = new Contexto();

        public Alumno ObtenerUnAlumno(int id)
        {
            return bdContexto.Alumnoes.FirstOrDefault(alumno => alumno.IdAlumno == id);
        }

        public List<Alumno> ObtenerTodosLosAlumnos()
        {
            return bdContexto.Alumnoes.ToList();
        }

        public bool verificarAlumnoRegistrado(Alumno alumno)
        {
            var alumnoRegistrado = bdContexto.Alumnoes.Where(alu => alu.Email == alumno.Email).FirstOrDefault();
            if(alumnoRegistrado != null)
            {
                return true;
            }
            return false;
        }

        public bool verificarEmailExistenteDeUnAlumno(Alumno alumno)
        {
            Alumno alumnoEmail = bdContexto.Alumnoes.Where(alu => alu.Email == alumno.Email).FirstOrDefault();
            if(alumnoEmail != null)
            {
                return true;
            }
            return false;
        }
    }
}