using System;

namespace CEPA.CCO.Entidades
{
    public class EncaBuque
    {
        public string c_llegada { get; set; }
        public string c_buque { get; set; }
        public string d_buque { get; set; }
        public string d_cliente { get; set; }
        public string c_cliente { get; set; }
        public DateTime f_llegada { get; set; }
        public string c_imo { get; set; }
        public int num_manif { get; set; }

        public string c_prefijo { get; set; }

        public DateTime f_desatraque { get; set; }
    }

    public class Documentos
    {
        public int c_valid { get; set; }

        public string n_contenedor { get; set; }
        public string v_tara { get; set; }
        public int b_20 { get; set; }
        public int b_40 { get; set; }
        public int b_45 { get; set; }
        public int b_48 { get; set; }
        public int b_fcl { get; set; }
        public int b_lcl { get; set; }
        public int b_vac { get; set; }
        public int b_trans { get; set; }
        public string c_tarifa { get; set; }
        public int real { get; set; }
        public int c_20 { get; set; }
        public int c_40 { get; set; }
        public int c_45 { get; set; }
        public int c_48 { get; set; }
        public string c_nul { get; set; }
        public string d_buque { get; set; }
        public string f_ingreso { get; set; }
        public string f_despacho { get; set; }
        public double v_valor { get; set; }
        public string d_cliente { get; set; }
        public string d_servicio { get; set; }
    }

    public class AnexoExport
    {
        public string c_factura { get; set; }
        public string c_nul { get; set; }
        public string d_buque { get; set; }
        public DateTime f_arribo { get; set; }

        public string c_naviera { get; set; }

        public string d_naviera { get; set; }
    }
}
