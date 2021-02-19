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
    public partial class wfConsulBL : System.Web.UI.Page
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
                pBL = BlDAL.getBL(c_llegada, DBComun.Estado.verdadero);
                string c_nul = "";

                if (pBL.Count > 0)
                {
                    foreach (var item in pBL)
                    {
                        c_nul = item.c_nul;
                        break;
                    }

                    List<Tarjas> pTarjas = new List<Tarjas>();
                    pTarjas = EncaBuqueDAL.getTarjaxNUL(c_llegada, n_contenedor);

                    pList = (from a in pBL
                             join b in pTarjas on new { c_llegada = a.c_llegada, n_contenedor = a.n_contenedor } equals new { c_llegada = b.c_llegada, n_contenedor = b.d_marcas }                             
                             select new Entidades.BL
                             {
                                 c_nul = a.c_nul,
                                 c_prefijo = a.c_prefijo,
                                 c_llegada = a.c_llegada,
                                 n_manifiesto = a.n_manifiesto,
                                 n_BL = b.c_bl,
                                 n_contenedor = a.n_contenedor,
                                 c_tamaño = a.c_tamaño,
                                 v_peso = b.v_peso,
                                 v_teus = a.v_teus,
                                 c_trafico = a.c_trafico,
                                 f_salidas = a.f_salida.ToString("dd/MM/yyyy HH:mm"),
                                 n_manejo = a.n_manejo,
                                 c_manejo = a.c_manejo,
                                 p_manejo = a.p_manejo,
                                 n_transfer = a.n_transfer,
                                 c_transfer = a.c_transfer,
                                 p_transfer = a.p_transfer,
                                 n_desp = a.n_desp,
                                 c_desp = a.c_desp,
                                 p_desp = a.p_desp,
                                 n_alm = a.n_alm,
                                 c_alm = a.c_alm,
                                 c_tarja = b.c_tarja,
                                 f_tarja = b.f_tarja.ToString("dd/MM/yyyy"),
                                 ta_alm = a.ta_alm,
                                 t_retencion = a.t_retencion,
                                 reff = a.reff,
                                 peso_entregado = a.peso_entregado
                             }).ToList();

                    if (pList.Count > 0)
                    {
                        foreach (var item in pList)
                        {
                            List<Entidades.BL> pBl = new List<Entidades.BL>();
                            List<Pago> _lstPagos = PagoDAL.ConsultarPago(item.c_tarja, item.n_contenedor);
                                                        
                            if(_lstPagos.Count > 0)
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
                                        n_manejo = item.n_manejo,
                                        c_manejo = item.c_manejo,
                                        p_manejo = iPago.ValManejo == "No" ? item.p_manejo : 0.00,
                                        n_transfer = item.n_transfer,
                                        c_transfer = item.c_transfer,
                                        p_transfer = iPago.ValTransfer == "No" ? item.p_transfer : 0.00,
                                        n_desp = item.n_desp,
                                        c_desp = item.c_desp,
                                        p_desp = iPago.ValDespacho == "No" ? item.p_desp : 0.00,
                                        n_alm = item.n_alm,
                                        c_alm = item.c_alm,
                                        c_tarja = item.c_tarja,
                                        f_tarja = item.f_tarja,
                                        ta_alm = item.ta_alm,
                                        t_retencion = item.t_retencion,
                                        reff = item.reff,
                                        peso_entregado = item.peso_entregado
                                    };                                                                       

                                    pList2.Add(_BL);
                                }
                            }
                            else
                            {

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