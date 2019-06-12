using System;
using CEPA.CCO.Entidades;
using CEPA.CCO.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using CEPA.CCO.BL;
using CEPA.CCO.DAL;
using System.Data.SqlClient;
using System.Threading;
using System.Globalization;

namespace CEPA.CCO.UI.Web.Aduana
{
    public partial class wfCargarValid : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (Request.QueryString["IdReg"] == null)
                    {
                        throw new Exception("Falta código de cabecera");
                    }
                    else
                    {
                        List<DocBuque> _encaList = DocBuqueLINQ.ObtenerAduanaId(Convert.ToInt32(Request.QueryString["IdReg"]));
                        //Sacar naviera
                        if (_encaList.Count > 0)
                        {
                            foreach (DocBuque item in _encaList)
                            {
                                IdRegP.Value = Request.QueryString["IdReg"].ToString();
                                c_imo.Text = item.c_imo;
                                d_buque.Text = item.d_buque;
                                f_llegada.Text = item.f_llegada.ToString("dd/MM/yyyy HH:mm:ss");
                                c_llegada.Text = item.c_llegada;
                                n_manif.Text = Request.QueryString["n_manif"].ToString();
                                IdDocP.Value = item.IdDoc.ToString();
                                a_mani.Value = item.a_manifiesto;
                                break;
                            }

                        }
                        else
                        {
                            //btnCargar.Enabled = false;
                        }

                    }
                    //ucMultiFileUpload1.Titulo = "Cargar Archivo";
                    //ucMultiFileUpload1.Comment = "máx. 4 MB en total.";
                    //ucMultiFileUpload1.MaxFilesLimit = 5;
                    //ucMultiFileUpload1.DestinationFolder = "~/Archivos"; // única propiedad obligatoria.
                    //ucMultiFileUpload1.FileExtensionsEnabled = ".xls|.xlsx";

                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ex.Message + "');", true);
                }
            }
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("wfConsultaBuques.aspx");
        }

        protected void btnCargar_Click(object sender, EventArgs e)
        {
            try
            {
                //ucMultiFileUpload1.UploadFiles(true);
                CargarArchivosLINQ _cargar = new CargarArchivosLINQ();
                _cargar.Imprimir();
                _cargar.c_usuario = User.Identity.Name;
                _cargar.n_manifiesto = Convert.ToInt32(n_manif.Text);
                _cargar.d_buque = d_buque.Text;
                _cargar.IdRegC = Convert.ToInt32(Request.QueryString["IdReg"]);
                _cargar.AlmacenarAduana();
                Thread.Sleep(4000);
                _cargar.Clear(string.Format("wfCargarValid.aspx?IdReg={0}&n_manif={1}", Request.QueryString["IdReg"].ToString(), Request.QueryString["n_manif"].ToString()));
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
                _carga.Clear(string.Format("wfCargarValid.aspx?IdReg={0}&n_manif={1}", Request.QueryString["IdReg"].ToString(), Request.QueryString["n_manif"].ToString()));
            }

        }
    }
}