using CEPA.CCO.BL;
using CEPA.CCO.Entidades;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace CEPA.CCO.UI.Web
{
    public partial class wfConsulExport : System.Web.UI.Page
    {
        private Cont_ExpBL BLConte = new Cont_ExpBL();
        public List<Cont_Exp> contenedores = new List<Cont_Exp>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txt_Contenedor.Value = "";
                txt_Contenedor.Focus();
            }
        }

        protected void btn_Buscar_Click(object sender, EventArgs e)
        {
            if (txt_Contenedor.Value != "" && (txt_Contenedor.Value).Length == 11)
            {
                contenedores = BLConte.GetContenedor(txt_Contenedor.Value);
                if (contenedores.Count > 0)
                {
                    this.Rpt_lista.DataSource = contenedores;
                    this.Rpt_lista.DataBind();
                }
                else
                {
                    Rpt_lista.DataSource = null;
                    this.Rpt_lista.DataBind();
                    ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('Número de contenedor no encontrado');", true);
                    //Response.Write("<script>window.alert('Número de contenedor no encontrado');</script>");
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('Verifique los datos');", true);
                //Response.Write("<script>window.alert('Verifique los datos.');</script>");
            }
        }
    }
}