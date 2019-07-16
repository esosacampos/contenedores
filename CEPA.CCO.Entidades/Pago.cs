using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CEPA.CCO.Entidades
{
    public class Pago
    {
        public string b_tarifa { get; set; }
        public string validacion { get; set; }
        public string ValTransfer { get; set; }
        public string ValDespacho { get; set; }
        public string ValManejo { get; set; }
        public string ValAlmacenaje { get; set; }

        public string b_dan { get; set; }
        public string tipoReten { get; set; }


        public string style_va  { get; set; }
        public string style_transfer{ get; set; }
        public string style_despac { get; set; }
        public string style_manejo { get; set; }
        public string style_alma { get; set; }
        public string style_dan { get; set; }

        public string n_contenedor { get; set; }
        public string b_salida { get; set; }
        public string b_transito { get; set; }
        public string c_tamaño { get; set; }
        public string descripcion { get; set; }
        public string s_naviero { get; set; }
        public string s_cliente { get; set; }
        public double s_pagos { get; set; }

        public string f_tarja { get; set; }
        public string f_salida { get; set; }


        public string style_naviero_transfer { get; set; }
        public string style_cliente_transfer { get; set; }
     
        public string style_naviero_manejo { get; set; }
        public string style_cliente_manejo { get; set; }

        public string style_naviero_alma { get; set; }
        public string style_cliente_alma { get; set; }

        public string tarifa { get; set; }
        public string b_danc { get; set; }

        public string style_naviero_despacho {get; set;}
        public string style_cliente_despacho { get; set; }
        

        public double? p_tranfer { get; set; }
        public double? p_manejo { get; set; }
        public double? p_alma { get; set; }
        public double? p_despacho { get; set; }
        public string b_aduana { get; set; }
        public string style_aduana { get; set; }

        public double v_peso { get; set; }
        public int v_dias { get; set; }
        public double v_teus { get; set; }


        
    }                           
}
