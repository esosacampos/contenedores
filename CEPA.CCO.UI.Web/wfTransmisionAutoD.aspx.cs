using CEPA.CCO.DAL;
using CEPA.CCO.Entidades;
using CEPA.CCO.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CEPA.CCO.UI.Web
{
    public partial class wfTransmisionAutoD : System.Web.UI.Page
    {
        //public string c_naviera;
        //private static readonly DateTime FIRST_GOOD_DATE = new DateTime(1900, 01, 01);
        int t_lineas = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {

                    if (Request.QueryString["c_llegada"] == null)
                    {
                        throw new Exception("Falta código de cabecera");
                    }
                    else
                    {
                        List<DocBuque> _encaList = DocBuqueLINQ.ObtenerTransmiAuto(Request.QueryString["c_llegada"].ToString());
                        //Sacar naviera
                        if (_encaList.Count > 0)
                        {
                            foreach (DocBuque item in _encaList)
                            {
                                c_imo.Text = item.c_imo;
                                d_buque.Text = item.d_buque;
                                f_llegada.Text = item.f_llegada.ToString("dd/MM/yyyy HH:mm:ss");
                                c_llegada.Text = item.c_llegada;                               
                                break;
                            }

                            Cargar(c_llegada.Text);
                          
                        }
                        else
                        {

                        }

                    }

                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + "Error: Durante la ejecución recomendamos volverlo intentar, o reportar a Informatica" + "');", true);
                }
            }

        }

        private void RegisterPostBackControl()
        {
           ScriptManager _scriptMan = ScriptManager.GetCurrent(this);
            _scriptMan.AsyncPostBackTimeout = 36000;
        }

        private void Cargar(string c_llegada)
        {
            GridView1.DataSource = DocBuqueLINQ.ObtenerTransmiConsulAuto(c_llegada);
            GridView1.DataBind();

            t_lineas = GridView1.Rows.Count;


            GridView1.HeaderRow.Cells[0].Attributes["data-class"] = "expand";

            // GridView1.HeaderRow.Cells[1].Attributes["data-hide"] = "phone";
            GridView1.HeaderRow.Cells[3].Attributes["data-hide"] = "phone";
            GridView1.HeaderRow.Cells[4].Attributes["data-hide"] = "phone";
            GridView1.HeaderRow.Cells[5].Attributes["data-hide"] = "phone";

            //GridView1.HeaderRow.Cells[8].Attributes["data-hide"] = "phone";

            GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;

            GridView1.FooterRow.Cells[0].Attributes["text-align"] = "center";
            GridView1.FooterRow.TableSection = TableRowSection.TableFooter;
            //  ViewState["EmployeeList"] = GridView1.DataSource;
        }

        protected void btnCargar_Click(object sender, EventArgs e)
        {
            //Thread.Sleep(4000);

            //ClientScript.RegisterStartupScript(GetType(), "proceso", "almacenando();", true);
        
            Guardar();

           
            Cargar(Request.QueryString["c_llegada"].ToString());

            //string mensaje = "prueba";

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

                SeleccionManager.TransmiAuto(GridView1);
                List<DetaNaviera> _lit = HttpContext.Current.Session["Transmi"] as List<DetaNaviera>;


                if (_lit == null)
                {
                    _lit = new List<DetaNaviera>();
                }

                if (_lit.Count > 0)
                {


                    foreach (DetaNaviera item in _lit)
                    {


                        int _resultado = 0;

                        _resultado = Convert.ToInt32(DetaNavieraDAL.ActualizarTransmiAut(DBComun.Estado.verdadero, item.IdDeta, User.Identity.Name));

                        //string resultado = _proxy.insertContenedorDAN(_Aduana);
                        //string resultado = "0";

                        if (_resultado > 0)
                        {
                            Valores = Valores + 1;
                            //_listaAct.Add(string.Concat(item.n_contenedor, "    # Oficio ", item.n_folio));
                            IdDeta = item.IdDeta;
                        }
                    }


                    //var grupoNavi = (from a in _lit
                    //                 group a by a.c_navi into g
                    //                 select new
                    //                 {
                    //                     c_naviera = g.Key
                    //                 }).ToList();




                    if (Valores > 0)
                    {
                      
                        EnviarCorreo(_lit, d_buque.Text);

                        //ClientScript.RegisterStartupScript(GetType(), "confirmacion", "bootbox.alert('Cantidad de contenedores liberados " + Valores + "');", true);
                        ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('Cantidad de contenedores registrados " + Valores + "');", true);

                       
                        Cargar(c_llegada.Text);

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
                HttpContext.Current.Session["Trasmi"] = null;
            }
        }

        public void EnviarCorreo(List<DetaNaviera> pContenedores, string d_buque)
        {
            string Html;

            EnvioCorreo _correo = new EnvioCorreo();
            
            DateTime _fecha;
            try
            {
                //List<UsuarioNaviera> pUser = UsuarioDAL.ObtenerUsuNavi(c_naviera);

                //if (pUser == null)
                //{
                //    pUser = new List<UsuarioNaviera>();
                //}

                Html = "<dir style=\"font-family: 'Arial'; font-size: 11px; line-height: 1.2em\">";
                Html += "<b><u> TRANSMISIÓN AUTOMÁTICA  </b></u><br />";
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



                Html += "MÓDULO : CONTENEDORES SELECCIONADO(S) PARA TRANSMITIR RECEPCION AUTOMATICA A LA DGA  <br />";
                Html += "TIPO DE MENSAJE : NOTIFICACIÓN DE CONTENEDORES SELECCIONADO(S) PARA TRANSMITIR RECEPCION AUTOMATICA A LA DGA <br /><br />";
                Html += string.Format("El presente listado de contenedores correspondientes al buque {0},  han sido seleccionados para transmitir recepción de forma automática.-", d_buque);
                Html += "<br /><br/>";


                Html += "Los siguientes contenedores que se detallan a continuación fueron seleccionados para transmitir recepción automáticamente a la DGA: ";
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
                    Html += "<td width=\"15px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>AGENCIA</font></th>";
                    Html += "<td width=\"15px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>MANIFIESTO</font></th>";
                    Html += "<td width=\"15px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>CONTENEDOR</font></th>";
                    Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>TAMAÑO</font></th>";
                    Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>PESO</font></th>";
                    Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>ESTADO</font></th>";
                    Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>CLIENTE</font></th>";
                    Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>DESPACHO</font></th>";
                    Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>MANEJO</font></th>";
                    Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>TRANSFERENCIA</font></th>";
                    Html += "</center>";
                    Html += "</tr>";
                  
                    foreach (DetaNaviera item in pContenedores)
                    {                        
                        Html += "<tr>";
                        Html += "<center>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.c_cliente + "</font></td>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.n_manifiesto + "</font></td>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.n_contenedor + "</font></td>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.c_tamaño + "</font></td>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + string.Format("{0:0.00}", item.v_peso) + "</font></td>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.b_estado + "</font></td>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.s_consignatario + "</font></td>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.b_despacho + "</font></td>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.b_manejo + "</font></td>";
                        Html += "<td height=\"25\"><font size=2 color=blue>" + item.b_transferencia + "</font></td>";
                        Html += "<center>";
                        Html += "</tr>";
                        Html += "</font>";
                    }
                    Html += "</table><br /><br/>";
                }

                _correo.Subject = string.Format("CEPA : Contenedores para Recepción Automática a la DGA del buque {0}", d_buque);
                _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_trans_auto", DBComun.Estado.verdadero, "0");
                //List<Notificaciones> _listaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_detenido", DBComun.Estado.verdadero, c_naviera);

                //if (_listaCC == null)
                //{
                //    _listaCC = new List<Notificaciones>();
                //}

                //_listaCC.AddRange(NotificacionesDAL.ObtenerNotificacionesCCN("b_noti_detenido", DBComun.Estado.verdadero, c_cliente));
                //_correo.ListaCC = _listaCC;

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
                

                //if (e.Row.Cells[7].Text == "CANCELADO")
                //{
                //    e.Row.BackColor = Color.FromName("#EB7A7A");
                //    e.Row.ForeColor = Color.White;
                //}

                //CheckBox pCheck = (e.Row.FindControl("CheckBox1") as CheckBox);
                //if (e.Row.Cells[7].Text == "CANCELADO")
                //{
                //    pCheck.Enabled = false;
                //}

                //if (ArchivoBookingDAL.isFecha(e.Row.Cells[8].Text) == true)
                //{
                //    if (Convert.ToDateTime(e.Row.Cells[8].Text) > FIRST_GOOD_DATE)
                //    { }
                //    else
                //    {
                //        e.Row.Cells[8].Text = "";
                //    }

                //}


                //Label pfTramite = (e.Row.FindControl("txtDOB") as Label);
                //if (ArchivoBookingDAL.isFecha(pfTramite.Text) == true)
                //{
                //    if (Convert.ToDateTime(pfTramite.Text) > FIRST_GOOD_DATE)
                //    { }
                //    else
                //    {
                //        pfTramite.Text = "";
                //    }

                //}


                //if (ArchivoBookingDAL.isFecha(e.Row.Cells[6].Text) == true)
                //{
                //    if (Convert.ToDateTime(e.Row.Cells[6].Text) > FIRST_GOOD_DATE)
                //    { }
                //    else
                //    {
                //        e.Row.Cells[6].Text = "";
                //    }

                //}

                //e.Row.Cells[7].Visible = false;
                //GridView1.HeaderRow.Cells[7].Visible = false;

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

        protected void GridView1_PreRender(object sender, EventArgs e)
        {
            //Cargar();
        }

        
    }
}