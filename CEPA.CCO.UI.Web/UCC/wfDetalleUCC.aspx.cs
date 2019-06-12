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

namespace CEPA.CCO.UI.Web.UCC
{
    public partial class wfDetalleUCC : System.Web.UI.Page
    {
        public string c_naviera;
        private static readonly DateTime FIRST_GOOD_DATE = new DateTime(1900, 01, 01);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {

                    if (Request.QueryString["IdReg"] == null)
                    {
                        throw new Exception("Falta código de cabecera o Número de Oficio");
                    }
                    else
                    {
                        string _nOficio = DetaNavieraDAL.MaxUCC();

                        List<DocBuque> _encaList = DocBuqueLINQ.ObtenerDANIdR(Convert.ToInt32(Request.QueryString["IdReg"]));
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
                                IdRegP.Value = Request.QueryString["IdReg"].ToString();
                                h_manifiesto.Value = item.a_manifiesto.Trim().TrimEnd().TrimStart();
                                i_oficio.Text = _nOficio;

                            }

                            Cargar();
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

        private void Cargar()
        {
            List<DetaNaviera> pLista = DetaNavieraDAL.ObtenerDetalleDAN(Convert.ToInt32(Request.QueryString["IdReg"]), "UCC").OrderBy(y => y.c_correlativo).ToList();

            GridView1.DataSource = pLista;
            GridView1.DataBind();

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

        protected void btnCargar_Click(object sender, EventArgs e)
        {
            CargarRetencion1();
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

                SeleccionManager.Detenidos((GridView)GridView1);
                List<DetaNaviera> _lit = HttpContext.Current.Session["Detenidos"] as List<DetaNaviera>;

                string ofi = i_oficio.Text;

                if (_lit == null)
                    _lit = new List<DetaNaviera>();


                if (_lit.Count > 0 && ofi.Length > 0)
                {
                    foreach (DetaNaviera item in _lit)
                    {
                        if (item.ClaveRe != "-- Seleccionar Tipo --")
                        {



                            int _resultado = Convert.ToInt32(DetaNavieraDAL.ActualizarUCC(DBComun.Estado.verdadero, item.IdDeta, item.ClaveRe, ofi, User.Identity.Name, item.b_escaner));

                            //string resultado = _proxy.insertContenedorDAN(_Aduana);
                            string resultado = "0";



                            if (_resultado > 0)
                            {
                                Valores = Valores + 1;
                                //_listaAct.Add(item.n_contenedor);
                                IdDeta = item.IdDeta;
                            }



                        }
                        else
                            throw new Exception(string.Format("Debe seleccionar el tipo de revisión error en contenedor #{0}", item.n_contenedor));
                        //ScriptManager.RegisterClientScriptBlock(btnCargar, typeof(Button), "Popup", string.Format("alert('Debe seleccionar el tipo de revisión error en contenedor #{0}');", item.n_contenedor), true);

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
                        EnviarCorreo(_lit, d_buque.Text, Valores, Total, "216", viaje.Text, manif.Text, c_naviera, ofi);
                        ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('Cantidad de contenedores retenidos " + Valores + "');", true);
                        Cargar();
                        //ScriptManager.RegisterStartupScript(this, typeof(string), "", "detener();", true);
                    }
                }
            }
            catch (Exception ex)
            {
                //Response.Write("<script>alert('" + ex.Message + "');</script>");
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ex.Message + "');", true);
            }
            finally
            {
                HttpContext.Current.Session["Detenidos"] = null;
                // Session["Oficio"] = null;
            }
        }

        [WebMethod]
        public static int CargarRetencion(string GridView1)
        {
            //wfDetalleDAN _pDetalle = new wfDetalleDAN();

            List<DetaNaviera> myDeserializedObjList = (List<DetaNaviera>)JsonConvert.DeserializeObject(GridView1);




            int Total = myDeserializedObjList.Count;
            List<string> _listaAct = new List<string>();
            CargarArchivosLINQ _cargar = new CargarArchivosLINQ();

            return 0;


            //try
            {

                //    SeleccionManager.Detenidos(GridView1 as GridView);
                //    List<DetaNaviera> _lit = HttpContext.Current.Session["Detenidos"] as List<DetaNaviera>;

                //    string ofi = h_oficio.Value;

                //    if (_lit == null)
                //        _lit = new List<DetaNaviera>();


                //    if (_lit.Count > 0 && ofi.Length > 0)
                //    {
                //        foreach (DetaNaviera item in _lit)
                //        {
                //            if (item.ClaveRe != "-- Seleccionar Tipo --")
                //            {
                //                int _resultado = Convert.ToInt32(DetaNavieraDAL.ActualizarDAN(DBComun.Estado.verdadero, item.IdDeta, item.ClaveRe, ofi, HttpContext.Current.Session["c_usuario"].ToString(), item.b_escaner));

                //                if (_resultado > 0)
                //                {
                //                    Valores = Valores + 1;
                //                    //_listaAct.Add(item.n_contenedor);
                //                    IdDeta = item.IdDeta;
                //                }
                //            }
                //            else
                //                throw new Exception(string.Format("Debe seleccionar el tipo de revisión error en contenedor #{0}", item.n_contenedor));
                //            //ScriptManager.RegisterClientScriptBlock(btnCargar, typeof(Button), "Popup", string.Format("alert('Debe seleccionar el tipo de revisión error en contenedor #{0}');", item.n_contenedor), true);

                //        }

                //        if (IdDeta > 0)
                //        {
                //            List<DetaNaviera> pNavi = DetaNavieraDAL.ObNavi(IdDeta);

                //            if (pNavi == null)
                //                pNavi = new List<DetaNaviera>();

                //            if (pNavi.Count > 0)
                //            {
                //                foreach (var item2 in pNavi)
                //                {
                //                    c_naviera = item2.c_navi;
                //                }
                //            }
                //        }

                //        if (Valores > 0)
                //        {
                //            //ScriptManager.RegisterStartupScript(this, typeof(string), "", "procesar();", true);
                //            //Response.Write(string.Format("<script>alert('Cantidad de contenedores retenidos {0}');</script>", Valores));
                //            EnviarCorreo(_lit, d_buque.Text, Valores, Total, "216", viaje.Text, manif.Text, c_naviera, ofi);
                //            return Valores;
                //            //ScriptManager.RegisterStartupScript(this, typeof(string), "", "detener();", true);
                //        }
                //        return 0;
                //    }

                //    return 0;
                //}
                //catch (Exception ex)
                //{
                //    //Response.Write("<script>alert('" + ex.Message + "');</script>");
                //    return 0;
                //}
                //finally
                //{
                //    HttpContext.Current.Session["Detenidos"] = null;
                //    // Session["Oficio"] = null;                
            }
        }

