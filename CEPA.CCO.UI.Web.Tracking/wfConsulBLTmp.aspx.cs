using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CEPA.CCO.Entidades;
using CEPA.CCO.DAL;

namespace CEPA.CCO.UI.Web.Tracking
{
    public partial class wfConsulBLTmp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //divScrolls.Visible = false;
            }
        }

        [System.Web.Services.WebMethod]
        public static string getFact(string c_tarja)
        {
            try
            {
                if (string.IsNullOrEmpty(c_tarja))
                    throw new Exception("No se posee tarja asociada");


                List<FacturasTarjas> pTarjas = new List<FacturasTarjas>();


                pTarjas = (from a in EncaBuqueDAL.getFacturasTarja(c_tarja)
                           select new FacturasTarjas
                           {
                               c_factura = a.c_factura,
                               c_preimpreso = a.c_preimpreso,
                               fa_factura = a.f_factura.ToString("dd/MM/yyyy HH:mm"),
                               s_detalle = a.s_detalle,
                               t_factura = a.t_factura,
                               f_factura = a.f_factura
                           }).ToList();

                var query = Newtonsoft.Json.JsonConvert.SerializeObject(pTarjas);

                return query;


            }
            catch (Exception ex)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(ex.Message);
            }
        }

        [System.Web.Services.WebMethod]
        public static string getBLs(string c_llegada, string n_contenedor)
        {
            try
            {
                if (string.IsNullOrEmpty(c_llegada) || string.IsNullOrEmpty(n_contenedor))
                    throw new Exception("Debe indicar BL a buscar");

                List<Entidades.BL> pList = new List<Entidades.BL>();
                List<Entidades.BL> pList2 = new List<Entidades.BL>();
                List<Entidades.BL> pBL = new List<Entidades.BL>();
                pBL = BlDAL.getBL(c_llegada, DBComun.Estado.verdadero, "tmp");
                string c_nul = "";

                if (pBL.Count > 0)
                {
                    foreach (var item in pBL)
                    {
                        c_nul = item.c_nul;
                        break;
                    }
                    string c_cliente = "";
                    string l_cliente = "";
                    List<Tarjas> pTarjas = new List<Tarjas>();
                    pTarjas = EncaBuqueDAL.getTarjaxNUL(c_llegada, n_contenedor);

                    if (pTarjas.Count > 0)
                    {
                        foreach (var itemC in pTarjas)
                        {
                            c_cliente = itemC.c_cliente;
                            l_cliente = itemC.tcliente;
                            break;
                        }
                    }

                    pList = (from a in pBL
                             join b in pTarjas on new { c_llegada = a.c_llegada, n_contenedor = a.n_contenedor } equals new { c_llegada = b.c_llegada, n_contenedor = b.d_marcas }
                             join c in ClienteDAL.getTipoClienteLL(DBComun.Estado.verdadero, c_cliente, l_cliente) on b.c_cliente equals c.c_cliente
                             select new Entidades.BL
                             {
                                 c_nul = a.c_nul,
                                 c_prefijo = a.c_prefijo,
                                 c_llegada = a.c_llegada,
                                 n_manifiesto = a.n_manifiesto,
                                 n_BL = b.c_bl,
                                 n_contenedor = a.n_contenedor,
                                 c_tamaño = a.c_tamaño,
                                 v_peso = a.v_peso,
                                 v_teus = a.v_teus,
                                 c_trafico = a.c_trafico,
                                 f_salidas = a.f_salida.ToString("dd/MM/yyyy HH:mm"),
                                 vn_manejo = a.vn_manejo,
                                 vc_manejo = a.vc_manejo,
                                 p_manejo = a.p_manejo,
                                 vn_transfer = a.vn_transfer,
                                 vc_transfer = a.vc_transfer,
                                 p_transfer = a.p_transfer,
                                 vn_desp = a.vn_desp,
                                 vc_desp = a.vc_desp,
                                 p_desp = a.p_desp,
                                 n_alm = a.n_alm,
                                 c_alm = a.c_alm,
                                 c_tarja = b.c_tarja,
                                 f_tarja = b.f_tarja.ToString("dd/MM/yyyy"),
                                 ta_alm = a.ta_alm,
                                 t_retencion = a.t_retencion,
                                 reff = a.reff,
                                 peso_entregado = a.peso_entregado,
                                 f_retenciones = a.f_retencion.ToString("dd/MM/yyyy HH:mm"),
                                 v_peso_tm = a.v_peso_tm,
                                 fs_recepcion = a.f_recepcion.ToString("dd/MM/yyyy"),
                                 fs_retiro = a.f_retiro.ToString("dd/MM/yyyy"),
                                 ValAlma = "",
                                 c_cliente = c.c_cliente,
                                 s_cliente = c.s_nombre_comercial,
                                 f_pago = c.facilidadPago
                             }).ToList();

                    if (pList.Count > 0)
                    {
                        foreach (var item in pList)
                        {
                            List<Entidades.BL> pBl = new List<Entidades.BL>();
                            List<Pago> _lstPagos = PagoDAL.ConsultarPago(item.c_tarja, item.n_contenedor);



                            if (_lstPagos.Count > 0)
                            {
                                foreach (var iPago in _lstPagos)
                                {
                                    Entidades.BL _BL = new Entidades.BL
                                    {
                                        c_nul = item.c_nul,
                                        c_prefijo = item.c_prefijo,
                                        c_llegada = item.c_llegada,
                                        n_manifiesto = item.n_manifiesto,
                                        n_BL = item.n_BL,
                                        n_contenedor = item.n_contenedor,
                                        c_tamaño = item.c_tamaño,
                                        v_peso = item.v_peso,
                                        v_teus = item.v_teus,
                                        c_trafico = item.c_trafico,
                                        f_salidas = item.f_salidas,
                                        vn_manejo = item.vn_manejo,
                                        vc_manejo = item.vc_manejo,
                                        p_manejo = iPago.ValManejo == "No" ? item.p_manejo : 0.00,
                                        vn_transfer = item.vn_transfer,
                                        vc_transfer = item.vc_transfer,
                                        p_transfer = iPago.ValTransfer == "No" ? item.p_transfer : 0.00,
                                        vn_desp = item.vn_desp,
                                        vc_desp = item.vc_desp,
                                        p_desp = iPago.ValDespacho == "No" ? item.p_desp : 0.00,
                                        n_alm = item.n_alm,
                                        c_alm = item.c_alm,
                                        c_tarja = item.c_tarja,
                                        f_tarja = item.f_tarja,
                                        ta_alm = item.ta_alm,
                                        t_retencion = item.t_retencion,
                                        reff = item.reff,
                                        peso_entregado = item.peso_entregado,
                                        f_retenciones = item.f_retenciones,
                                        v_peso_tm = item.v_peso_tm,
                                        fs_recepcion = item.fs_recepcion,
                                        fs_retiro = item.fs_retiro,
                                        ValAlma = iPago.ValAlmacenaje,
                                        c_cliente = item.c_cliente,
                                        s_cliente = item.s_cliente,
                                        f_pago = item.f_pago
                                    };

                                    pList2.Add(_BL);
                                }
                            }
                            else
                            {

                            }
                            List<FacturasTarjas> pFactu = new List<FacturasTarjas>();
                            pFactu = EncaBuqueDAL.getFacturasTarja(item.c_tarja);

                            if (pFactu.Count > 0)
                            {
                                if (pList2.Count > 0)
                                {
                                    foreach (var itM in pList2)
                                    {
                                        itM.t_facturaM = pFactu.Sum(x => x.t_factura);
                                        itM.f_calc_retiroM = pFactu.Max(x => x.f_calc_retiro);
                                    }
                                }
                            }

                        }
                    }

                }
                else
                    throw new Exception("La búsqueda no produjó resultados, verificar el # de BL consultado y volverlo a intentar");

                var query = (dynamic)null;

                if (pList2.Count > 0)
                {
                    query = Newtonsoft.Json.JsonConvert.SerializeObject(pList2);
                }
                else
                {
                    query = Newtonsoft.Json.JsonConvert.SerializeObject(pList);
                }

                return query;


            }
            catch (Exception ex)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(ex.Message);
            }
        }
    }
}