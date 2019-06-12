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

namespace CEPA.CCO.UI.Web
{
    public partial class wfAddPerfil : System.Web.UI.Page
    {
        public static int bEsta;
        public static List<Entidades.Menu> pMenuList = new List<Entidades.Menu>();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Request.QueryString["even"] != null)
                    {

                        LLenarArbol(TreeView1.Nodes, 0);
                        if (Request.QueryString["even"].ToString() == "insertar")
                        {
                            DropDownList1.Enabled = false;
                        }
                        else
                        {
                            DropDownList1.DataSource = MenuDAL.ObtenerEstados();
                            DropDownList1.DataTextField = "Descripcion";
                            DropDownList1.DataValueField = "IdProceso";
                            DropDownList1.DataBind();

                            List<Perfil> jLista = MenuDAL.ObtenerPerfiles(Convert.ToInt32(Request.QueryString["cod"]));

                            if (jLista.Count > 0)
                            {
                                foreach (Perfil item in jLista)
                                {
                                    TxtLastName.Text = item.NombrePerfil.ToUpper();
                                    if (item.Habilitado == "Activo")
                                        DropDownList1.SelectedIndex = 1;
                                    else
                                        DropDownList1.SelectedIndex = 2;
                                }
                            }

                            pMenuList = MenuDAL.ObtenerPefil(Convert.ToInt32(Request.QueryString["cod"]));

                            RecorrerMarcas(TreeView1.Nodes);
                        }


                    }

                }

                TreeView1.Attributes.Add("onclick", "OnTreeClick(event)");
            }
            catch (Exception ex)
            {
                Response.Write(string.Format("<script LANGUAGE='javascript'>alert('CEPA-Contenedores Error: {0}');</script>", ex.Message));
            }
        }

        protected void LLenarArbol(TreeNodeCollection nodo, int pPadre)
        {
            int ThisId;
            string ThisName;

            List<Entidades.Menu> pMenu = MenuDAL.ObtenerMenu().Where(a => a.PadreId == pPadre).ToList();

            foreach (Entidades.Menu item in pMenu)
            {
                ThisId = Convert.ToInt32(item.MenuId);
                ThisName = item.NombreMenu;

                TreeNode NewNode = new TreeNode(ThisName, ThisId.ToString());
                nodo.Add(NewNode);
                LLenarArbol(NewNode.ChildNodes, ThisId);
                TreeView1.ExpandAll();
            }
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownList1.SelectedIndex == 1)
                bEsta = 1;
            else if (DropDownList1.SelectedIndex == 2)
                bEsta = 0;
        }

        protected void BtnAceptar_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["even"].ToString() == "insertar")
            {
                int resul = MenuDAL.InsertarPerfil(TxtLastName.Text.ToUpper());
                if (resul > 0)
                {
                    RecorrerNodos(TreeView1.Nodes, resul);
                }
            }
            else
            {
                int del = MenuDAL.EliminarPerfil(Convert.ToInt32(Request.QueryString["cod"]));
                if (del > 0)
                {
                    MenuDAL.ActualizarPerfil(TxtLastName.Text.ToUpper(), bEsta, Convert.ToInt32(Request.QueryString["cod"]));
                    RecorrerNodos(TreeView1.Nodes, Convert.ToInt32(Request.QueryString["cod"]));
                }
            }

            StringBuilder sb = new StringBuilder("<script type='text/javascript'>");
            sb.Append("CerrarConEvento();");
            sb.Append("</script>");

            ClientScript.RegisterStartupScript(typeof(Page), "Cerrar", sb.ToString());

        }

        private void RecorrerNodos(TreeNodeCollection treeNode, int resultado)
        {
            int contador = 0;
            try
            {
                //Si el nodo que recibimos tiene hijos se recorrerá
                //para luego verificar si esta o no checado
                foreach (TreeNode tn in treeNode)
                {

                    //Se Verifica si esta marcado...
                    if (tn.Checked == true || tn.Value == "100")
                    {
                        PerfilMenu pMenu = new PerfilMenu
                        {
                            IdPerfilMenu = -1,
                            IdPerfil = resultado,
                            MenuId = Convert.ToInt32(tn.Value)
                        };
                        //Si esta marcado mostramos el texto del nodo
                        int res = MenuDAL.InsertarMenuPefil(pMenu);

                    }

                    if (tn.ChildNodes.Count > 0)
                    {
                        contador = 0;
                        foreach (TreeNode item in tn.ChildNodes)
                        {
                            if (item.Checked == true)
                                contador = contador + 1;
                        }
                        if (contador > 0)
                        {
                            PerfilMenu pMenu = new PerfilMenu
                            {
                                IdPerfilMenu = -1,
                                IdPerfil = resultado,
                                MenuId = Convert.ToInt32(tn.Value)
                            };

                            //Si esta marcado mostramos el texto del nodo
                            int res = MenuDAL.InsertarMenuPefil(pMenu);

                        }
                    }

                    RecorrerNodos(tn.ChildNodes, resultado);

                    //Ahora hago verificacion a los hijos del nodo actual
                    //Esta iteración no acabara hasta llegar al ultimo nodo principal



                }
            }
            catch (Exception ex)
            {
                Response.Write(string.Format("<script LANGUAGE='javascript'>alert('CEPA-Contenedores Error: {0}');</script>", ex.Message));
            }
        }

        private void RecorrerMarcas(TreeNodeCollection treeNode)
        {

            try
            {
                //Si el nodo que recibimos tiene hijos se recorrerá
                //para luego verificar si esta o no checado
                foreach (TreeNode tn in treeNode)
                {
                    bool insertOrderNew = pMenuList.Find(r => r.MenuId == Convert.ToInt32(tn.Value)) == null ? false : true;

                    if (insertOrderNew == true)
                        tn.Checked = true;

                    RecorrerMarcas(tn.ChildNodes);

                    //Ahora hago verificacion a los hijos del nodo actual
                    //Esta iteración no acabara hasta llegar al ultimo nodo principal
                }
            }
            catch (Exception ex)
            {
                Response.Write(string.Format("<script LANGUAGE='javascript'>alert('CEPA-Contenedores Error: {0}');</script>", ex.Message));
            }
        }
    }
}