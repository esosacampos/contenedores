using System;
using System.Collections.Generic;

namespace CEPA.CCO.Entidades
{
    public class DetaNaviera
    {
        public int IdDeta { get; set; }
        public int IdReg { get; set; }
        public string n_BL { get; set; }
        public string n_contenedor { get; set; }
        public string c_tamaño { get; set; }
        public string c_tamañoc { get; set; }
        public double v_peso { get; set; }
        public string b_estado { get; set; }
        public string s_consignatario { get; set; }
        public string n_sello { get; set; }
        public string c_pais_destino { get; set; }
        public string c_pais_origen { get; set; }
        public string c_detalle_pais { get; set; }
        public string s_comodity { get; set; }
        public string s_prodmanifestado { get; set; }
        public double v_tara { get; set; }
        public string b_reef { get; set; }
        public string b_ret_dir { get; set; }
        public string c_imo_imd { get; set; }
        public string c_un_number { get; set; }
        public string b_transhipment { get; set; }
        public string c_condicion { get; set; }
        public int c_correlativo { get; set; }
        public string s_observaciones { get; set; }
        public int IdDoc { get; set; }

        public string grupo { get; set; }
        public string grua { get; set; }
        public string b_estadoF { get; set; }
        public string b_estadoV { get; set; }
        public int c_marcacion { get; set; }
        public bool b_rastra { get; set; }
        public bool b_chasis { get; set; }
        public DateTime f_recepcion { get; set; }
        public bool b_recepcion { get; set; }
        public string b_recepcionc { get; set; }

        public string b_rastrac { get; set; }
        public string b_chasisc { get; set; }
        public string s_nombre { get; set; }
        public string s_operadora { get; set; }

        public string n_folio { get; set; }
        public string f_dan { get; set; }
        public string ClaveRe { get; set; }
        public int b_escaner { get; set; }
        public bool b_escan { get; set; }
        public int idClave { get; set; }

        public string c_navi { get; set; }
        public decimal CalcDias { get; set; }
        public double CalcDiasD { get; set; }
        public DateTime f_liberado { get; set; }
        public DateTime f_retenido { get; set; }
        public DateTime f_revision { get; set; }
        public DateTime f_tramite { get; set; }
        public string f_tramite_s { get; set; }
        public DateTime f_recep_patio { get; set; }
        public int t_revision { get; set; }

        public int t_detalle { get; set; }
        public DateTime f_cancelado { get; set; }

        public string b_shipper { get; set; }
        public string b_rdt { get; set; }

        public string b_trans { get; set; }
        public string f_trans { get; set; }
        public string f_recep { get; set; }

        public string c_llegada { get; set; }
        public string c_tarja { get; set; }

        public string b_transferencia { get; set; }
        public string b_manejo { get; set; }
        public string b_despacho { get; set; }

        public string c_cliente { get; set; }

        public int n_booking { get; set; }
        public string s_exportador { get; set; }
        public string s_notificador { get; set; }
        public string c_tipo_doc { get; set; }
        public string c_arivu { get; set; }
        public string c_fauca { get; set; }
        public string c_dm { get; set; }
        public string c_dut { get; set; }
        public string c_dmti { get; set; }
        public string c_manifiesto { get; set; }
        public string b_recepcion_c { get; set; }
        public string b_emb_dir { get; set; }

        public DateTime f_tarja { get; set; }

        public string b_retenido { get; set; }

        public string b_requiere { get; set; }

        public DateTime f_llegada { get; set; }
        public string d_buque { get; set; }

        public string c_tratamiento { get; set; }

        public string c_voyage { get; set; }
        public string c_ctama { get; set; }
        public double d_20 { get; set; }
        public double d_4045 { get; set; }
        public double s_20 { get; set; }
        public double s_4045 { get; set; }

        public string f_retenidoc { get; set; }
        public string f_recep_patioc { get; set; }

        public DateTime f_ini_dan { get; set; }
        public string TipoRevision { get; set; }

        public string b_cancelado { get; set; }
        public string b_aduanas { get; set; }

        public int n_aduana { get; set; }
        public string us_aduana { get; set; }

        public string n_manifiesto { get; set; }
        public string a_manifiesto { get; set; }

        public string b_bloqueo { get; set; }

        public string TipoRe { get; set; }
        public string f_salidas { get; set; }

        public string s_marchamo { get; set; }
    }

    public class EnvioAuto
    {
        public string c_naviera_corto { get; set; }
        public int n_manifiesto { get; set; }
        public string c_voyaje { get; set; }

        public int IdReg { get; set; }

        public string an_manifiesto { get; set; }
    }

    public class Facturacion
    {
        public int c_correlativo { get; set; }
        public string n_contenedor { get; set; }
        public string c_tarja { get; set; }
        public string s_consignatario { get; set; }
        public string n_BL { get; set; }
        public string c_llegada { get; set; }
        public double v_peso { get; set; }
    }

    public class Declaracion
    {
        public string tipo_doc { get; set; }
        public string num_doc { get; set; }
        public string b_estado { get; set; }

