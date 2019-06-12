using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CEPA.CCO.Entidades
{
    public class EstadosDecla
    {
        public int IdRegAduana {get;set;}
        public string c_aduana {get;set;} 
        public int n_manifiesto {get;set;} 
        public int a_manifiesto {get;set;}
        public string n_contenedor {get;set;}
        public int a_decla {get;set;} 
        public int s_decla {get;set;} 
        public int c_decla {get;set;} 
        public int IdEstado {get;set;} 
        public DateTime f_reg_aduana {get;set;} 
        public int IdSelectividad {get;set;}
        public string n_nit { get; set; }
        public int b_siduneawd { get; set; }

        public string s_consignatario { get; set; }
        public string s_descripcion { get; set; }
        public string n_BL { get; set; }

        public int a_transito { get; set; }
        public string r_transito { get; set; }
    }
}
