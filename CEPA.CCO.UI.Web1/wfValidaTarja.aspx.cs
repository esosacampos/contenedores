using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CEPA.CCO.Entidades;
using CEPA.CCO.BL;
using CEPA.CCO.DAL;
using System.Data.OleDb;
using CEPA.CCO.Linq;
using System.Threading;
using System.Text;

namespace CEPA.CCO.UI.Web
{
    public partial class wfValidaTarja : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnReg_Click(object sender, EventArgs e)
        {
            CargarArchivosLINQ _cargar = new CargarArchivosLINQ();
            try
            {
                if (txtTarja.Text.TrimStart().TrimEnd() == "" || txtObserva.Text.TrimStart().TrimEnd() == "")
                {
                    ScriptManager.RegisterStartupScript(this, typeof(string), "", "ErrorAlert();", true);
                }
                else
                {
                    int valor = ValidaTarjaDAL.ValidaCantidad(txtTarja.Text.TrimStart().TrimEnd());
                    if (valor > 0)
                    {

                        ScriptManager.RegisterStartupScript(this, typeof(string), "", string.Format("ErrorCantidad({0});", txtTarja.Text.TrimStart().TrimEnd()), true);
                        txtObserva.Text = string.Empty;
                        txtTarja.Text = string.Empty;
                        txtContenedor.Text = string.Empty;
                    }
                    else
                    {
                        //_cargar.Imprimir();
                        ScriptManager.RegisterStartupScript(this, typeof(string), "", "almacenando();", true);
                        Almacenar();
                        //Thread.Sleep(1000);
                        //_cargar.Clear("wfValidaTarja.aspx", "Registrado Correctamente");
                    }
                }
            }
            catch (Exception ex)
            {
                _cargar.Clear("wfValidaTarja.aspx", ex.Message);
            }
        }

        private void Almacenar()
        {
            if (txtTarja.Text != string.Empty && txtObserva.Text != string.Empty)
            {
                ValiadaTarja _valida = new ValiadaTarja
                {
                    c_tarja = txtTarja.Text,
                    b_observa = txtObserva.Text,
                    c_usuario = User.Identity.Name 

                };

                ValidaTarjaDAL.InsertarValida1(_valida);
                
                txtObserva.Text = string.Empty;
                txtTarja.Text = string.Empty;
                txtContenedor.Text = string.Empty;

            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            List<Manifiesto> pLista = new List<Manifiesto>();
            CargarArchivosLINQ _cargar = new CargarArchivosLINQ();
            try
            {
                //StringBuilder sb = new StringBuilder("<script language='javascript' type='text/javascript'>");
                //sb.Append("alert('CEPA-Contenedores Error: Esperar unos minutos realizará la búsqueda');");
                //sb.Append("</script>");


                //ClientScript.RegisterStartupScript(typeof(Button), "Cerrar", sb.ToString());

                ScriptManager.RegisterStartupScript(this, typeof(string), "", "alertError();", true);

                if (txtTarja.Text.Length >= 8)
                {
      
                    pLista = ValidaTarjaDAL.ObtenerContValida(txtTarja.Text);
                   

                    if (pLista.Count > 0)
                    {
                        foreach (Manifiesto item in pLista)
                        {
                            txtContenedor.Text = item.n_contenedor;
                        }
                    }
                    else
                        throw new Exception("Número de tarja no válido");
                }
                else
                    throw new Exception("Verificar número de tarja no es válido");
            }
            catch (Exception ex)
            {
                _cargar.Clear("wfValidaTarja.aspx", ex.Message);
            }
        }
    }
}