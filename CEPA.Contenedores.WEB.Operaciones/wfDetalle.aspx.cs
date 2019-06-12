using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CEPA.CCO.Entidades;
using CEPA.CCO.DAL;
using CEPA.CCO.Linq;
using System.Text;
using System.Web.Services;
using System.Threading;

namespace CEPA.CCO.WEB.Operaciones
{
    public partial class wfDetalle : System.Web.UI.Page
    {
        public string c_tamaño; 

        protected void Page_Load(object sender, EventArgs e)
        {
            txtBusqueda.Attributes.Add("onkeypress", "return validarEnter(event,'" + btnBuscar.ClientID + "')");
            //txtSize.Attributes.Add("onkeypress", "return validarEnter(event,'" + btnEjecutar.ClientID + "')");
            SetContextKey();         
            

            if (!IsPostBack)
            {
                Label23.Text = Session["d_buque"].ToString();
                Label4.Text = Session["grupo"].ToString();
                Label5.Text = Session["grua"].ToString();

                CargarM();
            }
            
        }

        private void CargarM()
        {
            GridView1.DataSource = DetaNavieraDAL.ObtenerTop(Convert.ToInt32(Session["IdRegistro"]));
            GridView1.DataBind();
        }

       [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
        public static string[] ObtenerContenedores(string prefixText, int count, string contextKey)
        {  

            int pId = Convert.ToInt32(contextKey);

            var query = from a in DetaNavieraDAL.ObtenerBusqueda(pId)
                        where a.n_contenedor.StartsWith(prefixText.ToUpper())
                        orderby a.n_contenedor
                        select new
                        {
                            IdDeta = a.IdDeta,
                            n_contenedor = a.n_contenedor
                        };

            List<string> contenedores = new List<string>();
            foreach (var item in query)
            {
                contenedores.Add(item.n_contenedor);
            }

            return contenedores.ToArray();

        }

        [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
       public static string ValidarSize(string Size)
       {
           Thread.Sleep(4000);
           bool _valid = false;
           
            _valid = Convert.ToBoolean(DetaNavieraDAL.ValidaSize(Size));

            if (_valid == true)
                System.Diagnostics.Debug.WriteLine("Cantidad inválida");

            return Size;           

       }

        private void SetContextKey()
        {

            AjaxControlToolkit.AutoCompleteExtender modal = (AjaxControlToolkit.AutoCompleteExtender)txtBusqueda.FindControl("AutoCompleteExtender1");
            modal.ContextKey = Session["IdRegistro"].ToString();// Any constant value
        }

        protected void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            SetContextKey();
        }

        protected void btnModificar_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("wfRegistro.aspx?IdReg={0}", Session["IdRegistro"].ToString()));
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
                       

