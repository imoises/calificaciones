using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using calificaciones.Entidades;

namespace calificaciones.Services
{
    public class RespuestaService
    {
        Contexto bdContexto = new Contexto();

        public List<RespuestaAlumno> ObtenerRespuestasPorId(int Id)
        {
            return bdContexto.RespuestaAlumnoes.Include("Alumno")
                .Include("ResultadoEvaluacion").Where(r => r.Pregunta.Nro == Id).ToList();
        }

        public void AgregarRespuesta(int idPregunta, String respuesta, int idAlumno)
        {
            RespuestaAlumno respuestaAlumno = new RespuestaAlumno();

            respuestaAlumno.IdPregunta = idPregunta;
            respuestaAlumno.IdAlumno = idAlumno;
            respuestaAlumno.FechaHoraRespuesta = DateTime.Now;
            respuestaAlumno.Respuesta = respuesta;
            respuestaAlumno.Orden = bdContexto.RespuestaAlumnoes.Where(r => r.IdPregunta == idPregunta).ToList().Count + 1;

            bdContexto.RespuestaAlumnoes.Add(respuestaAlumno);

            bdContexto.SaveChanges();
        }

        public RespuestaAlumno ObtenerRespuestaPorIdRespuesta(int idRespuestaAlumno)
        {
            return bdContexto.RespuestaAlumnoes.Where(r => r.IdRespuestaAlumno == idRespuestaAlumno).FirstOrDefault();
        }

        public void EnviarEmailRespuestaDelAlumno(Pregunta pregunta, string resp, int idAlum)
        {
            Alumno alumno = bdContexto.Alumnoes.Find(idAlum);
            Pregunta preg = bdContexto.Preguntas.Find(pregunta.IdPregunta);
            RespuestaAlumno respuestaAlumno = bdContexto.RespuestaAlumnoes.SingleOrDefault(p => p.IdPregunta == preg.IdPregunta
                && p.IdAlumno == alumno.IdAlumno);
            Profesor profesor = bdContexto.Profesors.Find(preg.IdProfesorCreacion);
            string email = profesor.Email;
            MailMessage msj = new MailMessage();
            msj.To.Add("pnsanchez@unlam.edu.ar");
            msj.Bcc.Add("matiaspaz@test.com");
            msj.Bcc.Add("marianojuiz@test.com");
            string asunto = "Asunto:Respuesta a Pregunta " + preg.Nro + " Orden: " + respuestaAlumno.Orden + " Apellido " + alumno.Apellido;
            msj.Subject = asunto;
            msj.SubjectEncoding = System.Text.Encoding.UTF8;
            string evaluarPregunta = ("http://localhost:57226/Profesor/EvaluarRespuestas?" + "Nro=" + preg.IdPregunta + "&" + "Clase=Clase%201" + "&" + "Tipo=Todas");
            msj.Body = ("Pregunta: " + preg.Pregunta1 + " Alumno: " + alumno.Nombre + " " + alumno.Apellido + " Orden: " + respuestaAlumno.Orden + "" + " Respuesta: " + resp + " Evaluar: " + evaluarPregunta);
            msj.BodyEncoding = System.Text.Encoding.UTF8;
            msj.IsBodyHtml = true;
            msj.From = new MailAddress("ferezecarr@gmail.com");
            SmtpClient cli = new SmtpClient();
            cli.Credentials = new NetworkCredential("calificapp1@gmail.com","calificapp12345678");
            cli.Port = 587;
            cli.EnableSsl = true;
            cli.Host = "smtp.gmail.com";
            cli.Send(msj);
        }

        public Pregunta ObtenerRespuestasTodas()
        {
            var pregunta = new Pregunta();
            var query = from r in bdContexto.RespuestaAlumnoes  select r;
            pregunta.RespuestaAlumnoes = query.ToList();
            return pregunta;
        }

        public RespuestaAlumno ObtenerUnaRespuestaId(int respuesta)
        {
            //var query = from p in bdContexto.Preguntas.Include("RespuestaAlumnoes").Include("Profesor") where p.Nro == Nro && p.IdClase == Clase select p;
            var query = from r in bdContexto.RespuestaAlumnoes where r.IdRespuestaAlumno == respuesta select r;
            var respuestaBuscada = query.FirstOrDefault();
            return respuestaBuscada;
        }

        public List<RespuestaAlumno> ObtenerRespuestasCorrectas(int idPregunta)
        {
            var query = from r in bdContexto.RespuestaAlumnoes where r.IdResultadoEvaluacion == 1 && r.IdPregunta == idPregunta select r;
            var respuestasCorrecta = query.ToList();
            return respuestasCorrecta;
        }

        public int ObtenerSinEvaluar(int idPregunta)
        {
            var query = from r in bdContexto.RespuestaAlumnoes where r.IdPregunta == idPregunta && r.IdResultadoEvaluacion == null select r;
            var respuestasSinEvaluar = query.Count();
            return respuestasSinEvaluar;
        }

        public bool ObtenerSiMejor(int idPregunta)
        {
            var query = from r in bdContexto.RespuestaAlumnoes where r.IdPregunta == idPregunta && r.MejorRespuesta == true select r;
            var mejorRespuesta = query.Any();
            return mejorRespuesta;
        }

