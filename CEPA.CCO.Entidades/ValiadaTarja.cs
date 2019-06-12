using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CEPA.CCO.Entidades
{
    public class ValiadaTarja
    {
        public string c_tarja {get; set;}
        public string b_observa { get; set; }
        public string c_usuario { get; set; }
        public int n_manifiesto { get; set; }
        public int a_manifiesto { get; set; }
        public string n_contenedor { get; set; }
    }

    public class Manifiesto
    {
        public string c_tarja { get; set; }
        public string n_contenedor { get; set; }
    }

    public class InfOperaciones
    {
        public int v_tara { get; set; }
        public int IdDeta { get; set; }
        public string c_tamaño { get; set; }
        public string c_trafico { get; set; }
        public string c_llegada { get; set; }
        public string c_estado { get; set; }
        public string c_naviera { get; set; }
        public string c_buque {get; set;}
        public string c_cliente { get; set; }
        public string f_recepcion { get; set; }
        public string n_contenedor { get; set; }

        public string b_detenido { get; set; }
        public string b_style { get; set; }


        public int Total { get; set; }
        public int OIRSA { get; set; }
        public int PO { get; set; }
        public int PATIO { get; set; }
        public int PP { get; set; }
        public string s_comentarios { get; set; }
        public DateTime f_realrpatio { get; set; }

        
        
        public int c_marcacion { get; set; }

        public string b_aduana { get; set; }
        public string b_staduana { get; set; }

    }
}