        public string n_contenedor { get; set; }
        public string n_manifiesto { get; set; }
        public string n_declaracion { get; set; }
        public string TipoEstado { get; set; }
        public string Descripcion { get; set; }
        public string f_aduana { get; set; }
        public int IdRegAduana { get; set; }

        public string s_descripcion { get; set; }
        public string n_bl { get; set; }
        public string n_nit { get; set; }
        public string s_consignatario { get; set; }

        public string s_tipo { get; set; }
    }

    public class TrackingEnca
    {
        public int IdDeta { get; set; }
        public string n_contenedor { get; set; }
        public string c_tamaño { get; set; }
        public string c_llegada { get; set; }
        public string c_naviera { get; set; }
        public DateTime f_llegada { get; set; }

        public DateTime f_desatraque { get; set; }
        public List<TrackingDatails> TrackingList { get; set; }

        public string d_cliente { get; set; }
        public string d_buque { get; set; }

        public string b_estado { get; set; }
        public string b_trafico { get; set; }
        public string c_tarja { get; set; }
        public string n_manifiesto { get; set; }
        public DateTime f_tarja { get; set; }
        public decimal v_peso { get; set; }
        public string descripcion { get; set; }

        public string b_cancelado { get; set; }
        public string c_tarjasn { get; set; }
        public int con_tarjas { get; set; }

        public string b_requiere { get; set; }

        public string b_shipper { get; set; }

        public string pais_origen { get; set; }

    }

    public class TrackingDatails
    {
        public int IdDeta { get; set; }
        public string n_oficio { get; set; }
        public DateTime? f_rep_naviera { get; set; }
        public DateTime? f_aut_aduana { get; set; }
        public DateTime? f_recep_patio { get; set; }
        public DateTime? f_ret_dan_o { get; set; }
        public string f_ret_dan { get; set; }
        public DateTime? f_tramite_dan { get; set; }
        public DateTime? f_liberado_dan { get; set; }
        public string f_salida_carga { get; set; }
        public DateTime? f_solic_ingreso { get; set; }
        public DateTime? f_auto_patio { get; set; }
        public DateTime? f_puerta1 { get; set; }
        public string ubicacion { get; set; }
        public string c_llegada { get; set; }
        public string n_contenedor { get; set; }
        public string c_naviera { get; set; }
        public string s_comentarios { get; set; }
        public DateTime? f_trans_aduana { get; set; }
        public string s_consignatario { get; set; }
        public string descripcion { get; set; }
        public DateTime? f_caseta { get; set; }
        public string f_marchamo_dan { get; set; }

        public DateTime? f_recepA { get; set; }
        public string f_reg_aduana { get; set; }
        public string f_reg_selectivo { get; set; }

        public DateTime? f_lib_aduana { get; set; }
        public DateTime? f_ret_mag { get; set; }
        public DateTime? f_lib_mag { get; set; }
        public string f_cancelado { get; set; }
        public string f_cambio { get; set; }


        public string f_ret_ucc { get; set; }
        public DateTime? f_tramite_ucc { get; set; }
        public DateTime? f_liberado_ucc { get; set; }
        public string f_marchamo_ucc { get; set; }

        public string f_deta_dan { get; set; }
        public string f_deta_ucc { get; set; }

        public string f_retencion_dga { get; set; }
        public string f_lib_dga { get; set; }
    }

    public class ProvisionalesEnca
    {
        public string Descripcion { get; set; }
        public double Total { get; set; }
        public string c_llegada { get; set; }
        public string n_contenedor { get; set; }
        public int IdDeta { get; set; }

        public List<ProvisionalesDeta> ProviList { get; set; }
    }

    public class ProvisionalesDeta
    {
        public int IdDeta { get; set; }
        public string C_Llegada { get; set; }
        public string Contenedor { get; set; }
        public DateTime Fecha_Prv { get; set; }
        public string Tipo { get; set; }
        public string Motorista_Prv { get; set; }
        public string Transporte_Prv { get; set; }
        public string Placa_Prv { get; set; }
        public string Chasis_Prv { get; set; }
        public DateTime? Fec_Reserva { get; set; }
        public DateTime? Fec_Valida { get; set; }
        public DateTime? Fecha_Ing { get; set; }
        public double Recno { get; set; }
        public double Num_Prv { get; set; }
        public double Num_Corr { get; set; }
        public string Cod_Prv { get; set; }
    }

    public class Tarjas
    {
        public string c_tarja { get; set; }
        public string d_marcas { get; set; }
        public string s_descripcion { get; set; }
        public string c_llegada { get; set; }
        public DateTime f_tarja { get; set; }
        public double v_peso { get; set; }
        public string c_contenedor { get; set; }
        public int con_tarjas { get; set; }
        public string c_bl { get; set; }

    }

    public class TipoRevisiones
    {
        public int IdRevision { get; set; }
        public string t_revision { get; set; }

        public int Years { get; set; }

    }

    public class FacturasTarjas
    {
        public string c_factura { get; set; }
        public string c_preimpreso { get; set; }
        public DateTime f_factura { get; set; }
        public string s_detalle { get; set; }

        public double t_factura { get; set; }

        public string fa_factura { get; set; }
    }

}
