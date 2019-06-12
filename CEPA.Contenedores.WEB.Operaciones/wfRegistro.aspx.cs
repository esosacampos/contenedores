using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CEPA.CCO.WEB.Operaciones
{
    public partial class wfRegistro : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Request.QueryString["IdReg"] == null)
                        throw new Exception("Debe seleccionar una operación válida");
                    else                        
                        Session["IdRegistro"] = Request.QueryString["IdReg"].ToString();


                    if (Session["grupo"] != null)
                        ddlGrupo.SelectedValue = Session["grupo"].ToString();

                    if (Session["grua"] != null)
                        ddlGrua.SelectedValue = Session["grua"].ToString();
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void btnContinuar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlGrupo.SelectedValue == "0")
                    throw new Exception("Debe indicar el grupo al que pertenece");

                if (ddlGrua.SelectedValue == "0")
                    throw new Exception("Debe indicar la grúa para efectuar operaciones");

                if (ddlGrupo.SelectedIndex != -1)
                    Session["grupo"] = ddlGrupo.SelectedValue;

                if (ddlGrua.SelectedIndex != -1)
                    Session["grua"] = ddlGrua.SelectedValue;

                if (Session["grupo"] != null && Session["grua"] != null)
                    Response.Redirect("wfDetalle.aspx");
                else
                    throw new Exception("Debe indicar valores válidos para iniciar operaciones");

            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }
    }
}