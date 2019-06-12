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

namespace CEPA.CCO.UI.Web.Navieras
{
    public partial class wfCancelarCon : System.Web.UI.Page
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
                        List<DocBuque> _encaList = DocBuqueLINQ.ObtenerAduanaIdCancel(Convert.ToInt32(Request.QueryString["IdReg"]), Session["c_naviera"].ToString());
                        //Sacar naviera
                        if (_encaList.Count > 0)
                        {
                            foreach (DocBuque item in _encaList)
                            {
                                c_imo.Text = item.c_imo;
                                d_buque.Text = item.d_buque;
                                f_llegada.Text = item.f_llegada.ToString("dd/MM/yyyy HH:mm:ss");
                                c_llegada.Text = item.c_llegada;
                                c_viaje.Value = item.c_voyage;
                                c_prefijo.Value = item.c_prefijo;
                                break;
                            }

                            Cargar();
                        }
                    }

                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ex.Message + "');", true);
                }
            }
        }

        private void Cargar()
        {
            GridView1.DataSource = DetaNavieraDAL.ObtenerDetalleCancel(Convert.ToInt32(Request.QueryString["IdReg"])).OrderBy(a=> a.c_correlativo);
            GridView1.DataBind();

            GridView1.HeaderRow.Cells[0].Attributes["data-class"] = "expand";

            GridView1.HeaderRow.Cells[2].Attributes["data-hide"] = "phone";
            GridView1.HeaderRow.Cells[3].Attributes["data-hide"] = "phone";
            GridView1.HeaderRow.Cells[4].Attributes["data-hide"] = "phone";


            GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;
            GridView1.FooterRow.Cells[0].Attributes["text-align"] = "center";
            GridView1.FooterRow.TableSection = TableRowSection.TableFooter;
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("wfConsultaCancel.aspx");
        }

        protected void btnCargar_Click(object sender, EventArgs e)
        {
            CargarArchivosLINQ _cargar = new CargarArchivosLINQ();
            try
            {
               
                int cantidad = GridView1.Rows.Count;
                List<int> pListDeta = new List<int>();
                List<string> pListCon = new List<string>();

                SeleccionManager.CanceladosID((GridView)GridView1);
                List<DetaNaviera> _lit = HttpContext.Current.Session["Cancelados"] as List<DetaNaviera>;

                if (_lit == null)
                    _lit = new List<DetaNaviera>();

                if (_lit.Count > 0)
                {
                    foreach (DetaNaviera item in _lit)
                    {
                        if (item.b_retenido != "RETENIDO")
                        {
                            if (item.s_observaciones != null && item.s_observaciones.Trim().TrimEnd().TrimStart().Length > 0)
                            {
                                int _resultado = Convert.ToInt32(DetaNavieraDAL.ActualizarCancelarId(DBComun.Estado.verdadero, item.IdDeta, item.s_observaciones, User.Identity.Name));

                                if (_resultado > 0)
                                {
                                    pListDeta.Add(item.IdDeta);
                                }
                            }
                            else
                            {
                                throw new Exception(string.Format("El contenedor {0} no posee justificación de su cancelación.", item.n_contenedor));
                            }
                        }
                        else
                        {
                            throw new Exception(string.Format("El contenedor {0} se encuentra solicitado para revisión, proceder con la solicitud a la DAN de su liberación; luego realizar cancelación", item.n_contenedor));
                        }

                    }

                    if (pListDeta.Count > 0)
                    {


                        EnviarCorreo(d_buque.Text, pListDeta.Count, cantidad, _lit, Session["c_naviera"].ToString());
                        
                        //_cargar.Clear("wfConsultaCancel.aspx");
                        

                        ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('Cantidad de contenedores cancelados " + pListDeta.Count + "');", true);
                        Cargar();



                    }
                }
                else
                    throw new Exception("No se encuentran contenedores seleccionados para cancelar.");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ex.Message + "');", true);
            }
            finally
            {
                HttpContext.Current.Session["Cancelados"] = null;
                Cargar();
            }
        }

        public void EnviarCorreo(string d_buque, int pValor, int pCantidad, List<DetaNaviera> pLista, string c_cliente)
        {
            string Html;
            int i = 1;
            EnvioCorreo _correo = new EnvioCorreo();
            try
            {

                Html = "<dir style=\"font-family: 'Arial'; font-size: 12px; line-height: 1.2em\">";
                Html += "MÓDULO : LISTADO DE CONTENEDORES CANCELADOS  <br />";
                Html += "TIPO DE MENSAJE : NOTIFICACIÓN DE CONTENEDORES CANCELADOS <br /><br />";
                Html += string.Format("El presente listado de contenedores correspondientes al barco {0} han sido cancelados {1} de {2} contenedores correspondientes a este barco.-", d_buque, pValor, pCantidad);
                Html += "<br /><br/>";

                if (pLista == null)
                    pLista = new List<DetaNaviera>();



                if (pLista.Count > 0)
                {
                    Html += "<table style=\"font-family: 'Arial' ; font-size: 12px;  line-height: 1em;width: 100%;border: thin solid #4F81BD; border-collapse: separate; border-spacing:0px; \">";
                    Html += "<tr>";
                    Html += "<center>";
                    Html += "<td width=\"5%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>No.</td>";
                    Html += "<td width=\"30%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>CONTENEDOR</font></th>";
                    Html += "<td width=\"30%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>TAMAÑO</font></th>";
                    Html += "<td width=\"30%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>JUSTIFICACIÓN</font></th>";
                    Html += "</center>";
                    Html += "</tr>";   
                    foreach (DetaNaviera item in pLista)
                    {
                        Html += "<tr>";
                        Html += "<center>";
                        Html += "<td width=\"6%\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.c_correlativo + "</font></td>";
                        Html += "<td width=\"10%\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.n_contenedor + "</font></td>";
                        Html += "<td width=\"10%\" style=\"border-right: thin solid #4F81BD\"><font size = 2 color=blue>" + item.c_tamaño + "</font></td>";
                        Html += "<td width=\"35%\" style=\"border-right: thin solid #4F81BD\"><font size = 2 color=blue>" + item.s_observaciones + "</font></td>";
                        Html += "</center>";
                        Html += "</tr>";
                        Html += "</font>";  
                    }
                }
                Html += "</table><br /><br/>";
                Html += "<br />";
                Html += "<br />";
               
                _correo.Subject = string.Format("CANCELADOS: Listado de Contenedores Cancelados de {0} para el buque {1}, # de Viaje {2}", c_prefijo.Value, d_buque, c_viaje.Value);
                //_correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_cancela", DBComun.Estado.verdadero, c_cliente);
                //_correo.ListaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_cancela", DBComun.Estado.verdadero, c_cliente);

                Notificaciones noti = new Notificaciones
                {
                    sMail = "elsa.sosa@cepa.gob.sv",
                    dMail = "Elsa Sosa"
                };

                List<Notificaciones> pLisN = new List<Notificaciones>();

                pLisN.Add(noti);

                _correo.ListaNoti = pLisN;

                _correo.Asunto = Html;
                _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.verdadero);
                _correo = null;
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }

        protected void onRowCreate(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int colSpan = e.Row.Cells.Count;

                for (int i = (e.Row.Cells.Count - 1); i >= 1; i -= 1)
                {
                    e.Row.Cells.RemoveAt(i);
                    e.Row.Cells[0].ColumnSpan = colSpan;
                }

                e.Row.Cells[0].Controls.Add(new LiteralControl("<ul class='pagination pagination-centered hide-if-no-paging'></ul><div class='divider'  style='margin-bottom: 15px;'></div></div><span class='label label-default pie' style='background-color: #dff0d8;border-radius: 25px;font-family: sans-serif;font-size: 18px;color: #468847;border-color: #d6e9c6;margin-top: 18px;'></span>"));
            }
        }



    }
}