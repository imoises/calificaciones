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
            RespuestaAlumno respuestaAlumno = bdContexto.RespuestaAlumnoes.SingleOrDefault(r => r.IdPregunta == preg.IdPregunta);
            Profesor profesor = bdContexto.Profesors.Find(preg.IdProfesorCreacion);
            string email = profesor.Email;
            MailMessage msj = new MailMessage();
            msj.To.Add(email);
            string asunto = "Asunto:Respuesta a Pregunta " + preg.Nro + " Orden: " + respuestaAlumno.Orden;
            msj.Subject = asunto;
            msj.SubjectEncoding = System.Text.Encoding.UTF8;
            string evaluarRespuesta = "http://localhost:53443/Profesor/EvaluarRespuestas" + preg.IdPregunta;
            msj.Body = "Pregunta: " + preg.Pregunta1 + " Alumno: " + alumno.Nombre + " ";
            msj.BodyEncoding = System.Text.Encoding.UTF8;
            msj.IsBodyHtml = true;
            msj.From = new MailAddress("ferezecarr@gmail.com");
            SmtpClient cli = new SmtpClient();
            cli.Credentials = new NetworkCredential("test@test.com","1234");
            cli.Port = 587;
            cli.EnableSsl = true;
            cli.Host = "smtp.gmail.com";
            cli.Send(msj);
        }

        public List<RespuestaAlumno> ObtenerRespuestasTodas()
        {
            var query = from r in bdContexto.RespuestaAlumnoes  select r;
            var respuestas = query.ToList();
            return respuestas;
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

        public int ObtenerRespuestasSinEvaluar(int idPregunta)
        {
            var query = from r in bdContexto.RespuestaAlumnoes where r.IdPregunta == idPregunta && r.IdResultadoEvaluacion == null select r;
            var respuestasSinEvaluar = query.Count();
            return respuestasSinEvaluar;
        }

        public bool ObtenerSiMejorRespuesta(int idPregunta)
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

        public List<RespuestaAlumno> ObtenerRespuestasAlumnoTipo(int idPregunta, String tipo) // tipo: Todas, SinCorregir, Correcta,Regular, Mal
        {
            switch (tipo)
            {
                case "SinCorregir":
                    return bdContexto.RespuestaAlumnoes.Where(r => r.IdPregunta == idPregunta && r.IdResultadoEvaluacion == null).ToList();
                case "Correctas":
                    return bdContexto.RespuestaAlumnoes.Where(r => r.IdPregunta == idPregunta && r.IdResultadoEvaluacion == 1).ToList();
                case "Regular":
                    return bdContexto.RespuestaAlumnoes.Where(r => r.IdPregunta == idPregunta && r.IdResultadoEvaluacion == 2).ToList();
                case "Mal":
                    return bdContexto.RespuestaAlumnoes.Where(r => r.IdPregunta == idPregunta && r.IdResultadoEvaluacion == 3).ToList();
                default:
                    return bdContexto.RespuestaAlumnoes.Where(r => r.IdPregunta == idPregunta).ToList();
            }

        }
    }
}