        //public List<RespuestaAlumno> ObtenerRespuestasAlumnoTipo(int idPregunta, String tipo) // tipo: Todas, SinCorregir, Correcta,Regular, Mal
        //{
        //    switch (tipo)
        //    {
        //        case "SinCorregir":
        //            return ObtenerRespuestasAlumnoSinCorregir(idPregunta);
        //        case "Correctas":
        //            return ObtenerRespustasAlumnoCorrecta(idPregunta);
        //        case "Regular":
        //            return ObtenerRespustasAlumnoRegular(idPregunta);
        //        case "Mal":
        //            return ObtenerRespustasAlumnoMal(idPregunta);
        //        default:
        //            return ObtenerTodasLasRespuestaPublicadas();
        //    }

        //}

        //public List<RespuestaAlumno> ObtenerRespustasAlumnoMal(int idPregunta)
        //{
        //    List<RespuestaAlumno> respuestasAlumno = bdContexto.RespuestaAlumnoes.Where(r => r.IdPregunta == idPregunta && r.IdResultadoEvaluacion == 3).ToList();

        //    return respuestasAlumno;
        //}

        public Pregunta ObtenerPreguntasConRespuestas(int nro, string clase)
        {
            var query = from p in bdContexto.Preguntas where p.Nro == nro && p.Clase.Nombre == clase select p;
            var preguntaRespuesta = query.FirstOrDefault();
            return preguntaRespuesta;
        }

        public List<RespuestaAlumno> ObtenerRespuestasAlumnoTipo(int idPregunta, String tipo) // tipo: Todas, SinCorregir, Correcta,Regular, Mal
        {
            switch (tipo)
            {
                case "SinCorregir":
                    return bdContexto.RespuestaAlumnoes.Where(r => r.IdPregunta == idPregunta && r.IdResultadoEvaluacion == null).ToList();
                case "Correcta":
                    return bdContexto.RespuestaAlumnoes.Where(r => r.IdPregunta == idPregunta && r.IdResultadoEvaluacion == 1).ToList();
                case "Regular":
                    return bdContexto.RespuestaAlumnoes.Where(r => r.IdPregunta == idPregunta && r.IdResultadoEvaluacion == 2).ToList();
                case "Mal":
                    return bdContexto.RespuestaAlumnoes.Where(r => r.IdPregunta == idPregunta && r.IdResultadoEvaluacion == 3).ToList();
                default:
                    return bdContexto.RespuestaAlumnoes.Where(r => r.IdPregunta == idPregunta).ToList();
            }
        }

        public Pregunta FiltroRespuesta(Pregunta respuestaAlumnos, string tipo)
        {
            var respuesta = new RespuestaAlumno();
            if (tipo != null && tipo != "Todas")
            {
                var varRespuestaAlumnos = respuestaAlumnos.RespuestaAlumnoes.Where(r => r.ResultadoEvaluacion != null).ToList();
                if (varRespuestaAlumnos.Count != 0)
                {
                    respuestaAlumnos.RespuestaAlumnoes = respuestaAlumnos.RespuestaAlumnoes.Where(r => r.ResultadoEvaluacion.Resultado == tipo).ToList();
                }
                else
                {
                    if (tipo == "SinCorregir")
                    {
                        respuestaAlumnos.RespuestaAlumnoes = respuestaAlumnos.RespuestaAlumnoes.Where(r => r.IdResultadoEvaluacion == null).ToList();
                    }
                    else
                    {
                        respuestaAlumnos.RespuestaAlumnoes = varRespuestaAlumnos;
                    }
                }
            }
            return respuestaAlumnos;
        }

        //public Pregunta FiltroRespuesta(Pregunta respuestaAlumnos, string tipo)
        //{
        //    var respuesta = new RespuestaAlumno();

        //    switch (tipo)
        //    {
        //        case "SinCorregir":
        //            respuestaAlumnos.RespuestaAlumnoes = respuestaAlumnos.RespuestaAlumnoes.Where(r => r.ResultadoEvaluacion.Resultado == null).ToList();
        //            break;
        //        case "Correcta":
        //            respuestaAlumnos.RespuestaAlumnoes = respuestaAlumnos.RespuestaAlumnoes.Where(r => r.ResultadoEvaluacion.IdResultadoEvaluacion == 1).ToList();
        //            break;
        //        case "Regular":
        //            respuestaAlumnos.RespuestaAlumnoes = respuestaAlumnos.RespuestaAlumnoes.Where(r => r.ResultadoEvaluacion.IdResultadoEvaluacion == 2).ToList();
        //            break;
        //        case "Mal":
        //            respuestaAlumnos.RespuestaAlumnoes = respuestaAlumnos.RespuestaAlumnoes.Where(r => r.ResultadoEvaluacion.IdResultadoEvaluacion == 3).ToList();
        //            break;
        //    }

        //    return respuestaAlumnos;
        //}

        public List<RespuestaAlumno> ObtenerRespuestasAlumnoTipo(String tipo) // tipo: Todas, SinCorregir, Correcta,Regular, Mal
        {
            switch (tipo)
            {
                case "SinCorregir":
                    return bdContexto.RespuestaAlumnoes.Where(r => r.IdResultadoEvaluacion == null).ToList();
                case "Correcta":
                    return bdContexto.RespuestaAlumnoes.Where(r => r.IdResultadoEvaluacion == 1).ToList();
                case "Regular":
                    return bdContexto.RespuestaAlumnoes.Where(r => r.IdResultadoEvaluacion == 2).ToList();
                case "Mal":
                    return bdContexto.RespuestaAlumnoes.Where(r => r.IdResultadoEvaluacion == 3).ToList();
                default:
                    return bdContexto.RespuestaAlumnoes.ToList();
            }

        }

        public void Guardar()
        {
            bdContexto.SaveChanges();
        }
    }
}