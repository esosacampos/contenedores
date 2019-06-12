using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CEPA.CCO.Entidades
{
    public class TipoRevision
    {
        public int IdRevision { get; set; }
        public string Clave { get; set; }
        public string Tipo { get; set; }
        public string Habilitado { get; set; }
        public int Hab { get; set; }
    }

    public class AgenteDAN
    {
        public int IdAgente {get;set;}
        public string n_agente {get;set;}
        public string d_agente {get;set;}
        public string Habilitado { get; set; }
    }
}
