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
using System.Web.Services;
using Newtonsoft.Json;
using System.Data;
using System.IO;
using System.Xml;
using System.Text;
using System.Drawing;

namespace CEPA.CCO.UI.Web
{
    public partial class wfDetalleDGA : System.Web.UI.Page
    {
        public string c_naviera;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {

                    if (Request.QueryString["n_manifiesto"] == null || Request.QueryString["c_cliente"] == null)
                    {
                        throw new Exception("Falta código de cabecera o Número de Oficio");
                    }
                    else
                    {
                        string _nOficio = DetaNavieraDAL.MaxDocDGA();

                        List<DocBuque> _encaList = DocBuqueLINQ.detalleDGA(Convert.ToString(Request.QueryString["n_manifiesto"]), Convert.ToString(Request.QueryString["c_cliente"]));
                        //Sacar naviera
                        if (_encaList.Count > 0)
                        {
                            foreach (DocBuque item in _encaList)
                            {
                                //n_oficio.Text = Session["Oficio"].ToString();
                                c_imo.Text = item.c_imo;
                                d_buque.Text = item.d_buque;
                                f_llegada.Text = item.f_llegada.ToString("dd/MM/yyyy HH:mm:ss");
                                c_llegada.Text = item.c_llegada;
                                viaje.Text = item.c_voyage;
                                manif.Text = item.num_manif.ToString();
                                c_naviera = item.c_cliente;                                
                                h_manifiesto.Value = item.a_manifiesto.Trim().TrimEnd().TrimStart();
                                i_oficio.Text = _nOficio;
                                IdRegP.Value = item.IdReg.ToString();
                                d_cliente.Text = item.d_cliente;
                                a_manif.Text = item.a_manifiesto.Trim().TrimEnd().TrimStart();
                                IdDocP.Value = item.IdDoc.ToString();
                            }

                            Cargar(Convert.ToInt32(IdDocP.Value));
                        }
                        else
                        {
                            // btnCargar.Enabled = false;
                        }

                    }

                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ex.Message + "');", true);
                }
            }

            GridView1.HeaderRow.Cells[0].Attributes["data-class"] = "expand";

            // GridView1.HeaderRow.Cells[1].Attributes["data-hide"] = "phone";
            GridView1.HeaderRow.Cells[3].Attributes["data-hide"] = "phone";
            GridView1.HeaderRow.Cells[4].Attributes["data-hide"] = "phone";
            GridView1.HeaderRow.Cells[5].Attributes["data-hide"] = "phone";
            GridView1.HeaderRow.Cells[6].Attributes["data-hide"] = "phone";
            GridView1.HeaderRow.Cells[7].Attributes["data-hide"] = "phone";
            GridView1.HeaderRow.Cells[8].Attributes["data-hide"] = "phone";
            //GridView1.HeaderRow.Cells[8].Attributes["data-hide"] = "phone";

            GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;

            GridView1.FooterRow.Cells[0].Attributes["text-align"] = "center";
            GridView1.FooterRow.TableSection = TableRowSection.TableFooter;
            //  ViewState["EmployeeList"] = GridView1.DataSource;
           
          
        }

        private void Cargar(int IdDoc)
        {
            List<DetaNaviera> pLista = DetaNavieraDAL.detaDGACnt(IdDoc).OrderBy(y => y.c_tamaño).ToList();
            
            GridView1.DataSource = pLista;
            GridView1.DataBind();

            GridView1.HeaderRow.Cells[0].Attributes["data-class"] = "expand";

            // GridView1.HeaderRow.Cells[1].Attributes["data-hide"] = "phone";
            GridView1.HeaderRow.Cells[3].Attributes["data-hide"] = "phone";
            GridView1.HeaderRow.Cells[4].Attributes["data-hide"] = "phone";
            GridView1.HeaderRow.Cells[5].Attributes["data-hide"] = "phone";
            GridView1.HeaderRow.Cells[6].Attributes["data-hide"] = "phone";
            GridView1.HeaderRow.Cells[7].Attributes["data-hide"] = "phone";
           
            //GridView1.HeaderRow.Cells[8].Attributes["data-hide"] = "phone";

            GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;

            GridView1.FooterRow.Cells[0].Attributes["text-align"] = "center";
            GridView1.FooterRow.TableSection = TableRowSection.TableFooter;
            //  ViewState["EmployeeList"] = GridView1.DataSource;

        }

        protected void btnCargar_Click(object sender, EventArgs e)
        {
            CargarRetencion1();
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            //Session["Oficio"] = null;
            Response.Redirect("wfPrincipalDGA.aspx");
        }

        private void CargarRetencion1()
        {

            int Valores = 0;
            int Total = GridView1.Rows.Count;
            List<string> _listaAct = new List<string>();
            CargarArchivosLINQ _cargar = new CargarArchivosLINQ();
            string c_naviera = null;
            int IdDeta = 0;



            try
            {
                if(Convert.ToInt32(hConte.Value) == 0)
                {
                    EnviarCorreo(d_buque.Text, "219", viaje.Text, h_manifiesto.Value + "-" + manif.Text, c_naviera, Total);
                    ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('CEPA - Contenedores <br/>El manifiesto <b>#" + h_manifiesto.Value + "-" + manif.Text + "</b> con <b>" + Total.ToString() + "</b> contenedores, determinaron que no tendría contenedores a escanear ', function () { location.reload(); });", true);
                    
                }
                else if (Convert.ToInt32(hConte.Value) > 0)
                {
                    SeleccionManager.Solicitados((GridView)GridView1);
                    List<DetaNaviera> _lit = HttpContext.Current.Session["Solicitados"] as List<DetaNaviera>;

                    string ofi = i_oficio.Text;

                    if (_lit == null)
                        _lit = new List<DetaNaviera>();


                    if (_lit.Count > 0 && ofi.Length > 0)
                    {
                        foreach (DetaNaviera item in _lit)
                        {

                            DetaNaviera _deta = new DetaNaviera()
                            {
                                IdDeta = item.IdDeta,
                                n_aduana = Convert.ToInt32(i_oficio.Text),
                                us_aduana = User.Identity.Name
                            };

                            int _resultado = Convert.ToInt32(DetaNavieraDAL.upDGA(DBComun.Estado.verdadero, _deta));

                            //string resultado = _proxy.insertContenedorDAN(_Aduana);
                            string resultado = "0";

                            if (_resultado > 0)
                            {
                                Valores = Valores + 1;
                                //_listaAct.Add(item.n_contenedor);
                                IdDeta = item.IdDeta;
                            }



                        }



                        if (IdDeta > 0)
                        {
                            List<DetaNaviera> pNavi = DetaNavieraDAL.ObNavi(IdDeta);

                            if (pNavi == null)
                                pNavi = new List<DetaNaviera>();

                            if (pNavi.Count > 0)
                            {
                                foreach (var item2 in pNavi)
                                {
                                    c_naviera = item2.c_navi;
                                }
                            }
                        }

                        


                        if (Valores > 0)
                        {
                            //ScriptManager.RegisterStartupScript(this, typeof(string), "", "procesar();", true);
                            //Response.Write(string.Format("<script>alert('Cantidad de contenedores retenidos {0}');</script>", Valores));
                            EnviarCorreo(_lit, d_buque.Text, Valores, Total, "219", viaje.Text, h_manifiesto.Value + "-" + manif.Text, c_naviera, ofi);
                            ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('Cantidad de contenedores solicitados " + Valores + "', function () { location.reload(); });", true);

                            Cargar(Convert.ToInt32(IdDocP.Value));
                            i_oficio.Text = DetaNavieraDAL.MaxDocDGA();
                            //ScriptManager.RegisterStartupScript(this, typeof(string), "", "detener();", true);
                        }
                    }
                }

                int pUpDoc = Convert.ToInt32(DetaNavieraDAL.upDocDGA(DBComun.Estado.verdadero, Convert.ToInt32(IdDocP.Value)));
            }
            catch (Exception ex)
            {
                //Response.Write("<script>alert('" + ex.Message + "');</script>");
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ex.Message + "');", true);
            }
            finally
            {
                HttpContext.Current.Session["Solicitados"] = null;
                // Session["Oficio"] = null;
            }
        }

        public void EnviarCorreo(List<DetaNaviera> pContenedores, string d_buque, int pValor, int pCantidad, string c_cliente, string nviaje, string nmani, string c_naviera, string oficio)
        {
            string Html;

            EnvioCorreo _correo = new EnvioCorreo();
            string c_prefijo = string.Empty;
            DateTime _fecha;
            try
            {
                List<UsuarioNaviera> pUser = UsuarioDAL.ObtenerUsuNavi(c_naviera);

                if (pUser == null)
                    pUser = new List<UsuarioNaviera>();


                foreach (var itemU in pUser)
                {
                    c_prefijo = itemU.c_navi_corto;
                    break;
                }


                Html = "<dir style=\"font-family: 'Arial'; font-size: 11px; line-height: 1.2em\">";
                Html += "<b><u> SOLICITUD  DE CONTENEDORES A ESCANEAR - DGA  </b></u><br />";
                Html += "<table style=\"font-family: 'Arial' ; font-size: 11px;  line-height: 1em;\">";
                Html += "<tr>";
                _fecha = DetaNavieraLINQ.FechaBD();
                Html += "<td style=\"text-align: left;\"><font size=2>Fecha/Hora&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:</font></td>";
                Html += "<td style=\"text-align: left;\"><font size = 2>&nbsp&nbsp;" + _fecha.ToString() + "</font></td>";
                Html += "</tr>";
                Html += "<tr>";
                Html += "<td style=\"text-align: left;\" ><font size = 2>Usuario&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:</font></td>";
                Html += "<td style=\"text-align: left;\"><font size = 2>&nbsp;&nbsp;" + User.Identity.Name + "</font></td>";
                Html += "</tr>";
                Html += "</table>";
                Html += "<br />";

                Html += "MÓDULO : LISTADO DE CONTENEDORES SOLICITADOS - DGA <br />";
                Html += "TIPO DE MENSAJE : NOTIFICACIÓN DE CONTENEDORES SOLICITADOS - DGA <br /><br />";
                Html += string.Format("El presente listado de contenedores correspondientes al buque {0},  número de Viaje {1}, manifiesto de ADUANA # {2} y Solicitud # {3} han sido solicitados {4} contenedores correspondientes a este barco.-", d_buque, nviaje, nmani, oficio, pValor);
                Html += "<br /><br/>";


                Html += "Los siguientes contenedores fueron solicitados por la DGA para ser escaneados se detallan a continuación : ";
                Html += "<br /><br/>";

                if (pContenedores == null)
                    pContenedores = new List<DetaNaviera>();

                string valor = null;

                if (pContenedores.Count > 0)
                {
                    Html += "<table style=\"font-family: 'Arial' ; font-size: 12px;  line-height: 1em;width: 100%;border: thin solid #4F81BD; border-collapse: separate; border-spacing:0px; \">";
                    Html += "<tr>";
                    Html += "<center>";
                    Html += "<td width=\"17px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>CONTENEDOR</font></th>";
                    Html += "<td width=\"17px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2># BL</font></th>";
                    Html += "<td width=\"17px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>ESTADO</font></th>";
                    Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>TAMAÑO</font></th>";
                    Html += "<td width=\"25px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>CONSIGNATARIO</font></th>";
                    Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>ESCANER</font></th>";
                    Html += "</center>";
                    Html += "</tr>";

                    foreach (DetaNaviera item in pContenedores)
                    {

                        Html += "<tr>";
                        Html += "<center>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.n_contenedor + "</font></td>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.n_BL + "</font></td>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.b_rdt + "</font></td>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.c_tamaño + "</font></td>";
                        Html += "<td align=\"left\" height=\"35\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.s_consignatario + "</font></td>";
                        Html += "<td height=\"20\"><font size=2 color=blue>" + "SI" + "</font></td>";
                        Html += "<center>";
                        Html += "</tr>";
                        Html += "</font>";
                    }
                    Html += "</table><br /><br/>";
                }

                _correo.Subject = string.Format("DGA : Listado de Contenedores Solicitados - DGA de {0} para el buque {1}, # de Viaje {2}, Manifiesto de Aduana # {3}, Solicitud # {4} ", c_prefijo, d_buque, nviaje, nmani, oficio);
                _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_sol_dga", DBComun.Estado.verdadero, "219");
                List<Notificaciones> _listaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_sol_dga", DBComun.Estado.verdadero, c_cliente);

                if (_listaCC == null)
                    _listaCC = new List<Notificaciones>();

                //_listaCC.AddRange(NotificacionesDAL.ObtenerNotificacionesCCN("b_noti_detenido", DBComun.Estado.verdadero, c_naviera));
                //_listaCC.AddRange(NotificacionesDAL.ObtenerNotificacionesCCN("b_noti_detenido", DBComun.Estado.verdadero, "219"));
                _correo.ListaCC = _listaCC;

                //Notificaciones noti = new Notificaciones
                //{
                //    sMail = "elsa.sosa@cepa.gob.sv",
                //    dMail = "Elsa Sosa"
                //};

                //List<Notificaciones> pLisN = new List<Notificaciones>();

                //pLisN.Add(noti);

                //_correo.ListaNoti = pLisN;


                _correo.Asunto = Html;
                _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.verdadero);
                _correo = null;
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }


        public void EnviarCorreo(string d_buque, string c_cliente, string nviaje, string nmani, string c_naviera, int pTotal)
        {
            string Html;

            EnvioCorreo _correo = new EnvioCorreo();
            string c_prefijo = string.Empty;
            DateTime _fecha;
            try
            {
                List<UsuarioNaviera> pUser = UsuarioDAL.ObtenerUsuNavi(c_naviera);

                if (pUser == null)
                    pUser = new List<UsuarioNaviera>();


                foreach (var itemU in pUser)
                {
                    c_prefijo = itemU.c_navi_corto;
                    break;
                }


                Html = "<dir style=\"font-family: 'Arial'; font-size: 11px; line-height: 1.2em\">";
                Html += "<b><u> SOLICITUD  DE CONTENEDORES A ESCANEAR - DGA  </b></u><br />";
                Html += "<table style=\"font-family: 'Arial' ; font-size: 11px;  line-height: 1em;\">";
                Html += "<tr>";
                _fecha = DetaNavieraLINQ.FechaBD();
                Html += "<td style=\"text-align: left;\"><font size=2>Fecha/Hora&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:</font></td>";
                Html += "<td style=\"text-align: left;\"><font size = 2>&nbsp&nbsp;" + _fecha.ToString() + "</font></td>";
                Html += "</tr>";
                Html += "<tr>";
                Html += "<td style=\"text-align: left;\" ><font size = 2>Usuario&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:</font></td>";
                Html += "<td style=\"text-align: left;\"><font size = 2>&nbsp;&nbsp;" + User.Identity.Name + "</font></td>";
                Html += "</tr>";
                Html += "</table>";
                Html += "<br />";

                Html += "MÓDULO : LISTADO DE CONTENEDORES SOLICITADOS - DGA <br />";
                Html += "TIPO DE MENSAJE : NOTIFICACIÓN DE CONTENEDORES SOLICITADOS - DGA <br /><br />";
                Html += string.Format("Del listado de {0} contenedores correspondientes al buque {1},  número de Viaje {2}, manifiesto de ADUANA # {3} <b><font color=red>NO SE SOLICITARAN</font></b> contenedores para ser escaneados.-",pTotal, d_buque, nviaje, nmani);
                Html += "<br /><br/>";
                                              
                _correo.Subject = string.Format("DGA : Listado de Contenedores Solicitados - DGA de {0} para el buque {1}, # de Viaje {2}, Manifiesto de Aduana # {3}", c_prefijo, d_buque, nviaje, nmani);
                _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_sol_dga", DBComun.Estado.verdadero, "219");
                List<Notificaciones> _listaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_sol_dga", DBComun.Estado.verdadero, c_cliente);

                if (_listaCC == null)
                    _listaCC = new List<Notificaciones>();

                //_listaCC.AddRange(NotificacionesDAL.ObtenerNotificacionesCCN("b_noti_detenido", DBComun.Estado.verdadero, c_naviera));
                //_listaCC.AddRange(NotificacionesDAL.ObtenerNotificacionesCCN("b_noti_detenido", DBComun.Estado.verdadero, "219"));
                _correo.ListaCC = _listaCC;

                //Notificaciones noti = new Notificaciones
                //{
                //    sMail = "elsa.sosa@cepa.gob.sv",
                //    dMail = "Elsa Sosa"
                //};

                //List<Notificaciones> pLisN = new List<Notificaciones>();

                //pLisN.Add(noti);

                //_correo.ListaNoti = pLisN;


                _correo.Asunto = Html;
                _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.verdadero);
                _correo = null;
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }


                
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {


                CheckBox pCheck = (e.Row.FindControl("CheckBox1") as CheckBox);
                HiddenField hCancel = (e.Row.FindControl("hCancelado") as HiddenField);
                HiddenField hAduana = (e.Row.FindControl("hAduanas") as HiddenField);
                


                if (hCancel.Value == "CANCELADO")
                {
                    pCheck.Enabled = false;
                    e.Row.BackColor = Color.FromName("#FFC7CE");
                    e.Row.ForeColor = Color.FromName("#9C0006");    
                }

                if(hAduana.Value == "DGA")
                {
                    pCheck.Enabled = false;
                    e.Row.BackColor = Color.FromName("#FFEB9C");
                    e.Row.ForeColor = Color.FromName("#9C6500");
                }

                if (e.Row.Cells[4].Text == "DAN")
                {
                    e.Row.BackColor = Color.FromName("#C6EFCE");
                    e.Row.ForeColor = Color.FromName("#006100");
                }


                e.Row.Cells[7].Visible = false;
                GridView1.HeaderRow.Cells[7].Visible = false;

                e.Row.Cells[8].Visible = false;
                GridView1.HeaderRow.Cells[8].Visible = false;

               
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

        protected void onPaging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            //Cargar();
        }
       
        
    }
}