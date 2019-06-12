using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CEPA.CCO.Entidades
{
    public class Notificaciones
    {
            
        public int IdNotificacion { get; set; }
        public string sMail { get; set; }
        public string dMail { get; set; } 
        public string Habilitado {get;set;}

    }

    public class ValoresNotifica
    {
        public int IdValores { get; set; }
        public string Descripcion { get; set; }
        public string Valor { get; set;}
        public string Habilitado { get; set; }
    }
}