        //public void EnviarCorreo(List<string> pContenedores, string d_buque, int pValor, int pCantidad, string c_cliente, string nviaje, string nmani, string c_naviera)
        //{
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
                Html += string.Format("El presente listado de contenedores correspondientes al buque {0},  número de Viaje {1}, manifiesto de ADUANA # {2} y Oficio # {3} han sido retenidos {4} contenedores correspondientes a este barco.-", d_buque, nviaje, nmani, oficio, pValor);
                Html += "<br /><br/>";


                Html += "Los siguientes contenedores fueron retenidos por la UCC para su revisión se detallan a continuación : ";
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
                    Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>ESTADO</font></th>";
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
                        if (item.b_rdt.Contains("VACIO"))
                            Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=red>" + item.b_rdt + "</font></td>";
                        else
                            Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.b_rdt + "</font></td>";
                        Html += "<td align=\"left\" height=\"35\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.s_consignatario + "</font></td>";
                        Html += "<td height=\"20\"><font size=2 color=blue>" + valor + "</font></td>";
                        Html += "<center>";
                        Html += "</tr>";
                        Html += "</font>";
                    }
                    Html += "</table><br /><br/>";
                }

                Html += "Los contenedores que en su estado posean /RD indican Retiro Directo, y /T Trasbordo ";

                _correo.Subject = string.Format("UCC : Listado de Contenedores Retenidos de {0} para el buque {1}, # de Viaje {2}, Manifiesto de Aduana # {3}, Oficio # {4} ", c_prefijo, d_buque, nviaje, nmani, oficio);
                _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_ucc", DBComun.Estado.verdadero, c_naviera);
                List<Notificaciones> _listaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_ucc", DBComun.Estado.verdadero, c_cliente);

                if (_listaCC == null)
                    _listaCC = new List<Notificaciones>();

                _listaCC.AddRange(NotificacionesDAL.ObtenerNotificacionesCCN("b_noti_ucc", DBComun.Estado.verdadero, c_naviera));
                _listaCC.AddRange(NotificacionesDAL.ObtenerNotificacionesCCN("b_noti_ucc", DBComun.Estado.verdadero, "219"));
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
            //Session["Oficio"] = null;
            Response.Redirect("wfPrincipalUCC.aspx");
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

                HiddenField _bloqueo = (e.Row.FindControl("_hb_bloqueo") as HiddenField);


                if (_bloqueo.Value == "DAN")
                {
                    CheckBox pCheck = (e.Row.FindControl("CheckBox1") as CheckBox);
                    CheckBox pCheck2 = (e.Row.FindControl("CheckBox2") as CheckBox);

                    pCheck.Enabled = false;
                    pCheck2.Enabled = false;
                    ddlRevision.Enabled = false;

                    e.Row.BackColor = Color.FromName("#fcf8e3");
                    e.Row.ForeColor = Color.FromName("#8a6d3b");  


                }

                if (ArchivoBookingDAL.isFecha(e.Row.Cells[6].Text) == true)
                {
                    if (Convert.ToDateTime(e.Row.Cells[6].Text) > FIRST_GOOD_DATE)
                    { }
                    else
                    {
                        e.Row.Cells[6].Text = "";
                    }

                }

                e.Row.Cells[10].Visible = false;
                GridView1.HeaderRow.Cells[10].Visible = false;

                e.Row.Cells[11].Visible = false;
                GridView1.HeaderRow.Cells[11].Visible = false;


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
            Cargar();
        }


        [WebMethod]
        public static string RetenValid(int pIdDeta)
        {
            string resultado = "";

            resultado = DetaDocDAL.RetenValidacion(pIdDeta);


            return resultado;
        }
    }
}