            try
            {
                int valor = 0;                

                if(txtBusqueda.Text == string.Empty)
                    throw new Exception("Ingrese contenedor a buscar");

                if(txtBusqueda.Text.Length == 0)
                    throw new Exception("Ingrese contenedor a buscar");

                if (txtBusqueda.Text.Length == 11)
                    valor = Convert.ToInt32(DetaNavieraDAL.ValidarNumero(txtBusqueda.Text));
                else
                    throw new Exception("La búsqueda no produjo resultados, vuelva a intentarlo");

                if (valor == 1)
                {
                    List<DetaNaviera> pLista = DetaNavieraDAL.ObtenerBusqueda(Convert.ToInt32(Session["IdRegistro"]), txtBusqueda.Text.TrimEnd().TrimStart());

                    if (pLista == null)
                        pLista = new List<DetaNaviera>();

                    if (pLista.Count > 0)
                    {
                        
                        
                        
                        foreach (DetaNaviera item in pLista)
                        {
                            lblCorrelativo.Text = item.c_correlativo.ToString();
                            lblContenedor.Text = item.n_contenedor;
                            txtSize.Text = item.c_tamaño;
                            txtTara.Text = item.v_tara.ToString();
                            lblEstado.Text = item.b_estadoF;
                            lblMarchamo.Text = item.n_sello;
                            Session["c_tamaño"] = item.c_tamaño;
                        }
                    }
                }
                else
                    throw new Exception("La búsqueda no produjo resultados, vuelva a intentarlo");
                

            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
                lblError.Text = "Error : " + ex.Message;
                lblContenedor.Text = string.Empty;
                lblCorrelativo.Text = string.Empty;
                lblEstado.Text = string.Empty;
                lblMarchamo.Text = string.Empty;
            }
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            DateTime ahora = DetaNavieraLINQ.FechaBD();
            try
            {

                bool _valid = false;

                if (txtSize.Text != Session["c_tamaño"].ToString())
                {
                    if (txtSize.Text.Length == 4)
                    {
                        _valid = Convert.ToBoolean(DetaNavieraDAL.ValidaSize(txtSize.Text));
                        if (_valid == true)
                            throw new Exception("El tamaño no coincide con el estándar");
                    }
                    else
                        throw new Exception("Al modificar tamaño debe usar formato estandar de 4 cátacteres");
                        
                }

                if (_valid == false)
                {
                    DetaNaviera _deta = new DetaNaviera
                    {
                        IdReg = Convert.ToInt32(Session["IdRegistro"]),
                        n_contenedor = lblContenedor.Text,
                        c_correlativo = Convert.ToInt32(lblCorrelativo.Text),
                        grupo = Label4.Text,
                        grua = Label5.Text,
                        c_marcacion = Convert.ToInt32(Session["c_marcacion"]),
                        b_chasis = Convert.ToBoolean(ckList.Items[0].Selected),
                        b_rastra = Convert.ToBoolean(ckList.Items[1].Selected),
                        c_tamaño = txtSize.Text.TrimEnd().TrimStart(),
                        v_tara = Convert.ToInt32(txtTara.Text.TrimEnd().TrimStart())
                    };



                    int _resul = Convert.ToInt32(DetaNavieraDAL.ActualizarRecepcion(_deta));

                    CargarM();

                    string c_naviera = null, c_voyage = null;
                    List<EncaNaviera> _listaEnca = EncaNavieraDAL.ObtenerDetalle(Convert.ToInt32(Session["IdRegistro"]));

                    foreach (var itEnca in _listaEnca)
                    {
                        c_naviera = itEnca.c_naviera;
                        c_voyage = itEnca.c_voyage;
                    }
                    

                    EnvioCorreo _correo1 = new EnvioCorreo();
                    string Html;

                    Html = "<dir style=\"font-family: 'Arial'; font-size: 12px; line-height: 1.2em\">";
                    Html += "<br />MÓDULO : RECEPCIÓN DE CONTENEDOR DE IMPORTACIÓN <br />";
                    Html += "TIPO DE MENSAJE : NOTIFICACIÓN RECEPCIÓN DE CONTENEDOR <br /><br />";
                    Html += string.Format("Se notifica que la recepción del contenedor # {0}, numeral {1} ha sido recibido en Puerto de Acajutla el día {2}", lblContenedor.Text, lblCorrelativo.Text, ahora.ToString());

                    //_correo.Subject = string.Format("Importación de Archivo Buque {0} Número de Viaje {1}", d_buque, cn_voyage);
                    _correo1.Subject = string.Format("Recepción de Contenedor Puerto de Acajutla: Buque {0}, número de Viaje {1} ", Session["d_buque"], c_voyage);
                    _correo1.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_recepcion", DBComun.Estado.verdadero, c_naviera);
                    _correo1.ListaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_recepcion", DBComun.Estado.verdadero, c_naviera);
                    
                    _correo1.Asunto = Html;
                    _correo1.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.verdadero);

                    MensajeAlerta(string.Format("Se ha actualizado el contenedor #:{0}", lblContenedor.Text));
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
                lblError.Text = "Error : " + ex.Message;
               /* lblContenedor.Text = string.Empty;
                lblCorrelativo.Text = string.Empty;
                lblEstado.Text = string.Empty;
                lblMarchamo.Text = string.Empty;*/
            }
        }

        private void MensajeAlerta(string mensaje)
        {
            //En esta ocasión agregaremos un literal que a su vez agregaremos un div que nos servira de Dialog
            //O si prefieren pueden crear el div directamente en el HTML
            Literal li = new Literal();
            StringBuilder sbMensaje = new StringBuilder();
            //Creamos el Div
            sbMensaje.Append("<div id='dialog' title='Actualización Recepción'>");
            //Le indicamos el mensaje a mostrar
            sbMensaje.Append(mensaje);
            //cerramos el div
            sbMensaje.Append("</div>");
            //Aperturamos la escritura de Javascript
            sbMensaje.Append("<script type='text/javascript'>");
            sbMensaje.Append("$(document).ready(function () {");
            //Destrimos cualquier rastro de dialogo abierto
           
            sbMensaje.Append("$('#dialog').dialog('destroy');");
            //le indicamos que muestre el dialogo en modo Modal
            sbMensaje.Append(" $('#dialog').dialog({ modal: true });");
            //Si quieres que muestre un boton para cerrar el mensaje seria esta linea que dejare en comentario
            //sbMensaje.Append(" $('#dialog').dialog({ modal: true, buttons: { 'Ok': function() { $(this).dialog('close'); } } });");
            sbMensaje.Append("ClearAllControls();");
            sbMensaje.Append("});");
            sbMensaje.Append("</script>");
            //Agremamos el texto del stringbuilder al literal
            li.Text = sbMensaje.ToString();
            //Agregamos el literal a la pagina
            Page.Controls.Add(li);
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            Response.Redirect("wfRecepcion.aspx");
        }

       

        

        

        
       
       
        
    }
}