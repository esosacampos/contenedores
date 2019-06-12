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
using System.Text;
using System.DirectoryServices;
using System.Web.Configuration;

namespace CEPA.CCO.UI.Web
{
    public partial class wfUserNavi : System.Web.UI.Page
    {
        public static List<Naviera> pNavieras = new List<Naviera>();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    CargarCliente();

                    if (Request.QueryString["even"] != null)
                    {
                        ddlCliente.SelectedValue = Request.QueryString["even"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(string.Format("<script LANGUAGE='javascript'>alert('CEPA-Contenedores Error: {0}');</script>", ex.Message));
            }
        }

        private void CargarCliente()
        {
            pNavieras = DocBuqueLINQ.ObtenerClientesValidos();
            ddlCliente.DataTextField = "d_nombre";
            ddlCliente.DataValueField = "c_cliente";
            ddlCliente.DataSource = pNavieras;
            ddlCliente.DataBind();
        }

        protected void BtnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlCliente.SelectedIndex > 0 && TxtLastName.Text != string.Empty && TxtLastName0.Text != string.Empty)
                {
                    Usuario _user = new Usuario
                    {
                        c_naviera = ddlCliente.SelectedValue,
                        c_iso_navi = TxtLastName.Text.ToUpper(),
                        c_navi_corto = TxtLastName0.Text.ToUpper()
                    };

                    string resul = UsuarioDAL.InserUserNaviera(_user);

                    StringBuilder sb = new StringBuilder("<script language='javascript' type='text/javascript'>");
                    sb.Append("CerrarConEvento();");
                    sb.Append("</script>");

                    ClientScript.RegisterStartupScript(typeof(Page), "Cerrar", sb.ToString());
                }


            }
            catch (Exception ex)
            {
                Response.Write(string.Format("<script LANGUAGE='javascript'>alert('CEPA-Contenedores Error: {0}');</script>", ex.Message));
            }
        }
    }
}