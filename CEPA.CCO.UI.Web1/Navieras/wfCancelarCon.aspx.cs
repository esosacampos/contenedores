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
                            }

                            Cargar();
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
            GridView1.DataSource = DetaNavieraDAL.ObtenerDetalleCancel(Convert.ToInt32(Request.QueryString["IdReg"]));
            GridView1.DataBind();
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
                int valores = 0;
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
                        if (item.s_observaciones != null && item.s_observaciones != "")
                        {
                            int _resultado = Convert.ToInt32(DetaNavieraDAL.ActualizarCancelarId(DBComun.Estado.verdadero, item.IdDeta, item.s_observaciones));

                            if (_resultado > 0)
                            {
                                pListDeta.Add(item.IdDeta);
                            }
                        }
                        else
                        {
                            pListCon.Add(item.n_contenedor);
                        }

                    }

                    if (pListDeta.Count > 0)
                    {
                        
                        _cargar.Imprimir();
                        Response.Write(string.Format("<script>alert('Cantidad de contenedores cancelados {0} de {1}');</script>", pListDeta.Count, cantidad));
                        List<DetaNaviera> lista = new List<DetaNaviera>();

                        foreach (int item in pListDeta)
	                    {
                            var pLista = DetaNavieraDAL.ObtenerCancel(Convert.ToInt32(Request.QueryString["IdReg"].ToString()), item);

                            if (pLista.Count > 0)
                            {
                                lista.AddRange(pLista);
                            }
	                    }                        

                        EnviarCorreo(d_buque.Text, lista.Count, cantidad, lista, Session["c_naviera"].ToString(), pListCon);
                        Thread.Sleep(1000);
                        _cargar.Clear("wfConsultaCancel.aspx");
                    }
                }
            }
            catch (Exception ex)
            {
                _cargar.Imprimir();
                Response.Write("<script>alert('" + ex.Message + "');</script>");
                Thread.Sleep(4000);
                _cargar.Clear(string.Format("wfCancelarCon.aspx?IdReg={0}", Request.QueryString["IdReg"].ToString()));
            }
            finally
            {                
                HttpContext.Current.Session["Cancelados"] = null;
            }
        }

        public void EnviarCorreo(string d_buque, int pValor, int pCantidad, List<DetaNaviera> pLista, string c_cliente, List<string> pList)
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
                    Html += "<table style=\"font-family: 'Arial' ; font-size: 12px;  line-height: 1em;\">";
                    Html += "<tr>";                           
                    foreach (DetaNaviera item in pLista)
                    {
                        Html += "<td width=\"6%\" style=\"text-align: left;\"><font size=2>" + i + "</font></td>";
                        Html += "<td width=\"10%\" style=\"text-align: left;\"><font size=2>" + item.n_contenedor + "</font></td>";
                        Html += "<td width=\"10%\" style=\"text-align: left;\"><font size = 2>" + item.c_tamaño + "</font></td>";
                        Html += "<td width=\"35%\" style=\"text-align: left;\"><font size = 2>" + item.s_observaciones + "</font></td>";                   
                        Html += "</tr>";
                        i = i + 1;
                    }                    
                }
                Html += "</td></tr></font></table>";                            
                Html += "<br />";
                Html += "<br />";
                if (pList.Count > 0)
                {
                    Html += "El siguiente listado de contenedores no pudo ser cancelados debido a que no presentaron justificación de cancelación :";
                    Html += "<OL>";
                    foreach (string item2 in pList)
                    {
                        Html += "<LI>" + item2;
                    }                
                }
                Html += "</OL>";
                Html += "<br />";
                Html += "<br />";

                _correo.Subject = string.Format("Listado de Contenedores Cancelados {0}", d_buque);                
                _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_cancela", DBComun.Estado.verdadero, c_cliente);
                _correo.ListaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_cancela", DBComun.Estado.verdadero, c_cliente);
                _correo.Asunto = Html;
                _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.verdadero);
                _correo = null;
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
        }

      


    }
}