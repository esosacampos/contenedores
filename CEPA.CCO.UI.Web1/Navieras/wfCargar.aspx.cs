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
using System.Globalization;

namespace CEPA.CCO.UI.Web
{
    public partial class wfCargar : System.Web.UI.Page
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
                    // Propiedades del control.
                    ucMultiFileUpload1.Titulo = "Cargar Archivo";
                    ucMultiFileUpload1.Comment = "máx. 4 MB en total.";
                    ucMultiFileUpload1.MaxFilesLimit = 5;
                    ucMultiFileUpload1.DestinationFolder = "~/Archivos"; // única propiedad obligatoria.
                    ucMultiFileUpload1.FileExtensionsEnabled = ".xls|.xlsx";

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void btnCargar_Click(object sender, EventArgs e)
        {
            try
            {

                ucMultiFileUpload1.UploadFiles(true);
                CargarArchivosLINQ _cargar = new CargarArchivosLINQ();
                _cargar.Imprimir();
                _cargar.d_buque = d_buque_c.Text;
                _cargar.c_usuario = User.Identity.Name;
                _cargar.c_imo = c_imo.Text;
                _cargar.c_llegada = c_llegada.Text;
                _cargar.f_llegada = Convert.ToDateTime(f_llegada.Text, CultureInfo.CreateSpecificCulture("es-SV"));
                _cargar.AlmacenaCarga();
                Thread.Sleep(4000);
                _cargar.Clear(string.Format("wfCargar.aspx?c_buque={0}&c_llegada={1}", Request.QueryString["c_buque"].ToString(), Request.QueryString["c_llegada"].ToString()));
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

        //public void EnviarCorreo()
        //{
        //    string Html;
        //    int i = 1;
        //    EnvioCorreo _correo = new EnvioCorreo();
        //    try
        //    {

        //        Html = "<dir style=\"font-family: 'Arial'; font-size: 12px; line-height: 1.2em\">";
        //        Html += "MÓDULO : LISTADO DE CONTENEDORES AUTORIZADOS  <br />";
        //        Html += "TIPO DE MENSAJE : NOTIFICACIÓN DE CONTENEDORES AUTORIZADOS <br /><br />";
        //        Html += string.Format("El presente listado de contenedores correspondientes a {0} para el barco {1}, con # de Viaje {2}, Manifiesto de Aduana # {3}  han sido autorizados {4} de {5} contenedores correspondientes a este barco.-", c_navi_corto, d_buque, c_viaje, n_manifiesto, pValor, pCantidad);
        //        Html += "<br /><br/>";

        //        if (pLista == null)
        //            pLista = new List<DetaNaviera>();

        //        if (pValor < pCantidad)
        //        {
        //            if (pLista.Count > 0)
        //            {
        //                Html += "Los siguientes contenedores fueron denegados por ADUANA para su revisión se detallan a continuación : ";
        //                Html += "<OL>";
        //                foreach (DetaNaviera item in pLista)
        //                {
        //                    Html += "<LI>" + item.n_contenedor;
        //                }
        //                Html += "</OL>";
        //            }
        //        }

        //        if (pArchivo.Count < 3 && c_cliente != "11")
        //        {
        //            Html += "<font color=red> Este barco no posee contenedores clasificados con peligrosidad </font><br/> ";
        //        }

        //        Html += "<br /><br />";
        //        Html += "<font style=\"color:#1F497D;\"><b> ACCIONES A REALIZAR: </b></font><br /><br />";
        //        Html += "<font style=\"color:#1F497D;\"><b> TODOS: </b></font><br />";
        //        Html += "<font color=blue>Impresión de los listados para ser utilizado en la operación del buque</font><br /><br />";
        //        Html += "<font style=\"color:#1F497D;\"><b> NAVIERA: </b></font><br />";
        //        Html += "<font color=blue>Remitir a CEPA el Manifiesto de Carga impreso y en digital </font><br /><br />";

        //        _correo.Subject = string.Format("PASO 4 de 4: Autorización de Listado de Importación de {0} para el Buque: {1}, # de Viaje {2}, Cod. de Llegada # {3}, Manifiesto de Aduana # {4}", c_navi_corto, d_buque, c_viaje, c_llegada, n_manifiesto);
        //        //_correo.Subject = string.Format("Listado de Contenedores Autorizados {0} con C. Llegada {1} ", d_buque, c_llegada);
        //        _correo.ListArch = pArchivo;
        //        _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_autoriza", DBComun.Estado.verdadero, c_cliente);


        //        List<Notificaciones> _listaCC = new List<Notificaciones>();

        //        if (c_cliente != "11")
        //            _listaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_autoriza", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());

        //        if (_listaCC == null)
        //            _listaCC = new List<Notificaciones>();

        //        _listaCC.AddRange(NotificacionesDAL.ObtenerNotificacionesCCN("b_noti_carga", DBComun.Estado.verdadero, "219"));
        //        _correo.ListaCC = _listaCC;

        //        _correo.Asunto = Html;
        //        _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.verdadero);
        //    }
        //    catch (Exception Ex)
        //    {
        //        throw new Exception(Ex.Message);
        //    }
        //}

    }
}