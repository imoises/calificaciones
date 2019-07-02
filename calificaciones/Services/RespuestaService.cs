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
    }
}