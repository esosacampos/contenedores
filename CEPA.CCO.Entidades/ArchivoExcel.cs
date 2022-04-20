using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CEPA.CCO.Entidades
{
    public class ArchivoExcel
    {
        public int num_manif { get; set; }
        public string c_num_manif { get; set; }
        public string anum_manif { get; set; }
        public int num_fila { get; set; }
        
        public double c_imo {get; set;}
        public string c_imoc { get; set; }
        public string c_voyage { get; set; }
        public string n_BL { get; set; }
        public string celda { get; set; }
        public string n_contenedor {get; set;}
        public string c_tamaño {get; set;} 
        public double v_peso {get; set;}
        public string b_estado {get; set;}
        public string s_consignatario {get; set;}
        public string n_sello {get; set;}
        public string celdaSello { get; set; }
        public string c_pais_destino {get; set;} 
        public string c_pais_origen {get; set;}
        public string c_detalle_pais { get; set; }
        public string s_comodity {get; set;}
        public string s_prodmanifestado {get; set;}
        public double v_tara {get; set;}
        public string b_reef {get; set;}
        public string b_ret_dir {get; set;}
        public string c_imo_imd {get; set;}
        public string celdaPel { get; set; }
        public string c_un_number {get; set;}

        public string celdaClase { get; set; }
        public string b_transhipment {get; set;}
        public string c_condicion { get; set; }

        public string b_shipper { get; set; }

        public string b_transferencia { get; set; }
        public string celdaTrans { get; set; }

        public string b_manejo { get; set; }

        public string celdaTara { get; set; }

        public string celdaMan { get; set; }
        public string b_despacho { get; set; }

        public string celdaDes { get; set; }

        
    }

    public class ArchivoExport
    {
       
        public int num_fila { get; set; }

        public int IdDeta { get; set; }
        public double c_imo { get; set; }
        public string c_imoc { get; set; }
        public string c_voyage { get; set; }        
        public string n_contenedor { get; set; }
        public string c_tamaño { get; set; }
        public double v_peso { get; set; }
        public string b_estado { get; set; }
        public string s_consignatario { get; set; }
        public string n_sello { get; set; }
        public string c_pais_destino { get; set; }
        public string c_pais_origen { get; set; }        
        public string s_comodity { get; set; }
        public string s_prodmanifestado { get; set; }
        public double v_tara { get; set; }
        public string b_reef { get; set; }
        public string b_emb_dir { get; set; }
        public string c_imo_imd { get; set; }
        public string c_un_number { get; set; }
        public string b_transhipment { get; set; }
        public string c_condicion { get; set; }

        public string b_shipper { get; set; }

        public string s_transferencia { get; set; }
        public string s_manejo { get; set; }
        public string s_recepcion { get; set; }
        public string s_almacenaje { get; set; }

        public int n_booking { get; set; }
        public string s_exportador { get; set; }       
        public string c_tipo_doc { get; set; }
        public string n_documento { get; set; }
        public int IdReg { get; set; }
        public int IdDoc { get; set; }

        public int c_correlativo { get; set; }       
        public string b_estado_c {get;set;}
        public string c_tamaño_c { get; set; }
        public string d_pais_destino { get; set; }
        public string d_pais_origen { get; set; }

        public string nit_exportador { get; set; }
        public string em_exportador { get; set; }
        public string tel_exportador { get; set; }

        public string c_pais_trasbordo { get; set; }
        public string c_puerto_trasbordo { get; set; }
        public string t_estatus { get; set; }

        public string s_trafico { get; set; }

        public string s_posicion { get; set; }

        public string s_pe { get; set; }

        public DateTime f_venc_arivu { get; set; }
        public string s_nom_predio { get; set; }
        public int c_corr_previo { get; set; }

        public double v_peso_st { get; set; }
        public string s_fec_venc { get; set; }
        public string c_llegada { get; set; }

        public string c_prefijo { get; set; }
        public string c_naviera { get; set; }

    }

    public class ArchivoAduanaValid
    {
        public int IdValid { get; set; }
        public int n_manifiesto { get; set; }
        public string n_contenedor { get; set; }
        public DateTime f_registro { get; set; }

        public string n_manifi { get; set; }
        public string n_BL { get; set; }
        public string a_mani { get; set; }

        public int IdReg { get; set; }
        public int IdDeta { get; set; }

        public string c_tipo_bl { get; set; }
        public int b_sidunea { get; set; }

        public string c_tamaño { get; set; }
        public string s_agencia { get; set; }
        public double v_peso { get; set; }
        public int c_paquete { get; set; }
        public string c_embalaje { get; set; }
        public string d_embalaje { get; set; }
        public string c_pais_origen { get; set; }
        public string d_puerto_origen { get; set; }
        public string c_pais_destino { get; set; }
        public string d_puerto_destino { get; set; }
        public string s_nit { get; set; }
        public string s_consignatario { get; set; }

        public string n_BL_master { get; set; }
    }


    public class IncoAduana
    {
        public int IdReg { get; set; }
        public int n_manifiesto { get; set; }
        public string n_contenedor { get; set; }
       
        public string n_manifi { get; set; }
        public string n_BL { get; set; }
        public string a_mani { get; set; }
        public string c_naviera { get; set; }
    }

    public class ContenedoresAduana
    {
        public string n_contenedor { get; set; }
        public int c_correlativo { get; set; }

        public int n_manifiesto { get; set; }
        public string a_manifiesto { get; set; }
        public int b_sidunea { get; set; }

        public string c_navi_corto { get; set; }

        public double v_aduana { get; set; }
        public double v_cepa { get; set; }

        public string b_ship_aduana { get; set; }
        public string b_ship_cepa { get; set; }
    }
}
