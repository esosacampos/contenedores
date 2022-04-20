using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CEPA.CCO.Entidades;
using CEPA.CCO.BL;
using CEPA.CCO.DAL;
using System.Data.SqlClient;
using CEPA.CCO.Linq;
using System.Threading;
using System.Globalization;
using System.Drawing;

using Newtonsoft.Json;


namespace CEPA.CCO.UI.Web
{
    public partial class wfConsulBLNvo: System.Web.UI.Page
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
        public static string getBLs(string c_llegada, string n_contenedor, DateTime f_salida_user)
        {
            try
            {
                if (string.IsNullOrEmpty(c_llegada) || string.IsNullOrEmpty(n_contenedor))
                    throw new Exception("Debe indicar BL a buscar");

                List<Entidades.BL2> pList = new List<Entidades.BL2>();
                List<Entidades.BL2> pList2 = new List<Entidades.BL2>();
                List<Entidades.BL2> pBL = new List<Entidades.BL2>();
                //DateTime pf_salida_user = (f_salida_user == null) ? DateTime.Today : f_salida_user;
                List<Tarjas> pTarjas = new List<Tarjas>();
                pTarjas = EncaBuqueDAL.getTarjaxNUL(c_llegada, n_contenedor);

                //pBL = BlDAL.getBL2(c_llegada, DBComun.Estado.verdadero, n_contenedor, "0", f_salida_user);
                //string c_nul = "";

                if (pTarjas.Count > 0)
                {
                    /*foreach (var item in pBL)
                    {
                        c_nul = item.c_nul;
                        break;
                    }*/

                    string c_cliente = "";
                    string l_cliente = "";
                    
                   

                    if (pTarjas.Count > 0)
                    {
                        foreach (var itemC in pTarjas)
                        {
                            c_cliente = itemC.c_cliente;
                            l_cliente = itemC.tcliente;
                            break;
                        }
                    }
                    List<TipoCliente> pLstClientes = new List<TipoCliente>();
                    if (c_cliente.Length > 0)
                        pLstClientes = ClienteDAL.getTipoClienteLL(DBComun.Estado.verdadero, c_cliente, l_cliente);

                    string  b_facilidad = "0";
                    foreach (var itemClie in pLstClientes)
                    {
                        b_facilidad = itemClie.facilidadPago.Contains("CON") ? "1" : "0";
                        break;
                     }

                    pList = (from b in pTarjas
                             join c in pLstClientes  on b.c_cliente equals c.c_cliente
                             join a in BlDAL.getBL2(c_llegada, DBComun.Estado.verdadero, n_contenedor, b_facilidad, f_salida_user) on new { c_llegada = b.c_llegada, n_contenedor = b.d_marcas } equals new { c_llegada =  a.c_llegada, n_contenedor = a.n_contenedor }                             
                             select new Entidades.BL2
                             {
                                 c_nul = a.c_nul,
                                 c_prefijo = a.c_prefijo,
                                 c_llegada = a.c_llegada,
                                 n_manifiesto = a.n_manifiesto,
                                 n_bl = b.c_bl,
                                 n_contenedor = a.n_contenedor,
                                 c_tamaño = a.c_tamaño,
                                 v_peso = (decimal)b.v_peso,
                                 v_teus = a.v_teus,
                                 c_trafico = a.c_trafico,
                                 f_salidas = a.f_salida.ToString("dd/MM/yyyy HH:mm"),
                                 vn_manejo = (a.n_manejo.Equals("X")) ? (double)a.v_manejo : 0,
                                 vc_manejo = (a.c_manejo.Equals("X")) ? (double)a.v_manejo : 0,
                                 p_manejo = (double)a.v_manejo,
                                 vn_transfer = (a.n_transferencia.Equals("X")) ? (double)a.v_transferencia : 0,
                                 vc_transfer = (a.c_transferencia.Equals("X")) ? (double)a.v_transferencia : 0,
                                 p_transfer = (double)a.v_transferencia,
                                 vn_desp = (a.n_despacho.Equals("X")) ? (double)a.v_despacho : 0,
                                 vc_desp = (a.c_despacho.Equals("X")) ? (double)a.v_despacho : 0,
                                 p_desp = (double)a.v_despacho,
                                 n_almacenaje = a.n_almacenaje,
                                 c_almacenaje = a.c_almacenaje,
                                 vn_almacenaje = (a.n_almacenaje.Equals("X")) ? (double)a.v_almacenaje : 0,
                                 vc_almacenaje = (a.c_almacenaje.Equals("X")) ? (double)a.v_almacenaje : 0,
                                 c_tarja = b.c_tarja,
                                 f_tarja = b.f_tarja.ToString("dd/MM/yyyy"),
                                 ta_alm = a.ta_alm,
                                 t_retencion = a.t_retencion,
                                 reff = a.reff,
                                 peso_entregado = a.peso_entregado,
                                 f_retenciones = a.f_retencion.ToString("dd/MM/yyyy HH:mm"),
                                 dias_total = a.dias_t_ant+a.dias_t_nva,
                                 c_cliente = c.c_cliente,
                                 s_cliente = c.s_nombre_comercial,
                                 f_pago = c.facilidadPago,
                                 p_almacenaje = (double)a.v_almacenaje
                             }).ToList();

                    if (pList.Count > 0)
                    {
                        foreach (var item in pList)
                        {
                            List<Entidades.BL2> pBl = new List<Entidades.BL2>();
                            List<Pago> _lstPagos = PagoDAL.ConsultarPago(item.c_tarja, item.n_contenedor);
                                                        
                            if(_lstPagos.Count > 0)
                            {                                
                                foreach (var iPago in _lstPagos)
                                {
                                    Entidades.BL2 _BL = new Entidades.BL2
                                    {
                                        c_nul = item.c_nul,
                                        c_prefijo = item.c_prefijo,
                                        c_llegada = item.c_llegada,
                                        n_manifiesto = item.n_manifiesto,
                                        n_bl = item.n_bl,
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
                                        n_almacenaje = item.n_almacenaje,
                                        c_almacenaje = item.c_almacenaje,
                                        vn_almacenaje = item.vn_almacenaje,
                                        vc_almacenaje = item.vc_almacenaje,                                        
                                        c_tarja = item.c_tarja,
                                        f_tarja = item.f_tarja,
                                        ta_alm = item.ta_alm,
                                        t_retencion = item.t_retencion,
                                        reff = item.reff,
                                        peso_entregado = item.peso_entregado,
                                        f_retenciones = item.f_retenciones,
                                        c_cliente = item.c_cliente,
                                        s_cliente = item.s_cliente,
                                        f_pago = item.f_pago,
                                        p_almacenaje = item.p_almacenaje
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
                                        itM.t_facturam = pFactu.Sum(x => x.t_factura);
                                        itM.f_calc_retirom = pFactu.Max(x => x.f_calc_retiro);
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
                }else
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