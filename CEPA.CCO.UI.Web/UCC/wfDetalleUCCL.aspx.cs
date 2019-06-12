using CEPA.CCO.DAL;
using CEPA.CCO.Entidades;
using CEPA.CCO.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CEPA.CCO.UI.Web.UCC
{
    public partial class wfDetalleUCCL : System.Web.UI.Page
    {
        public string c_naviera;
        private static readonly DateTime FIRST_GOOD_DATE = new DateTime(1900, 01, 01);



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    DateTime myDateTime = DateTime.Now;
                    string year = myDateTime.Year.ToString();

                    ddlYear.DataSource = TipoRevisionesDAL.Year("UCC");
                    ddlYear.DataTextField = "Years";
                    ddlYear.DataValueField = "IdRevision";
                    ddlYear.DataBind();

                    ddlYear.Items.Insert(0, new ListItem("-- Seleccionar -- "));


                    Cargar("UCC", Convert.ToInt32(year));


                }
                catch (Exception)
                {

                    ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('Este módulo presento problemas, intentarlo de nuevo o consultar a Informatica');", true);
                }
            }

        }

        private void RegisterPostBackControl()
        {
            foreach (GridViewRow row in GridView1.Rows)
            {
                DropDownList lnkFull = row.FindControl("ddlRevision") as DropDownList;
                ScriptManager.GetCurrent(this).RegisterPostBackControl(lnkFull);

                DropDownList lnkFull1 = row.FindControl("ddlDetalle") as DropDownList;
                ScriptManager.GetCurrent(this).RegisterPostBackControl(lnkFull1);
            }

            DropDownList lnkFull2 = ddlYear as DropDownList;
            ScriptManager _scriptMan = ScriptManager.GetCurrent(this);
            _scriptMan.AsyncPostBackTimeout = 36000;
        }

        private void Cargar(string pTipo, int pYear)
        {

            var query = (from a in DetaNavieraDAL.ObtenerDetalleDANL(pTipo, pYear)
                         join b in EncaBuqueDAL.ObtenerShipping(DBComun.Estado.verdadero) on a.c_llegada equals b.c_llegada
                         select new DetaNaviera
                         {
                             IdReg = a.IdReg,
                             IdDeta = a.IdDeta,
                             n_BL = a.n_BL,
                             n_contenedor = a.n_contenedor,
                             c_tamaño = a.c_tamaño,
                             n_folio = a.n_folio,
                             f_retenidoc = a.f_retenidoc,
                             f_recep_patioc = a.f_recep_patioc,
                             c_cliente = a.c_cliente,
                             c_manifiesto = a.c_manifiesto,
                             c_navi = a.c_navi,
                             c_tipo_doc = a.c_tipo_doc,
                             c_llegada = a.c_llegada,
                             d_buque = b.d_buque,
                             f_llegada = a.f_llegada,
                             f_tramite_s = a.f_tramite_s,
                             b_estado = a.b_estado
                         }).OrderByDescending(g => g.IdDeta).ToList();




            GridView1.DataSource = query;
            GridView1.DataBind();

            GridView1.HeaderRow.Cells[0].Attributes["data-class"] = "expand";

            // GridView1.HeaderRow.Cells[1].Attributes["data-hide"] = "phone";
            GridView1.HeaderRow.Cells[3].Attributes["data-hide"] = "phone";
            GridView1.HeaderRow.Cells[4].Attributes["data-hide"] = "phone";
            GridView1.HeaderRow.Cells[5].Attributes["data-hide"] = "phone";
            GridView1.HeaderRow.Cells[6].Attributes["data-hide"] = "phone";

            GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;

            GridView1.FooterRow.Cells[0].Attributes["text-align"] = "center";
            GridView1.FooterRow.TableSection = TableRowSection.TableFooter;
            //  ViewState["EmployeeList"] = GridView1.DataSource;
        }

        protected void btnCargar_Click(object sender, EventArgs e)
        {
            DateTime myDateTime = DateTime.Now;
            string year = myDateTime.Year.ToString();



            Guardar();

            if (ddlYear.SelectedIndex > 0)
            {
                year = ddlYear.SelectedItem.ToString();
            }
            Cargar("UCC", Convert.ToInt32(year));

        }

        private void Guardar()
        {
            int Valores = 0;
            int Total = GridView1.Rows.Count;
            int IdDeta = 0;
            List<string> _listaAct = new List<string>();
            CargarArchivosLINQ _cargar = new CargarArchivosLINQ();


            try
            {

                SeleccionManager.Liberados(GridView1);
                List<DetaNaviera> _lit = HttpContext.Current.Session["Liberados"] as List<DetaNaviera>;


                if (_lit == null)
                {
                    _lit = new List<DetaNaviera>();
                }

                if (_lit.Count > 0)
                {


                    foreach (DetaNaviera item in _lit)
                    {


                        int _resultado = 0;

                        if (item.t_revision > 0 && item.f_revision != null)
                        {
                            _resultado = Convert.ToInt32(DetaNavieraDAL.ActualizarUCCL(DBComun.Estado.verdadero, item.IdDeta, User.Identity.Name, item.f_revision.ToString("dd/MM/yyyy HH:mm"), item.t_revision, item.t_detalle));

                        }
                        else
                        {
                            throw new Exception("Debe indicar fecha de revision y/o tipo de revision");
                        }

                        //string resultado = _proxy.insertContenedorDAN(_Aduana);
                        //string resultado = "0";

                        if (_resultado > 0)
                        {
                            Valores = Valores + 1;
                            //_listaAct.Add(string.Concat(item.n_contenedor, "    # Oficio ", item.n_folio));
                            IdDeta = item.IdDeta;
                        }
                    }


                    var grupoNavi = (from a in _lit
                                     group a by a.c_navi into g
                                     select new
                                     {
                                         c_naviera = g.Key
                                     }).ToList();




                    if (Valores > 0)
                    {
                        foreach (var item in grupoNavi)
                        {
                            List<DetaNaviera> quen = new List<DetaNaviera>();
                            DetaNaviera _detaValores = new DetaNaviera();
                            quen = _lit.Where(a => a.c_navi == item.c_naviera).ToList();

                            foreach (var itemV in quen)
                            {
                                _detaValores.c_tipo_doc = itemV.c_tipo_doc;
                                _detaValores.c_manifiesto = itemV.c_manifiesto;
                                _detaValores.c_navi = itemV.c_navi;
                                _detaValores.c_cliente = itemV.c_cliente;
                                _detaValores.d_buque = itemV.d_buque;
                                break;
                            }

                            EnviarCorreo(quen, _detaValores.d_buque, quen.Count(), Total, "216", _detaValores.c_tipo_doc, _detaValores.c_manifiesto, _detaValores.c_navi, _detaValores.c_cliente);

                        }


                        //ClientScript.RegisterStartupScript(GetType(), "confirmacion", "bootbox.alert('Cantidad de contenedores liberados " + Valores + "');", true);
                        ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('Cantidad de contenedores liberados " + Valores + "');", true);

                        DateTime myDateTime = DateTime.Now;
                        string year = myDateTime.Year.ToString();
                        Cargar("UCC", Convert.ToInt32(year));
                    }
                }
            }
            catch (FormatException ef)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ef.Message + "');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ex.Message + "');", true);
            }
            finally
            {
                HttpContext.Current.Session["Liberados"] = null;
            }
        }

        public void EnviarCorreo(List<DetaNaviera> pContenedores, string d_buque, int pValor, int pCantidad, string c_cliente, string nviaje, string nmani1, string c_naviera, string c_prefijo)
        {
            string Html;

            EnvioCorreo _correo = new EnvioCorreo();
            string nmani = null;
            string[] n_manifs = nmani1.Split('-');
            nmani = n_manifs[1].ToString();
            DateTime _fecha;
            try
            {
                List<UsuarioNaviera> pUser = UsuarioDAL.ObtenerUsuNavi(c_naviera);

                if (pUser == null)
                {
                    pUser = new List<UsuarioNaviera>();
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


                Html += "Los siguientes contenedores fueron liberados por la UCC para su revisión se detallan a continuación : ";
                Html += "<br /><br/>";

                if (pContenedores == null)
                {
                    pContenedores = new List<DetaNaviera>();
                }

                if (pContenedores.Count > 0)
                {
                    Html += "<table style=\"font-family: 'Arial' ; font-size: 12px;  line-height: 1em;width: 100%;border: thin solid #4F81BD; border-collapse: separate; border-spacing:0px; \">";
                    Html += "<tr>";
                    Html += "<center>";
                    Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2># OFICIO</font></th>";
                    Html += "<td width=\"15px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>CONTENEDOR</font></th>";
                    Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>TAMAÑO</font></th>";
                    Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>F. RECEPCION</font></th>";
                    Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>F. RETENCION</font></th>";
                    Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>F. TRAMITACION</font></th>";
                    Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>F. REVISION</font></th>";
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
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.f_recep_patio.ToString("dd/MM/yyyy HH:mm") + "</font></td>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.f_retenido.ToString("dd/MM/yyyy HH:mm") + "</font></td>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.f_tramite.ToString("dd/MM/yyyy HH:mm") + "</font></td>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.f_revision.ToString("dd/MM/yyyy HH:mm") + "</font></td>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.f_dan + "</font></td>";
                        Html += "<td height=\"25\"><font size=2 color=blue>" + Math.Round(item.CalcDias / 24, 2) + "</font></td>";
                        Html += "<center>";
                        Html += "</tr>";
                        Html += "</font>";
                    }
                    Html += "</table><br /><br/>";
                }

                _correo.Subject = string.Format("UCC : Listado de Contenedores Liberados de {0} para el buque {1}, # de Viaje {2}, Manifiesto de Aduana # {3}", c_prefijo, d_buque, nviaje, nmani);
                _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_ucc", DBComun.Estado.verdadero, c_naviera);
                List<Notificaciones> _listaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_ucc", DBComun.Estado.verdadero, c_naviera);

                if (_listaCC == null)
                {
                    _listaCC = new List<Notificaciones>();
                }

                _listaCC.AddRange(NotificacionesDAL.ObtenerNotificacionesCCN("b_noti_ucc", DBComun.Estado.verdadero, c_cliente));
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
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + Ex.Message + "');", true);
            }
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/default.aspx");
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlRevision = (e.Row.FindControl("ddlRevision") as DropDownList);

                ddlRevision.DataSource = TipoRevisionesDAL.Revisiones();
                ddlRevision.DataTextField = "t_revision";
                ddlRevision.DataValueField = "IdRevision";
                ddlRevision.DataBind();

                ddlRevision.Items.Insert(0, new ListItem("-- Seleccionar -- "));

                DropDownList ddlDetalle = (e.Row.FindControl("ddlDetalle") as DropDownList);

                ddlDetalle.DataSource = TipoRevisionesDAL.DetalleLib();
                ddlDetalle.DataTextField = "t_revision";
                ddlDetalle.DataValueField = "IdRevision";
                ddlDetalle.DataBind();

                ddlDetalle.Items.Insert(0, new ListItem("-- Seleccionar -- "));


                //ddlRevision.Attributes.Add("class", "selectpicker seleccion");
                //ddlRevision.Attributes.Add("data-style", "btn-primary");

                ScriptManager current = ScriptManager.GetCurrent(Page);
                if (current != null)
                {
                    current.RegisterAsyncPostBackControl(ddlRevision);
                    current.RegisterAsyncPostBackControl(ddlDetalle);
                }



                if (e.Row.Cells[11].Text == "CANCELADO")
                {
                    e.Row.BackColor = Color.FromName("#EB7A7A");
                    e.Row.ForeColor = Color.White;
                }



                CheckBox pCheck = (e.Row.FindControl("CheckBox1") as CheckBox);
                if (e.Row.Cells[11].Text == "CANCELADO")
                {
                    pCheck.Enabled = false;
                }


                if (ArchivoBookingDAL.isFecha(e.Row.Cells[12].Text) == true)
                {
                    if (Convert.ToDateTime(e.Row.Cells[12].Text) > FIRST_GOOD_DATE)
                    { }
                    else
                    {
                        e.Row.Cells[12].Text = "";
                    }

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
        [WebMethod]
        public static List<TipoRevisiones> LlenarRevision()
        {
            List<TipoRevisiones> pLista = new List<TipoRevisiones>();

            pLista = TipoRevisionesDAL.Revisiones();

            return pLista;
        }

        protected void GridView1_PreRender(object sender, EventArgs e)
        {
            //Cargar();
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime myDateTime = DateTime.Now;
            string year = myDateTime.Year.ToString();


            if (ddlYear.SelectedIndex > 0)
            {
                year = ddlYear.SelectedItem.ToString();
            }

            Cargar("UCC", Convert.ToInt32(year));
        }
    }
}