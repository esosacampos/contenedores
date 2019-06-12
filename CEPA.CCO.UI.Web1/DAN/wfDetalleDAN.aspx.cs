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
    public partial class wfDetalleDAN : System.Web.UI.Page
    {
        public string c_naviera;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    n_oficio.Attributes.Add("onclick", "Javascript:abrirModal('wfAddFolio.aspx?IdReg=" + Request.QueryString["IdReg"].ToString() + "'," + Request.QueryString["IdReg"].ToString() + ")");
                    if (Request.QueryString["IdReg"] == null && Session["Oficio"] == null)
                    {
                        throw new Exception("Falta código de cabecera o Número de Oficio");
                    }
                    else
                    {
                        List<DocBuque> _encaList = DocBuqueLINQ.ObtenerDANIdR(Convert.ToInt32(Request.QueryString["IdReg"]));
                        //Sacar naviera
                        if (_encaList.Count > 0 && Session["Oficio"].ToString().Length >0 )
                        {
                            foreach (DocBuque item in _encaList)
                            {
                                n_oficio.Text = Session["Oficio"].ToString();
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
            GridView1.DataSource = DetaNavieraDAL.ObtenerDetalleDAN(Convert.ToInt32(Request.QueryString["IdReg"])).OrderBy(y => y.c_correlativo).ToList();
            GridView1.DataBind();          
        }

        protected void btnCargar_Click(object sender, EventArgs e)
        {
            int Valores = 0;
            int Total = GridView1.Rows.Count;
            List<string> _listaAct = new List<string>();
            CargarArchivosLINQ _cargar = new CargarArchivosLINQ();
            string c_naviera = null;
            int IdDeta = 0;
            
            try
            {
                SeleccionManager.Detenidos((GridView)GridView1);
                List<DetaNaviera> _lit = HttpContext.Current.Session["Detenidos"] as List<DetaNaviera>;


                if (_lit == null)
                    _lit = new List<DetaNaviera>();


                if (_lit.Count > 0 && Session["Oficio"].ToString().Length > 0)
                {
                    foreach (DetaNaviera item in _lit)
                    {
                        if (item.ClaveRe != "-- Seleccionar Tipo --" )
                        {
                            int _resultado = Convert.ToInt32(DetaNavieraDAL.ActualizarDAN(DBComun.Estado.verdadero, item.IdDeta, item.ClaveRe, Session["Oficio"].ToString(), User.Identity.Name, item.b_escaner));

                            if (_resultado > 0)
                            {
                                Valores = Valores + 1;
                                //_listaAct.Add(item.n_contenedor);
                                IdDeta = item.IdDeta;
                            }
                        }
                        else
                            //throw new Exception(string.Format("Debe seleccionar el tipo de revisión error en contenedor #{0}", item.n_contenedor));
                            ScriptManager.RegisterClientScriptBlock(btnCargar, typeof(Button), "Popup", string.Format("alert('Debe seleccionar el tipo de revisión error en contenedor #{0}');", item.n_contenedor), true);

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
                        _cargar.Imprimir();
                        Response.Write(string.Format("<script>alert('Cantidad de contenedores retenidos {0}');</script>", Valores));
                        EnviarCorreo(_lit, d_buque.Text, Valores, Total, "11", viaje.Text, manif.Text, c_naviera);
                        Session["Oficio"] = null;
                        Thread.Sleep(1000);
                        _cargar.Clear("wfPrincipalDAN.aspx");
                    }
                }
            }
            catch (Exception ex)
            {               
                Response.Write("<script>alert('" + ex.Message + "');</script>");               
            }
            finally
            {
                HttpContext.Current.Session["Detenidos"] = null;
               // Session["Oficio"] = null;
            }
        }

        //public void EnviarCorreo(List<string> pContenedores, string d_buque, int pValor, int pCantidad, string c_cliente, string nviaje, string nmani, string c_naviera)
        //{
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
                Html += "<b><u> RETENCIÓN  DE CONTENEDORES  </b></u><br />";
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
                
                Html += "MÓDULO : LISTADO DE CONTENEDORES RETENIDOS  <br />";
                Html += "TIPO DE MENSAJE : NOTIFICACIÓN DE CONTENEDORES RETENIDOS <br /><br />";
                Html += string.Format("El presente listado de contenedores correspondientes al buque {0},  número de Viaje {1}, manifiesto de ADUANA # {2} y Oficio # {3} han sido retenidos {4} contenedores correspondientes a este barco.-", d_buque, nviaje, nmani, Session["Oficio"].ToString(), pValor);
                Html += "<br /><br/>";

                               
                //if (pContenedores.Count > 0)
                //{
                //    Html += "Los siguientes contenedores fueron retenidos por la DAN para su revisión se detallan a continuación : ";
                //    Html += "<OL>";
                //    foreach (string item in pContenedores)
                //    {
                //        Html += "<LI>" + item;
                //    }
                //    Html += "</OL>";
                //}


                Html += "Los siguientes contenedores fueron retenidos por la DAN para su revisión se detallan a continuación : ";
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
                    Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>TAMAÑO</font></th>";
                    Html += "<td width=\"25px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>CONSIGNATARIO</font></th>";
                    Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>ESCANER</font></th>";
                    Html += "</center>";
                    Html += "</tr>";

                    foreach (DetaNaviera item in pContenedores)
                    {

                        if (item.b_escaner == 1)
                            valor = "SI";
                        else
                            valor = "";


                        Html += "<tr>";
                        Html += "<center>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.n_contenedor + "</font></td>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.n_BL + "</font></td>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.c_tamaño + "</font></td>";
                        Html += "<td align=\"left\" height=\"35\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.s_consignatario + "</font></td>";
                        Html += "<td height=\"20\"><font size=2 color=blue>" + valor + "</font></td>";
                        Html += "<center>";
                        Html += "</tr>";
                        Html += "</font>";
                    }
                    Html += "</table><br /><br/>";
                }

                _correo.Subject = string.Format("DAN : Listado de Contenedores Retenidos de {0} para el buque {1}, # de Viaje {2}, Manifiesto de Aduana # {3}, Oficio # {4} ", c_prefijo, d_buque, nviaje, nmani, Session["Oficio"].ToString());
                _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_detenido", DBComun.Estado.verdadero, c_naviera);
                List<Notificaciones> _listaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_detenido", DBComun.Estado.verdadero, c_cliente);

                if (_listaCC == null)
                    _listaCC = new List<Notificaciones>();

                _listaCC.AddRange(NotificacionesDAL.ObtenerNotificacionesCCN("b_noti_detenido", DBComun.Estado.verdadero, c_naviera));
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

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Session["Oficio"] = null;
            Response.Redirect("wfPrincipalDAN.aspx");
        }

        protected void n_oficio_Click(object sender, EventArgs e)
        {
           // ScriptManager.RegisterClientScriptBlock(n_oficio, typeof(Button), "Popup", "Javascript:abrirModal('wfAddFolio.aspx?IdReg=" + Request.QueryString["IdReg"].ToString() + "'," + Request.QueryString["IdReg"].ToString() + ")", true);
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlRevision = (e.Row.FindControl("ddlRevision") as DropDownList);

                ddlRevision.DataSource = TipoSolicitudDAL.ObtenerRevision();
                ddlRevision.DataTextField = "Tipo";
                ddlRevision.DataValueField = "Clave";
                ddlRevision.DataBind();

                ddlRevision.Items.Insert(0, new ListItem("-- Seleccionar Tipo --"));

                e.Row.Cells[9].Visible = false;
                GridView1.HeaderRow.Cells[9].Visible = false;

                e.Row.Cells[10].Visible = false;
                GridView1.HeaderRow.Cells[10].Visible = false;

               
            }
        }

       

    }
}