using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CEPA.CCO.Entidades
{
    public class DocBuque : EncaBuque 
    {
        public int? CantArchivo { get; set; }
        public int? IdReg { get; set; }
        public string c_voyage { get; set; }

        public int? TotalImp { get; set; }
        public int? TotalTrans { get; set; }
        public int? TotalTransA { get; set; }
        public int? TotalPTransA { get; set; }
        public int? TotalPTrans { get; set; }
        public string a_manifiesto { get; set; }
        public int IdDoc { get; set; }
        public int? TotalCancel { get; set; }

        public int? CantExport { get; set; }
        public List<int> IdDeta { get; set; }
        public string c_prefijo { get; set; }

        public int? CantRemo { get; set; }

        public int b_sidunea { get; set; }
    }

    public class DetaDAN
    {
        public string d_buque { get; set; }
        public string d_cliente { get; set; }
        public string c_viaje { get; set; }
        public string n_oficio { get; set; }
        public string n_contenedor { get; set; }
        public string c_pais_origen { get; set; }
        public string c_consignatario { get; set; }
        public int c_correlativo { get; set; }
        public string c_llegada { get; set; }
        public string f_dan { get; set; }
        public string c_naviera { get; set; }
        public string c_imo { get; set; }

        public string jefe_almacen { get; set;}
        public string sub_inspector { get; set;}
        public string Clave { get; set; }
        public string b_escaner { get; set; }
        public string c_prefijo { get; set; }
        public string ClaveP { get; set; }
        public int Total { get; set; }
        public int Cantidad { get; set; }

    }

    public class RptIngreImport
    {
        public int IdDeta { get; set; }
        public string c_llegada { get; set; }
        public string n_contenedor { get; set; }
        public string p_procedencia { get; set; }
        public string navicorto { get; set; }

        public string s_consignatario { get; set; }

        public string s_mercaderia { get; set; }

        public string f_retencion { get; set; }
        public string f_liberacion { get; set; }

        public string b_cancelado { get; set; }
        public string d_buque { get; set; }
        public string  c_naviera { get; set; }

    }


}
