using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CEPA.CCO.Entidades
{
    public class BL
    {
        public string c_nul { get; set; }
        public string c_prefijo { get; set; }
        public string c_llegada { get; set; }
        public string n_manifiesto { get; set; }
        public string n_BL { get; set; }
        public string n_contenedor { get; set; }
        public string c_tamaño { get; set; }
        public double v_peso { get; set; }
        public double v_teus { get; set; }
        public string c_trafico { get; set; }
        public DateTime f_salida { get; set; }
        public string n_manejo { get; set; }
        public string c_manejo { get; set; }
        // MANEJO
        public double vn_manejo { get; set; }
        public double vc_manejo { get; set; }
        public double v_manejo { get; set; }
        public double p_manejo { get; set; }
        public string n_transfer { get; set; }
        public string c_transfer { get; set; }
        // TRANSFERENCIA
        public double vn_transfer { get; set; }
        public double vc_transfer { get; set; }
        public double v_transfer { get; set; }
        public double p_transfer { get; set; }

        //DESPACHO
        public string n_desp { get; set; }
        public string c_desp { get; set; }

        public double vn_desp { get; set; }
        public double vc_desp { get; set; }
        public double v_desp { get; set; }
        public double p_desp { get; set; }
        public string n_alm { get; set; }
        public string c_alm { get; set; }
        public double v_alm { get; set; }

        public string c_tarja { get; set; }
        public string f_tarja { get; set; }

        public string f_salidas { get; set; }
        public string b_cancelado { get; set; }

        public double ta_alm { get; set; }
        public string t_retencion { get; set; }
        public string reff { get; set; }

        public double peso_entregado { get; set; }

        public DateTime f_retencion { get; set; }

        public string f_retenciones { get; set; }
    }
}
