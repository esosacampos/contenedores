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
        public static string getBLs(string buscar)
        {
            try
            {
                if (string.IsNullOrEmpty(buscar))
                    throw new Exception("Debe indicar el # de BL a buscar");

                List<Entidades.BL> pList = new List<Entidades.BL>();
                List<Entidades.BL> pBL = new List<Entidades.BL>();
                pBL = BlDAL.getBL(buscar, DBComun.Estado.verdadero);
                string c_nul = "";

                if (pBL.Count > 0)
                {
                    foreach (var item in pBL)
                    {
                        c_nul = item.c_nul;
                    }

                    List<Tarjas> pTarjas = new List<Tarjas>();
                    pTarjas = EncaBuqueDAL.getTarjaxNUL(c_nul);

                    pList = (from a in pBL
                             join b in pTarjas on new { c_llegada = a.c_llegada, n_contenedor = a.n_contenedor } equals new { c_llegada = b.c_llegada, n_contenedor = b.d_marcas }
                             select new Entidades.BL
                             {
                                 c_nul = a.c_nul,
                                 c_prefijo = a.c_prefijo,
                                 c_llegada = a.c_llegada,
                                 n_manifiesto = a.n_manifiesto,
                                 n_BL = a.n_BL,
                                 n_contenedor = a.n_contenedor,
                                 c_tamaño = a.c_tamaño,
                                 v_peso = a.v_peso,
                                 v_teus = a.v_teus,
                                 c_trafico = a.c_trafico,
                                 f_salidas = a.f_salida.ToString("dd/MM/yy HH:mm"),
                                 n_manejo = a.n_manejo,
                                 c_manejo = a.c_manejo,
                                 n_transfer = a.n_transfer,
                                 c_transfer = a.c_transfer,
                                 n_desp = a.n_desp,
                                 c_desp = a.c_desp,
                                 n_alm = a.n_alm,
                                 c_alm = a.c_alm,
                                 c_tarja = b.c_tarja,
                                 f_tarja = b.f_tarja.ToString("dd/MM/yyyy")
                             }).OrderBy(x => x.f_tarja).ToList();
                }
                else
                    throw new Exception("La búsqueda no produjó resultados, verificar el # de BL consultado y volverlo a intentar");
                
                
                var query = Newtonsoft.Json.JsonConvert.SerializeObject(pList);

                return query;


            }
            catch (Exception ex)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(ex.Message);
            }
        }
    }
}