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

        // CAMBIOS
        public double v_peso_tm { get; set; }
        public DateTime f_recepcion { get; set; }
        public DateTime f_retiro { get; set; }

        public string fs_recepcion { get; set; }
        public string fs_retiro { get; set; }

        public string ValAlma { get; set; }

        // FACTURAS
        public double t_facturaM { get; set; }
        public DateTime f_calc_retiroM { get; set; }

        //DATOS CLIENTES
        public string c_cliente { get; set; }
        public string s_cliente { get; set; }
        public string f_pago { get; set; }
    }

    public class BL2
    {
        public string c_nul { get; set; }
        public string c_prefijo { get; set; }
        public string c_llegada { get; set; }
        public string n_manifiesto { get; set; }
        public string n_bl { get; set; }
        public string n_contenedor { get; set; }
        public string c_tamaño { get; set; }
        public decimal v_peso { get; set; }
        public decimal v_teus { get; set; }
        public string c_trafico { get; set; }
        public DateTime f_salida { get; set; }
        public string n_manejo { get; set; }
        public string c_manejo { get; set; }
        // manejo
        public double vn_manejo { get; set; }
        public double vc_manejo { get; set; }
        public decimal v_manejo { get; set; }
        public double p_manejo { get; set; }
        public string n_transferencia { get; set; }
        public string c_transferencia { get; set; }
        // transferencia
        public double vn_transfer { get; set; }
        public double vc_transfer { get; set; }
        public decimal v_transferencia { get; set; }
        public double p_transfer { get; set; }

        //despacho
        public string n_despacho { get; set; }
        public string c_despacho { get; set; }

        public double vn_desp { get; set; }
        public double vc_desp { get; set; }
        public decimal v_despacho { get; set; }
        public double p_desp { get; set; }
        public string n_almacenaje { get; set; }
        public string c_almacenaje { get; set; }
        public decimal v_almacenaje { get; set; }
        public double vn_almacenaje { get; set; }
        public double vc_almacenaje { get; set; }
        public string c_tarja { get; set; }
        public string f_tarja { get; set; }

        public string f_salidas { get; set; }
        public string b_cancelado { get; set; }

        public double ta_alm { get; set; }
        public string t_retencion { get; set; }
        public string reff { get; set; }

        public decimal peso_entregado { get; set; }

        public DateTime f_retencion { get; set; }

        public string f_retenciones { get; set; }

        // cambios
        public decimal v_peso_tm { get; set; }
        public DateTime f_recepcion { get; set; }
        public DateTime f_retiro { get; set; }

        public string fs_recepcion { get; set; }
        public string fs_retiro { get; set; }

        public string valalma { get; set; }
        public double p_almacenaje { get; set; }

        // facturas
        public double t_facturam { get; set; }
        public DateTime f_calc_retirom { get; set; }

        //datos clientes
        public string c_cliente { get; set; }
        public string s_cliente { get; set; }
        public string f_pago { get; set; }

        //nuevo por tarifas
        public string b_estado { get; set; }
        public int dias_total { get; set; }
        public int dias_t_ant { get; set; }
        public int dias_t_nva { get; set; }
        public DateTime? f_reg_dan { get; set; }
        public DateTime? f_retencion_ucc { get; set; }
        public DateTime? f_retencion_dga { get; set; }
        //public string ValAlma { get; set; }

    }
}
