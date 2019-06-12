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
    public partial class wfUsuarios : System.Web.UI.Page
    {
        public static string pId = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Cargar();
            }
        }

        private void Cargar()
        {
            GridView2.DataSource = DetaNavieraLINQ.ObtenerUsuarios();
            GridView2.DataBind();

            if (GridView2.Rows.Count > 0)
            {
                int conta = 0;
                foreach (GridViewRow item in GridView2.Rows)
                {
                    if (conta == 0)
                        CargarPerfil(item.Cells[2].Text);
                    conta = 1;
                }
            }
        }

        protected void GridView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            TreeView1.Nodes.Clear();
            GridViewRow row = GridView2.SelectedRow;
            pId = GridView2.DataKeys[row.RowIndex].Value.ToString();

            CargarPerfil(pId);
        }

        private void CargarPerfil(string pId)
        {
            GridView1.DataSource = MenuDAL.ObtenerPerfilUser(pId);
            GridView1.DataBind();
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TreeView1.Nodes.Clear();
            GridViewRow row = GridView1.SelectedRow;

            int pId = Convert.ToInt32(GridView1.DataKeys[row.RowIndex].Value);

            LLenarArbol(TreeView1.Nodes, pId, 0, true);
        }

        protected void LLenarArbol(TreeNodeCollection nodo, int pIdPerfil, int pPadre, bool pVariable)
        {
            int ThisId;
            string ThisName;
            List<CEPA.CCO.Entidades.Menu> pMenu = new List<Entidades.Menu>();

            if (pVariable == true)
                pMenu = MenuDAL.ObtenerPefil(pIdPerfil).Where(a => a.PadreId == pPadre).ToList();
            else
                pMenu = MenuDAL.ObtenerMenu(pId).Where(a => a.PadreId == pPadre).ToList();

            foreach (Entidades.Menu item in pMenu)
            {
                ThisId = Convert.ToInt32(item.MenuId);
                ThisName = item.NombreMenu;

                TreeNode NewNode = new TreeNode(ThisName, ThisId.ToString());

                nodo.Add(NewNode);

                LLenarArbol(NewNode.ChildNodes, pIdPerfil, ThisId, pVariable);
                TreeView1.ExpandAll();

            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (pId != null)
            {
                TreeView1.Nodes.Clear();
                LLenarArbol(TreeView1.Nodes, 0, 0, false);
            }
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton button = (LinkButton)e.Row.FindControl("lkButton");
                string desc = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "c_usuario"));

                //button.Attributes.Add("OnClientClick", string.Format("Javascript:abrirModal('wfAddPerfil.aspx?even=editar&cod={0}')", desc));

                button.OnClientClick = string.Format("Javascript:abrirModal('wfAddUser.aspx?even=editar&cod={0}')", desc);

            }
        }

      
    }
}