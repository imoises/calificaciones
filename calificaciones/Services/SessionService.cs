using calificaciones.Entidades;
using calificaciones.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace calificaciones.Services
{
    public class SessionService
    {
        Contexto bdContexto = new Contexto();
        public Profesor IniciarProfesor(Usuario usuario)
        {
            var query = from p in bdContexto.Profesors where p.Email == usuario.Email && p.Password == usuario.Password select p;
            var profesor = query.FirstOrDefault();
            return profesor;
        }

        public Alumno IniciarAlumno(Usuario usuario)
        {
            var query = from a in bdContexto.Alumnoes where a.Email == usuario.Email && a.Password == usuario.Password select a;
            var alumno = query.FirstOrDefault();
            return alumno;
        }
    }
}
