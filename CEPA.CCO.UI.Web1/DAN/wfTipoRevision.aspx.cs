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


namespace CEPA.CCO.UI.Web.DAN
{
    public partial class wfTipoRevision : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Cargar();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void Cargar()
        {
            GridView2.DataSource = TipoSolicitudDAL.ObtenerRevisionTo();            
            GridView2.DataBind();            
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton button = (LinkButton)e.Row.FindControl("lkButton");
                string desc = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "IdRevision"));

                //button.Attributes.Add("OnClientClick", string.Format("Javascript:abrirModal('wfAddPerfil.aspx?even=editar&cod={0}')", desc));

                button.OnClientClick = string.Format("Javascript:abrirModal('wfAddTipo.aspx?even=editar&cod={0}')", desc);

            }
        }
    }
}