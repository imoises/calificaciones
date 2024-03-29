﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace calificaciones.Session
{
    public static class SessionManagement
    {
        public static int IdUsuario
        {
            get
            {
                if(HttpContext.Current.Session["Id"] == null)
                {
                    return 0;
                }
                return (int)HttpContext.Current.Session["Id"];
            }
            set
            {
                HttpContext.Current.Session["Id"] = value;
            }
        }

        public static string Nombre
        {
            get
            {
                if(HttpContext.Current.Session["Nombre"] == null)
                {
                    return "";
                }
                return (string)HttpContext.Current.Session["Nombre"];
            }
            set
            {
                HttpContext.Current.Session["Nombre"] = value;
            }
        }
        
        public static string Rol { get; set; }
    }
}