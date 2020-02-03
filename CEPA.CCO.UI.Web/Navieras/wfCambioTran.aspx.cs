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
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Data;
using System.Web.Configuration;

namespace CEPA.CCO.UI.Web.Navieras
{
    public partial class wfCambioTran : System.Web.UI.Page
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
            GridView1.DataSource = DetaNavieraDAL.ObtenerDetallesCambio(Convert.ToInt32(Request.QueryString["IdReg"])).OrderBy(a => a.c_correlativo);
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
            Response.Redirect("wfConsultaCambio.aspx");
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {


                CheckBox pCheck = (e.Row.FindControl("CheckBox2") as CheckBox);
                
                if(!e.Row.Cells[2].Text.Contains("HREF"))
                {
                    pCheck.Enabled = false;
                }

            }
        }
        public static string getRetDir(int pIdDeta, int pTipo)
        {
            string _contenedores = "";
            string apiUrl = WebConfigurationManager.AppSettings["apiFox"].ToString();
            Procedure proceso = new Procedure
            {
                NBase = "CONTENEDORES",
                Procedimiento = "Sqlretdir", // "contenedor_exp"; //"Sqlentllenos"; //contenedor_exp('NYKU3806160') //"lstsalidascarga";// ('NYKU3806160')
                Parametros = new List<Parametros>()
            };
            proceso.Parametros.Add(new Parametros { nombre = "_IdDeta", valor = pIdDeta.ToString() });
            proceso.Parametros.Add(new Parametros { nombre = "_pTipo", valor = pTipo.ToString() });
            
            string inputJson = JsonConvert.SerializeObject(proceso);
            apiUrl = apiUrl + inputJson;
            _contenedores = Conectar(_contenedores, apiUrl);
            return _contenedores;
        }

        private static string Conectar(string _contenedores, string apiUrl)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(apiUrl);
            httpWebRequest.Method = WebRequestMethods.Http.Get;
            httpWebRequest.Accept = "application/json; charset=utf-8";
            string file = string.Empty;
            var response = (HttpWebResponse)httpWebRequest.GetResponse();
            //string idx = "{ "DBase":"CONTENEDORES","Servidor":null,"Procedimiento":"Sqlentllenos","Consulta":true,"Parametros":[{"nombre":"_dia","valor":"15-05-2019"}]}";
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                file = sr.ReadToEnd();
                DataTable tabla = JsonConvert.DeserializeObject<DataTable>(file) as DataTable;
                if (tabla.Rows.Count > 0)
                {
                    if (!tabla.Rows[0][0].ToString().StartsWith("ERROR"))
                    {
                        _contenedores = tabla.Rows[0][0].ToString();
                    }
                }
            }
            return _contenedores;
        }
        protected void btnCargar_Click(object sender, EventArgs e)
        {
            CargarArchivosLINQ _cargar = new CargarArchivosLINQ();
            List<string> pListCon = new List<string>();
            try
            {
               
                int cantidad = GridView1.Rows.Count;
                List<int> pListDeta = new List<int>();
                
                string dc, da, bc_cambio = null;

                SeleccionManager.CambiosID((GridView)GridView1);
                List<DetaNaviera> _lit = HttpContext.Current.Session["Cambios"] as List<DetaNaviera>;
                List<DetaNaviera> _litClon = new List<DetaNaviera>();


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



                            }
                            else
                            {


                                throw new Exception("Se notifica que el contenedor" + item.n_contenedor + " no posee justificación de su cambio.");
                            }
                        }
                        else
                        {
                            throw new Exception(string.Format("El contenedor {0} se encuentra solicitado para revisión, proceder con la solicitud a la DAN de su liberación; luego realizar cambio", item.n_contenedor));
                        }

                    }

                    foreach (var item in _lit)
                    {                        

                        dc = item.b_estado == "DESPACHO NORMAL" ? "DN" : "RD";
                        da = item.b_estado == "DESPACHO NORMAL" ? "RD" : "DN";
                        bc_cambio = item.b_estado == "DESPACHO NORMAL" ? "Y" : "";
                        
                                
                        int _resultado = Convert.ToInt32(DetaNavieraDAL.ActualizarCambiosId(DBComun.Estado.verdadero, item.IdDeta, item.s_observaciones, User.Identity.Name, bc_cambio, dc, da, item.c_tamaño, item.b_reef));

                        int pTipo = 0;
                        if (bc_cambio == "")
                        {
                            pTipo = 2;
                        }
                        else
                        {
                            pTipo = 1;
                        }

                        string _reader1 = getRetDir(item.IdDeta, pTipo);

                        if (_resultado > 0)
                        {
                            pListDeta.Add(item.IdDeta);
                        }
                    }

                    if (pListDeta.Count > 0)
                    {


                        EnviarCorreo(d_buque.Text, pListDeta.Count, cantidad, _lit, Session["c_naviera"].ToString());
                        
                        //_cargar.Clear("wfConsultaCancel.aspx");
                        

                        ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('Cantidad de contenedores actualizados " + pListDeta.Count + "');", true);                      

                    }

                  


                }
                else
                    throw new Exception("No se encuentran contenedores seleccionados para cambiar.");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ex.Message + "');", true);
            }
            finally
            {
                HttpContext.Current.Session["Cambios"] = null;               

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
                Html += "MÓDULO : LISTADO DE CONTENEDORES ACTUALIZADOS  <br />";
                Html += "TIPO DE MENSAJE : NOTIFICACIÓN DE CONTENEDORES ACTUALIZADOS <br /><br />";
                Html += string.Format("El presente listado de contenedores correspondientes al barco {0} han sido actualizados {1} de {2} contenedores correspondientes a este barco.-", d_buque, pValor, pCantidad);
                Html += "<br /><br/>";

                if (pLista == null)
                    pLista = new List<DetaNaviera>();



                if (pLista.Count > 0)
                {
                    Html += "<table style=\"font-family: 'Arial' ; font-size: 12px;  line-height: 1em;width: 100%;border: thin solid #4F81BD; border-collapse: separate; border-spacing:0px; \">";
                    Html += "<tr>";
                    Html += "<center>";
                    Html += "<td width=\"5%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>No.</td>";
                    Html += "<td width=\"20%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>CONTENEDOR</font></th>";
                    Html += "<td width=\"10%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>TAMAÑO</font></th>";
                    Html += "<td width=\"15%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>REEF A CONECTAR</font></th>";
                    Html += "<td width=\"30%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>TIPO DESPACHO</font></th>";
                    Html += "<td width=\"35%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>JUSTIFICACIÓN</font></th>";
                    Html += "</center>";
                    Html += "</tr>";   
                    foreach (DetaNaviera item in pLista)
                    {
                        var sEstado = item.b_estado == "DESPACHO NORMAL" ? "DE DESPACHO NORMAL A RETIRO DIRECTO" : "RETIRO DIRECTO A DESPACHO NORMAL";
                        string b_refer = item.c_tamaño.Contains("HREF") ? item.b_reef == "Y" ? "SI" : "NO" : "";
                        Html += "<tr>";
                        Html += "<center>";
                        Html += "<td width=\"5%\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.c_correlativo + "</font></td>";
                        Html += "<td width=\"20%\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.n_contenedor + "</font></td>";
                        Html += "<td width=\"10%\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.c_tamaño + "</font></td>";
                        Html += "<td width=\"15%\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + b_refer  + "</font></td>";
                        Html += "<td width=\"30%\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + sEstado + "</font></td>";
                        Html += "<td width=\"35%\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.s_observaciones + "</font></td>";
                        Html += "</center>";
                        Html += "</tr>";
                        Html += "</font>";  
                    }
                }
                Html += "</table><br /><br/>";
                Html += "<br />";
                Html += "<br />";
               
                _correo.Subject = string.Format("CAMBIOS: Listado de Contenedores Actualizado de {0} para el buque {1}, # de Viaje {2}", c_prefijo.Value, d_buque, c_viaje.Value);
                _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_cambios", DBComun.Estado.verdadero, c_cliente);

                List<Notificaciones> _listaCC = new List<Notificaciones>();

                if (c_cliente != "11" && c_cliente != "216")
                    _listaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_cambios", DBComun.Estado.verdadero, c_cliente);

                if (_listaCC == null)
                    _listaCC = new List<Notificaciones>();

                _listaCC.AddRange(NotificacionesDAL.ObtenerNotificacionesCCN("b_noti_cambios", DBComun.Estado.verdadero, "219"));
                _listaCC.AddRange(NotificacionesDAL.ObtenerNotificaciones("b_noti_cambios", DBComun.Estado.verdadero, "17"));
                _correo.ListaCC = _listaCC;


                //_correo.ListaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_cancela", DBComun.Estado.verdadero, c_cliente);

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