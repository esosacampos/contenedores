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


namespace CEPA.CCO.UI.Web
{
    public partial class wfPerfil : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                    Cargar();
            }
            catch (Exception ex)
            {
                Response.Write(string.Format("<script LANGUAGE='javascript'>alert('CEPA-Contenedores Error: {0}');</script>", ex.Message));
            }
        }

        private void Cargar()
        {
            GridView2.DataSource = MenuDAL.ObtenerPerfiles();
            GridView2.DataBind();
        }

        protected void LLenarArbol(TreeNodeCollection nodo, int pIdPerfil, int pPadre)
        {
            int ThisId;
            string ThisName;

            List<CEPA.CCO.Entidades.Menu> pMenu = MenuDAL.ObtenerPefil(pIdPerfil).Where(a => a.PadreId == pPadre).ToList();

            foreach (CEPA.CCO.Entidades.Menu item in pMenu)
            {
                ThisId = Convert.ToInt32(item.MenuId);
                ThisName = item.NombreMenu;

                TreeNode NewNode = new TreeNode(ThisName, ThisId.ToString());

                nodo.Add(NewNode);
                LLenarArbol(NewNode.ChildNodes, pIdPerfil, ThisId);
                TreeView1.ExpandAll();

            }
        }

        protected void GridView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            TreeView1.Nodes.Clear();
            GridViewRow row = GridView2.SelectedRow;

            int pId = Convert.ToInt32(GridView2.DataKeys[row.RowIndex].Value);

            LLenarArbol(TreeView1.Nodes, pId, 0);
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton button = (LinkButton)e.Row.FindControl("lkButton");
                int desc = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "IdPerfil"));

                //button.Attributes.Add("OnClientClick", string.Format("Javascript:abrirModal('wfAddPerfil.aspx?even=editar&cod={0}')", desc));

                button.OnClientClick = string.Format("Javascript:abrirModal('wfAddPerfil.aspx?even=editar&cod={0}')", desc);

            }
        }
    }
}