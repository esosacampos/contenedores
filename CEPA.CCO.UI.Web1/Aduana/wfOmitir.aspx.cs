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
using System.Text;

namespace CEPA.CCO.UI.Web
{
    public partial class wfOmitir : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if (Request.QueryString["nmanif"] != null)
                //    TextBox1.Text = Request.QueryString["nmanif"].ToString();
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                int n_manif = Convert.ToInt32(Request.QueryString["nmanif"]);

                if (TextBox1.Text != string.Empty || TextBox1.Text.TrimEnd().TrimStart().Length > 0)
                {
                    int resultado = Convert.ToInt32(DetaDocDAL.ActualizarOmitir(TextBox1.Text, n_manif, User.Identity.Name));

                    StringBuilder sb = new StringBuilder("<script language='javascript' type='text/javascript'>");
                    sb.Append("alert('Ahora proceda a dar clic en el botón Autorizar');");
                    sb.Append("CerrarConEvento();");
                    sb.Append("</script>");

                    ClientScript.RegisterStartupScript(typeof(Page), "Cerrar", sb.ToString());
                }
                else
                    throw new Exception("Debe justificar porque se omitirá la validación");
            }
            catch (Exception ex)
            {
                Response.Write(string.Format("<script LANGUAGE='javascript'>alert('CEPA-Contenedores Error: {0}');</script>", ex.Message));
            }
        }
    }
}