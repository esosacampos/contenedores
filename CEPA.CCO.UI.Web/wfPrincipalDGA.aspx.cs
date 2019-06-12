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
using System.Web.Services;

namespace CEPA.CCO.UI.Web
{
    public partial class wfPrincipalDGA : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    
                    Cargar();
                    
                }
                catch (Exception Ex)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + Ex.Message + "');", true);
                }
            }     

           
        }

        private void Cargar()
        {
            EncaBuqueBL _encaBL = new EncaBuqueBL();

            GridView1.DataSource = DocBuqueLINQ.ObtenerDGA();
            GridView1.DataBind();

            GridView1.HeaderRow.Cells[0].Attributes["data-class"] = "expand";

            // GridView1.HeaderRow.Cells[1].Attributes["data-hide"] = "phone";
            GridView1.HeaderRow.Cells[2].Attributes["data-hide"] = "phone";
            GridView1.HeaderRow.Cells[3].Attributes["data-hide"] = "phone";
            GridView1.HeaderRow.Cells[4].Attributes["data-hide"] = "phone";
            GridView1.HeaderRow.Cells[5].Attributes["data-hide"] = "phone";
                        
            //GridView1.HeaderRow.Cells[8].Attributes["data-hide"] = "phone";

            GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;

            GridView1.FooterRow.Cells[0].Attributes["text-align"] = "center";
            GridView1.FooterRow.TableSection = TableRowSection.TableFooter;
        }

        public void Timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                Cargar();               
            }   
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //HyperLink hl = (HyperLink)(e.Row.FindControl("Link1"));
                //hl.Attributes.Add("onclick", "Javascript:abrirModal(" + e.Row.Cells[0].Text + ");");

                Label l1 = (Label)(e.Row.FindControl("lblTdga"));

                HiddenField h1 = (HiddenField)(e.Row.FindControl("hEnvia"));

                if (h1.Value == "T")
                {
                    l1.Attributes.Add("class", "badgeV");
                }
                else if (h1.Value == "F"){
                    l1.Attributes.Add("class", "badgeR");
                }
                
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
        public static string EnviarCorreo(string d_buque, string c_cliente, string nviaje, string nmani, string c_naviera, int pTotal, int pIdDoc)
        {
            string Html;

            EnvioCorreo _correo = new EnvioCorreo();
            string c_prefijo = string.Empty;
            DateTime _fecha;
            nmani = nmani.Substring(0, 4) + "-" + nmani.Substring(4, (nmani.Length - 4));
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
                Html += "<td style=\"text-align: left;\"><font size = 2>&nbsp;&nbsp;" + HttpContext.Current.User.Identity.Name + "</font></td>";
                Html += "</tr>";
                Html += "</table>";
                Html += "<br />";

                Html += "MÓDULO : LISTADO DE CONTENEDORES SOLICITADOS - DGA <br />";
                Html += "TIPO DE MENSAJE : NOTIFICACIÓN DE CONTENEDORES SOLICITADOS - DGA <br /><br />";
                Html += string.Format("Del listado de {0} contenedores correspondientes al buque {1},  número de Viaje {2}, manifiesto de ADUANA # {3} <b><font color=red>NO SE SOLICITARAN</font></b> contenedores para ser escaneados.-", pTotal, d_buque, nviaje, nmani);
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

                int pUpDoc = Convert.ToInt32(DetaNavieraDAL.upDocDGA(DBComun.Estado.verdadero, pIdDoc));

                string _mensaje = "CEPA - Contenedores <br/>El manifiesto <b>#" + nmani + "</b> con <b>" + pTotal.ToString() + "</b> contenedores, determinaron que no tendría contenedores a escanear";

                return Newtonsoft.Json.JsonConvert.SerializeObject(_mensaje);

            }
            catch (Exception Ex)
            {
                string _mensaje = "Error: Tiempo de espera agotado, intentelo de nuevo, presione la tecla F5";

                return Newtonsoft.Json.JsonConvert.SerializeObject(_mensaje);
            }
        }
    }
}