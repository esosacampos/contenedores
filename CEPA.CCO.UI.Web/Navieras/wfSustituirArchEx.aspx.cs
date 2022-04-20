using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CEPA.CCO.Entidades;
using CEPA.CCO.DAL;
using CEPA.CCO.BL;
using System.Text;
using CEPA.CCO.Linq;
using System.Threading;
using System.IO;

namespace CEPA.CCO.UI.Web
{
    public partial class wfSustituirArchEx : System.Web.UI.Page
    {
        public string c_imo_c { get; set; }
        public string c_llegada_c { get; set; }
        public string c_usuario { get; set; }
        public DateTime f_llegada_c { get; set; }
        public string d_buque { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            //ClientScript.GetPostBackEventReference(btnCargar, string.Empty);

            try
            {

                if (!IsPostBack)
                {
                    if (Request.QueryString["c_buque"] == null || Request.QueryString["c_llegada"] == null || Request.QueryString["c_cliente"] == null)
                    {
                        throw new Exception("Falta código de buque");
                    }
                    else
                    {
                        CargarEncabezado();
                        CargarArchivos();
                        if(lblArchivos.Items.Count <= 0)
                            throw new Exception("CEPA-Contenedores Error: Este buque no posee archivos disponibles a sustituir..");
                    }

                    
                }
            }
            catch (Exception ex)
            {

                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ex.Message + "');setTimeout(function () { window.location.href = '/Navieras/wfPrincipalNavi.aspx'; }, 2000);", true);
            }
        }

        #region "Metodos"

        private void CargarEncabezado()
        {
            EncaBuqueBL _encaBL = new EncaBuqueBL();
            List<EncaBuque> _encaList = new List<EncaBuque>();
            _encaList = _encaBL.ObtenerBuqueID(DBComun.Estado.verdadero, Request.QueryString["c_cliente"].ToString(), Request.QueryString["c_buque"].ToString(), Request.QueryString["c_llegada"].ToString());

            if (_encaList.Count > 0)
            {              
                foreach (EncaBuque item in _encaList)
                {
                    foreach (var itemNav in EncaNavieraDAL.GetNavieras(DBComun.Estado.verdadero).Where(a => a.c_naviera == Request.QueryString["c_cliente"].ToString()))
                    {
                        d_agencia.Text = itemNav.d_naviera_p;
                        hIsoNavi.Value = itemNav.c_iso_navi;
                        break;
                    }
                    c_imo.Text = item.c_imo;
                    d_buque_c.Text = item.d_buque;
                    f_llegada.Text = item.f_llegada.ToString("dd/MM/yyyy HH:mm:ss");
                    c_llegada.Text = item.c_llegada;
                    //h_iBooking.Value = Session["b_ibooking"].ToString();
                    hNaviera.Value = Request.QueryString["c_cliente"].ToString();
                }
            }
        }

        private void CargarArchivos()
        {
            lblArchivos.DataTextField = "s_archivo";
            lblArchivos.DataValueField = "s_archivo";
            lblArchivos.DataSource = DetaDocDAL.ObtenerDocSEx(DBComun.Estado.verdadero, Request.QueryString["c_cliente"].ToString(), c_llegada.Text);
            lblArchivos.DataBind();
        }

        #endregion

        protected void lblArchivos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("wfPrincipalNavi.aspx");
        }



      


    }
}