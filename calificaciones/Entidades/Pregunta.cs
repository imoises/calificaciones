
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace calificaciones.Entidades
{

using System;
    using System.Collections.Generic;
    
public partial class Pregunta
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Pregunta()
    {

        this.RespuestaAlumnoes = new HashSet<RespuestaAlumno>();

    }


    public int IdPregunta { get; set; }

    public int Nro { get; set; }

    public int IdClase { get; set; }

    public int IdTema { get; set; }

    public Nullable<System.DateTime> FechaDisponibleDesde { get; set; }

    public Nullable<System.DateTime> FechaDisponibleHasta { get; set; }

    public string Pregunta1 { get; set; }

    public int IdProfesorCreacion { get; set; }

    public System.DateTime FechaHoraCreacion { get; set; }

    public Nullable<int> IdProfesorModificacion { get; set; }

    public Nullable<System.DateTime> FechaHoraModificacion { get; set; }



    public virtual Clase Clase { get; set; }

    public virtual Profesor Profesor { get; set; }

    public virtual Profesor Profesor1 { get; set; }

    public virtual Tema Tema { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<RespuestaAlumno> RespuestaAlumnoes { get; set; }

}

}
