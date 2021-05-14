using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CEPA.CCO.Entidades
{
    public class ArchivoAduana
    {
        public int c_correlativo { get; set; }
        public int num_manif { get; set; }

        public string n_BL { get; set; }
        public string n_contenedor { get; set; }
        public string n_pais_origen { get; set; }
        public string n_pais_destino { get; set; }
        public string b_estado_c { get; set; }
        public string c_tamaño_c { get; set; }
        public int v_tara { get; set; }
        public double v_peso { get; set; }
        public string b_condicion { get; set; }
        public string s_consignatario { get; set; }
        public string c_tamaño { get; set; }
        public string b_reef { get; set; }
        public string b_ret_dir { get; set; }
        public string b_tranship { get; set; }
        public string b_estado { get; set; }
        public string c_imo_imd { get; set; }
        public string c_un_number { get; set; }
        public string n_sello { get; set; }
        public string s_comodity { get; set; }
        public string s_promanifiesto { get; set; }
        
        public string c_imo { get; set; }
        public string c_voyage { get; set; }

        public string b_transferencia { get; set; }
        public string b_manejo { get; set; }
        public string b_despacho { get; set; }

        public string c_condicion { get; set; }

        public string c_pais_origen { get; set; }
        public string c_pais_destino { get; set; }
        public string b_req_tarja { get; set; }

        public string c_iso_navi { get; set; }
        public string b_tratamiento { get; set; }

        public string a_manifiesto { get; set; }

        public string tipoDecla { get; set; }
        public int c_paquete { get; set; }
        public string c_embalaje { get; set; }
        public string d_cliente { get; set; }

        public string c_tipo_bl { get; set; }
        public string b_shipper { get; set; }
    }
}
