using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CEPA.CCO.BL;
using CEPA.CCO.DAL;
using CEPA.CCO.Entidades;
using CEPA.CCO.Linq;
using Microsoft.Office.Interop.Excel;
using System.Threading;


namespace CEPA.CCO.UI.Web.Navieras
{
    public partial class wfCargarBooking : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["c_naviera"] == null)
                {
                    Response.Write("<script>alert(' Naviera asociada no existe ');</script>");
                }
                else
                {
                    List<EncaBuque> _listEnca = EncaBuqueDAL.ObtenerNaviera(DBComun.Estado.verdadero, Session["c_naviera"].ToString());
                    foreach (EncaBuque item in _listEnca)
                    {
                        d_naviera.Text = item.d_cliente;
                    }

                    f_registro.Text = DetaNavieraLINQ.FechaBD().ToShortDateString();

                }
            }

            // Propiedades del control.
            ucMultiFileUpload1.Titulo = "Cargar Archivo";
            ucMultiFileUpload1.Comment = "máx. 4 MB en total.";
            ucMultiFileUpload1.MaxFilesLimit = 5;
            ucMultiFileUpload1.DestinationFolder = "~/Archivos"; // única propiedad obligatoria.
            ucMultiFileUpload1.FileExtensionsEnabled = ".xls|.xlsx";
        }

        protected void btnCargar_Click(object sender, EventArgs e)
        {
            //try
            //{
                ucMultiFileUpload1.UploadFiles(true);
                CargarArchivosLINQ _cargar = new CargarArchivosLINQ();
                _cargar.Imprimir();
                _cargar.c_usuario = User.Identity.Name;
                _cargar.AlmacenarBooking();
                Thread.Sleep(4000);
                _cargar.Clear("wfCargarBooking.aspx");
            //}
            //catch (Exception ex)
            //{
            //    Response.Write("<script>alert('" + ex.Message + "');</script>");
            //    CargarArchivosLINQ _carga = new CargarArchivosLINQ();
            //    _carga.Clear("wfCargarBooking.aspx");
            //}
            //finally 
            //{
                if (Session["nuevoArch"] != null)
                {
                    if (System.IO.File.Exists(Session["nuevoArch"].ToString()))
                        System.IO.File.Delete(Session["nuevoArch"].ToString());
                }
            //}
        }
    }
}