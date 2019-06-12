using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CEPA.CCO.BL;
using CEPA.CCO.DAL;
using CEPA.CCO.Entidades;
using CEPA.CCO.Linq;
using Microsoft.Office.Interop.Excel;
using System.Threading;
using System.Globalization;

namespace CEPA.CCO.UI.Web.Navieras
{
    public partial class wfCargarExport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Request.QueryString["c_buque"] == null || Request.QueryString["c_llegada"] == null)
                    {
                        throw new Exception("Falta código de buque");
                    }
                    else
                    {
                        EncaBuqueBL _encaBL = new EncaBuqueBL();
                        List<EncaBuque> _encaList = new List<EncaBuque>();
                        _encaList = _encaBL.ObtenerBuqueID(DBComun.Estado.verdadero, Session["c_naviera"].ToString(), Request.QueryString["c_buque"].ToString(), Request.QueryString["c_llegada"].ToString());

                        if (_encaList.Count > 0)
                        {
                            foreach (EncaBuque item in _encaList)
                            {
                                c_imo.Text = item.c_imo;
                                d_buque_c.Text = item.d_buque;
                                f_llegada.Text = item.f_llegada.ToString("dd/MM/yyyy hh:mm tt");
                                c_llegada.Text = item.c_llegada;
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ex.Message + "');", true);
            }

        }

         public void btnCargar_Click(object sender, EventArgs e)
        {
            CargarList();

        }

        public void CargarList()
        {
            try
            {

                //ucMultiFileUpload1.UploadFiles(true);
                CargarArchivosLINQ _cargar = new CargarArchivosLINQ();
                //_cargar.Imprimir();
                _cargar.d_buque = d_buque_c.Text;
                _cargar.c_usuario = User.Identity.Name;
                _cargar.c_imo = c_imo.Text;
                _cargar.c_llegada = c_llegada.Text;
                _cargar.f_llegada = Convert.ToDateTime(f_llegada.Text, CultureInfo.CreateSpecificCulture("es-SV"));
                _cargar.AlmacenaCarga();
               // Thread.Sleep(4000);
                //_cargar.Clear(string.Format("wfCargar.aspx?c_buque={0}&c_llegada={1}", Request.QueryString["c_buque"].ToString(), Request.QueryString["c_llegada"].ToString()));
            }
            catch (FormatException ef)
            {
                String errorMessage;
                errorMessage = "Error: ";
                errorMessage = String.Concat(errorMessage, ef.Message);
                errorMessage = String.Concat(errorMessage, " Line: ");
                errorMessage = String.Concat(errorMessage, ef.Source);
                Response.Write("<script>alert('" + errorMessage + "');</script>");
                CargarArchivosLINQ _carga = new CargarArchivosLINQ();
                _carga.Clear(string.Format("wfCargar.aspx?c_buque={0}&c_llegada={1}", Request.QueryString["c_buque"].ToString(), Request.QueryString["c_llegada"].ToString()));
            }
            catch (Exception ex)
            {

                String errorMessage;
                errorMessage = "Error: ";
                errorMessage = String.Concat(errorMessage, ex.Message);
                errorMessage = String.Concat(errorMessage, " Line: ");
                errorMessage = String.Concat(errorMessage, ex.Source);
                Response.Write("<script>alert('" + errorMessage + "');</script>");
                CargarArchivosLINQ _carga = new CargarArchivosLINQ();
                _carga.Clear(string.Format("wfCargar.aspx?c_buque={0}&c_llegada={1}", Request.QueryString["c_buque"].ToString(), Request.QueryString["c_llegada"].ToString()));
            }
            finally
            {
                HttpContext.Current.Session["listaValid"] = null;
            }
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("wfPrincipalNavi.aspx");
        }
    }
}