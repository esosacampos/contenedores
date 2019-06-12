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

namespace CEPA.CCO.UI.Web.DAN
{
    public partial class wfDetalleDANL : System.Web.UI.Page
    {
        public string c_naviera;

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
                        List<DocBuque> _encaList = DocBuqueLINQ.ObtenerDANId(Convert.ToInt32(Request.QueryString["IdReg"]));
                        //Sacar naviera
                        if (_encaList.Count > 0)
                        {
                            foreach (DocBuque item in _encaList)
                            {
                                c_imo.Text = item.c_imo;
                                d_buque.Text = item.d_buque;
                                f_llegada.Text = item.f_llegada.ToString("dd/MM/yyyy HH:mm:ss");
                                c_llegada.Text = item.c_llegada;
                                viaje.Text = item.c_voyage;
                                manif.Text = item.num_manif.ToString();
                                c_naviera = item.c_cliente;
                            }

                            Cargar();
                        }
                        else
                        {
                            btnCargar.Enabled = false;
                        }

                    }

                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
            }
        }

        private void Cargar()
        {
            GridView1.DataSource = DetaNavieraDAL.ObtenerDetalleDANL(Convert.ToInt32(Request.QueryString["IdReg"])).OrderBy(y => y.n_contenedor).ToList();
            GridView1.DataBind();
        }

        protected void btnCargar_Click(object sender, EventArgs e)
        {
            int Valores = 0;
            int Total = GridView1.Rows.Count;
            int IdDeta = 0;
            string c_navi = null;
            List<string> _listaAct = new List<string>();
            CargarArchivosLINQ _cargar = new CargarArchivosLINQ();
            try
            {
                SeleccionManager.Liberados((GridView)GridView1);
                List<DetaNaviera> _lit = HttpContext.Current.Session["Liberados"] as List<DetaNaviera>;


                if (_lit == null)
                    _lit = new List<DetaNaviera>();


                if (_lit.Count > 0)
                {
                    foreach (DetaNaviera item in _lit)
                    {
                        int _resultado = Convert.ToInt32(DetaNavieraDAL.ActualizarDANL(DBComun.Estado.verdadero, item.IdDeta, item.f_liberado.ToString("dd/MM/yyyy HH:mm"), User.Identity.Name));

                        if (_resultado > 0)
                        {
                            Valores = Valores + 1;
                            //_listaAct.Add(string.Concat(item.n_contenedor, "    # Oficio ", item.n_folio));
                            IdDeta = item.IdDeta;
                        }
                    }

                    if (IdDeta > 0)
                    {
                        List<DetaNaviera> pNavi = DetaNavieraDAL.ObNavi(IdDeta);

                        if (pNavi == null)
                            pNavi = new List<DetaNaviera>();


                        foreach (var item2 in pNavi)
                        {
                            c_navi = item2.c_navi;
                        }

                    }

                    if (Valores > 0)
                    {
                        _cargar.Imprimir();
                        Response.Write(string.Format("<script>alert('Cantidad de contenedores liberados {0}');</script>", Valores));
                        EnviarCorreo(_lit, d_buque.Text, Valores, Total, "11", viaje.Text, manif.Text, c_navi);
                        Thread.Sleep(1000);
                        _cargar.Clear("wfPrincipalDANL.aspx");
                    }
                }
            }
            catch (FormatException ef)
            {
                _cargar.Imprimir();
                Response.Write("<script>alert('" + "Debe indicar la fecha de inicio de tramite" + "');</script>");
                Thread.Sleep(1000);
                _cargar.Clear("wfDetalleDANL.aspx?IdReg=" + Convert.ToInt32(Request.QueryString["IdReg"]));
            }
            catch (Exception ex)
            {
                _cargar.Imprimir();
                Response.Write("<script>alert('" + ex.Message + "');</script>");
                Thread.Sleep(1000);
                _cargar.Clear("wfDetalleDANL.aspx?IdReg=" + Convert.ToInt32(Request.QueryString["IdReg"]));
            }         
            finally
            {
                HttpContext.Current.Session["Liberados"] = null;
            }
        }

        public void EnviarCorreo(List<DetaNaviera> pContenedores, string d_buque, int pValor, int pCantidad, string c_cliente, string nviaje, string nmani, string c_naviera)
        {
            string Html;
            int i = 1;
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
                Html += "<b><u> LIBERACION  DE CONTENEDORES  </b></u><br />";
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

               

                Html += "MÓDULO : LISTADO DE CONTENEDORES LIBERADOS  <br />";
                Html += "TIPO DE MENSAJE : NOTIFICACIÓN DE CONTENEDORES LIBERADOS <br /><br />";
                Html += string.Format("El presente listado de contenedores correspondientes al buque {0},  número de Viaje {1} y manifiesto de ADUANA # {2}, han sido liberados {3} contenedores correspondientes a este barco.-", d_buque, nviaje, nmani, pValor);
                Html += "<br /><br/>";


                Html += "Los siguientes contenedores fueron liberados por la DAN para su revisión se detallan a continuación : ";
                Html += "<br /><br/>";

                if (pContenedores == null)
                    pContenedores = new List<DetaNaviera>();

                if (pContenedores.Count > 0)
                { 
                    Html += "<table style=\"font-family: 'Arial' ; font-size: 12px;  line-height: 1em;width: 100%;border: thin solid #4F81BD; border-collapse: separate; border-spacing:0px; \">";
                    Html += "<tr>";
                    Html += "<center>";
                    Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2># OFICIO</font></th>";
                    Html += "<td width=\"15px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>CONTENEDOR</font></th>";
                    Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>TAMAÑO</font></th>";
                    Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>F. RETENCION</font></th>";
                    Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>F. TRAMITACION</font></th>";
                    Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>F. LIBERACION</font></th>";
                    Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>TIEMPO (Días)</font></th>";
                    Html += "</center>";
                    Html += "</tr>";                                 
                    
                    foreach (DetaNaviera item in pContenedores)
                    {
                        Html += "<tr>";
                        Html += "<center>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.n_folio + "</font></td>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.n_contenedor + "</font></td>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.c_tamaño + "</font></td>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.f_recepcion.ToString("dd/MM/yyyy HH:mm") + "</font></td>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.f_liberado.ToString("dd/MM/yyyy HH:mm") + "</font></td>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.f_dan + "</font></td>";                        
                        Html += "<td height=\"25\"><font size=2 color=blue>" + Math.Round(item.CalcDias / 24, 2) + "</font></td>";
                        Html += "<center>";
                        Html += "</tr>";
                        Html += "</font>";                       
                    }
                    Html += "</table><br /><br/>";
                }

                _correo.Subject = string.Format("DAN : Listado de Contenedores Liberados de {0} para el buque {1}, # de Viaje {2}, Manifiesto de Aduana # {3}", c_prefijo, d_buque, nviaje, nmani);
                _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_detenido", DBComun.Estado.verdadero, c_naviera);
                List<Notificaciones> _listaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_detenido", DBComun.Estado.verdadero, c_naviera);

                if (_listaCC == null)
                    _listaCC = new List<Notificaciones>();

                _listaCC.AddRange(NotificacionesDAL.ObtenerNotificacionesCCN("b_noti_detenido", DBComun.Estado.verdadero, c_cliente));
                _correo.ListaCC = _listaCC;

                _correo.Asunto = Html;
                _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.verdadero);
                _correo = null;
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("wfPrincipalDANL.aspx");
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlRevision = (e.Row.FindControl("ddlRevision") as DropDownList);

                ddlRevision.DataSource = TipoSolicitudDAL.ObtenerAgentes();
                ddlRevision.DataTextField = "d_agente";
                ddlRevision.DataValueField = "n_agente";
                ddlRevision.DataBind();

                ddlRevision.Items.Insert(0, new ListItem("-- Seleccionar Agente -- "));
            }
        }

        protected void GridView1_RowDataBound1(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                
                e.Row.Cells[5].Visible = false;                
                GridView1.HeaderRow.Cells[5].Visible = false;
                
            }
        }

    }
}