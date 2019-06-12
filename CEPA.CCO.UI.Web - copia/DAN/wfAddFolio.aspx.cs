using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace CEPA.CCO.UI.Web.DAN
{
    public partial class wfAddFolio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Request.QueryString["IdReg"] != null)
                    {
                        if (Session["Oficio"] != null)
                        {
                            TxtLastName0.Text = Session["Oficio"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(string.Format("<script LANGUAGE='javascript'>alert('CEPA-Contenedores Error: {0}');</script>", ex.Message));
            }
        }

        protected void BtnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                if (TxtLastName0.Text != string.Empty)
                {
                    
                    Session["Oficio"] = TxtLastName0.Text;

                    StringBuilder sb = new StringBuilder("<script language='javascript' type='text/javascript'>");
                    sb.Append("CerrarConEvento();");

                    sb.Append("</script>");

                    ClientScript.RegisterStartupScript(typeof(Page), "Cerrar", sb.ToString());

                   
                    
                }
                else
                    throw new Exception("Debe ingresar el número de oficio");
            }
            catch (Exception ex)
            {
                Response.Write(string.Format("<script LANGUAGE='javascript'>alert('CEPA-Contenedores Error: {0}');</script>", ex.Message));
            }
        }
    }
}