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

        // ObtenerUsuarioSesion no esta implementado
        public UsuarioSession ObtenerUsuarioSesion(Usuario usuario)
        {
            UsuarioSession usuarioSession = new UsuarioSession();

            if (usuario.SoyProfesor)
            {
                Profesor profesor = IniciarProfesor(usuario);

                usuarioSession.Id = profesor.IdProfesor;
                usuarioSession.Nombre = profesor.Nombre;
                usuarioSession.Apellido = profesor.Apellido;
                usuarioSession.Rol = "Profesor";

                return usuarioSession;
            }
            else
            {
                Alumno alumno = IniciarAlumno(usuario);

                usuarioSession.Id = alumno.IdAlumno;
                usuarioSession.Nombre = alumno.Nombre;
                usuarioSession.Apellido = alumno.Apellido;
                usuarioSession.Rol = "Alumno";

                return usuarioSession;
            }
        }
    }
}