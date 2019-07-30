using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CEPA.CCO.Entidades
{
    public class TransferXML
    {
        public string year { get; set; }
        public string aduana { get; set; }
        public string nmanifiesto { get; set; }
        public string nbl { get; set; }
        public string contenedor { get; set; }
        public string f_rpatio { get; set; }
        public string sitio { get; set; }
        public string comentarios { get; set; }
        public int IdDeta { get; set; }
        public string c_llegada { get; set; }

        public int IdValue { get; set; }

        public int b_sidunea { get; set; }
        public string c_naviera { get; set; }

        public string f_retencion { get; set; }
        public string f_liberacion { get; set; }
    }

    public class CorteCOTECNA
    {      
        public string c_llegada { get; set; }
        public string c_cliente { get; set; }
        public string d_buque { get; set; }
        public string d_cliente { get; set; }
        public string n_manifiesto { get; set; }
        public int t_contenedores { get; set; }
        public int t_dan { get; set; }
        public int t_dga { get; set; }
        public DateTime f_atraque { get; set; }
        public string c_prefijo { get; set; }
        public string b_solidga { get; set; }
        public string c_voyage { get; set; }
        public int IdDoc { get; set; }

        public string c_nul { get; set; }
    }

    public class EncaLiquid
    {
        public string c_llegada { get; set; }
        public string n_buque { get; set; }
        public string c_imo { get; set; }
        public string c_null { get; set; }
        public DateTime f_atraque { get; set; }
        public DateTime f_desatraque { get; set; }
        public string c_cliente { get; set; }
        public string d_cliente { get; set; }

        public string s_atraque { get; set; }
        public string s_desatraque { get; set; }
    }

    public class LiquidADUANA
    {
        public string c_llegada { get; set; }
        public string c_cliente { get; set; }
        public string d_buque { get; set; }
        public string d_cliente { get; set; }
        public string n_manifiesto { get; set; }
        public int t_manifestados { get; set; }
        public int t_recibidos { get; set; }
        public int t_cancelados { get; set; }


        
        public DateTime f_atraque { get; set; }
        public string c_prefijo { get; set; }
        public string b_solidga { get; set; }
        public string c_voyage { get; set; }
        public int IdDoc { get; set; }
    }

    public class DetaillLiquid
    {
        public int c_correlativo { get; set; }
        public string c_llegada { get; set; }
        public string n_manifiesto { get; set; }
        public string a_manifiesto { get; set; }
        public string d_cliente { get; set; }
        public string n_contenedor { get; set; }
        public string c_naviera { get; set; }
    }

    public class AlertaDANTarja
    {
        public string c_tarja { get; set; }
        public string c_parametro { get; set; }
  
    }

    public class TarjaNotificada
    {
        public string c_tarja { get; set; }
        public string c_parametro { get; set; }
        public int y_parametro { get; set; }

    }

    public class ParametroAlerta
    {
        public string c_parametro { get; set; }

    }
}
