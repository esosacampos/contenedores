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
    public partial class wfAddUser : System.Web.UI.Page
    {
        public static List<string> pIdPerfiles = new List<string>();
        public static int bEsta;
        public static List<Perfil> pLista = new List<Perfil>();
        public static List<Naviera> pNavieras = new List<Naviera>();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Request.QueryString["even"] != null)
                    {
                        CargarCliente();
                        CargarEstado();

                        if (Request.QueryString["even"].ToString() == "insertar")
                        {
                            DropDownList1.Enabled = false;
                        }
                        else
                        {
                            List<Usuario> jLista = MenuDAL.ObtenerPerfilesUser(Request.QueryString["cod"].ToString());

                            if (jLista.Count > 0)
                            {
                                foreach (Usuario item in jLista)
                                {
                                    //DropDownList2.SelectedValue = pUsuarios.Where(a => a.c_id_usuario == item.IdUser).ToString();
                                    txtUser.Text = item.c_usuario;
                                    txtNombre.Text = item.d_usuario;
                                    ddlCliente.SelectedValue = item.c_naviera;

                                    if (item.Habilitado == "Activo")
                                    {
                                        DropDownList1.SelectedIndex = 1;
                                        bEsta = 1;
                                    }
                                    else
                                    {
                                        DropDownList1.SelectedIndex = 2;
                                        bEsta = 0;
                                    }

                                    
                                    txtUser.Enabled = false;

                                }
                            }

                            pLista = MenuDAL.ObtenerPerfilUser(Request.QueryString["cod"].ToString());
                        }

                        GridView2.DataSource = MenuDAL.ObtenerPerfiles();
                        GridView2.DataBind();
                    }

                }
            }
            catch (Exception ex)
            {
                Response.Write(string.Format("<script LANGUAGE='javascript'>alert('CEPA-Contenedores Error: {0}');</script>", ex.Message));
            }
        }

        private void CargarEstado()
        {
            DropDownList1.DataSource = MenuDAL.ObtenerEstados();
            DropDownList1.DataTextField = "Descripcion";
            DropDownList1.DataValueField = "IdProceso";
            DropDownList1.DataBind();
        }

        private void CargarCliente()
        {
            pNavieras = DocBuqueLINQ.ObtenerClientesValidos();
            ddlCliente.DataTextField = "d_nombre";
            ddlCliente.DataValueField = "c_cliente";
            ddlCliente.DataSource = pNavieras;
            ddlCliente.DataBind();
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownList1.SelectedIndex == 1)
                bEsta = 1;
            else if (DropDownList1.SelectedIndex == 2)
                bEsta = 0;
        }

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
           
            KeepPerfil((GridView)GridView2);

            TreeView1.Nodes.Clear();

            pIdPerfiles = HttpContext.Current.Session["Perfiles"] as List<string>;

            if (pIdPerfiles == null)
                pIdPerfiles = new List<string>();

            if (pIdPerfiles.Count > 0)
            {
                string[] pPerfiles = pIdPerfiles.ToArray();

                LLenarArbol(TreeView1.Nodes, 0, pPerfiles);
            }
        
        }

        protected void LLenarArbol(TreeNodeCollection nodo, int pPadre, params string[] pIdPerfil)
        {
            int ThisId;
            string ThisName;

            List<Entidades.Menu> pMenu = MenuDAL.ObtenerPefilIdM(pIdPerfil).Where(a => a.PadreId == pPadre).ToList();

            foreach (Entidades.Menu item in pMenu)
            {
                ThisId = Convert.ToInt32(item.MenuId);
                ThisName = item.NombreMenu;

                TreeNode NewNode = new TreeNode(ThisName, ThisId.ToString());

                nodo.Add(NewNode);
                LLenarArbol(NewNode.ChildNodes, ThisId, pIdPerfil);
                TreeView1.ExpandAll();
            }
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Request.QueryString["even"].ToString() == "editar")
                {
                    CheckBox button = (CheckBox)e.Row.FindControl("CheckBox1");

                    if (pLista.Count > 0)
                    {
                        bool insertOrderNew = pLista.Find(r => r.IdPerfil == Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "IdPerfil"))) == null ? false : true;

                        if (insertOrderNew == true)
                            button.Checked = true;
                    }
                }
            }

        }


        public static void KeepPerfil(GridView grid)
        {
            List<Perfil> _listCC = new List<Perfil>();

            //
            // se obtienen los id de producto checkeados de la pagina actual
            //
            List<string> checkPerfil = (from item in grid.Rows.Cast<GridViewRow>()
                                        let check = (CheckBox)item.FindControl("CheckBox1")
                                        where check.Checked
                                        select Convert.ToString(grid.DataKeys[item.RowIndex].Value)).ToList();




            HttpContext.Current.Session["Perfiles"] = checkPerfil;

            //EmpleadoBL _empBL = new EmpleadoBL();
            //_listEmp = _empBL.ObtenerEmpleados();

            //HttpContext.Current.Session["ListaEmpleados"] = _listEmp;

        }

        protected void BtnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                List<PerfilUsuario> pUsers = new List<PerfilUsuario>();
                if (Request.QueryString["even"].ToString() == "insertar")
                {
                    if (txtNombre.Text != string.Empty && pIdPerfiles.Count > 0 && txtUser.Text != string.Empty)
                    {

                        Almacenar(pUsers, true);
                    }
                }
                else
                {
                    int del = MenuDAL.EliminarUsuario(Convert.ToString(Request.QueryString["cod"]));

                    if (del > 0)
                    {
                        Almacenar(pUsers, false);
                        //MenuDAL.ActualizarUsuario(bEsta, txtUser.Text);
                    }
                }

                StringBuilder sb = new StringBuilder("<script language='javascript' type='text/javascript'>");
                sb.Append("CerrarConEvento();");
                sb.Append("</script>");

                ClientScript.RegisterStartupScript(typeof(Page), "Cerrar", sb.ToString());
            }
            catch (Exception ex)
            {
                Response.Write(string.Format("<script LANGUAGE='javascript'>alert('CEPA-Contenedores Error: {0}');</script>", ex.Message));
            }
        }

        private void Almacenar(List<PerfilUsuario> pUsers, bool pAlmacenar)
        {

            DirectoryEntry directoryEntry = new DirectoryEntry(string.Format("{0}", WebConfigurationManager.AppSettings["ActiveDirectoryPath"].ToString()));
            DirectorySearcher _dSearch = new DirectorySearcher(directoryEntry, string.Format("(SAMAccountName={0})", txtUser.Text));
            SearchResult _result = _dSearch.FindOne();

            if (pAlmacenar == true)
            {
                if (_result != null)
                {
                    ActUser(pUsers, true);
                }
                else
                {
                    throw new Exception("Este usuario no existe en el Directorio Activo");
                }
            }
            else
            {
                if (_result != null)
                {
                    ActUser(pUsers, false);
                }
                else
                {
                    throw new Exception("Este usuario no existe en el Directorio Activo");
                }
            }
        }

        private void ActUser(List<PerfilUsuario> pUsers, bool pValor)
        {
            string c_iso_navi = null, c_navi_corto = null;

            List<UsuarioNaviera> pUsuNavi = UsuarioDAL.ObtenerUsuNavi(ddlCliente.SelectedValue);

            if (pUsuNavi == null)
                pUsuNavi = new List<UsuarioNaviera>();

            if (pUsuNavi.Count == 0)
            {
                Response.Write("<script LANGUAGE='javascript'>alert('CEPA-Contenedores Error: Esta Naviera no posee registros en nuestro sistema debe agregarla');</script>");

                ScriptManager.RegisterClientScriptBlock(ddlCliente, typeof(DropDownList), "Popup", "javascript:Confirm(" + ddlCliente.Text + ");", true);

            }

            if (pUsuNavi.Count > 0)
            {
                foreach (var UsuN in pUsuNavi)
                {
                    c_iso_navi = UsuN.c_iso_navi;
                    c_navi_corto = UsuN.c_navi_corto;
                    break;
                }
            }

            Usuario _user = new Usuario
            {
                c_usuario = txtUser.Text,
                d_usuario = txtNombre.Text.ToUpper(),
                c_naviera = ddlCliente.SelectedValue,
                c_iso_navi = c_iso_navi,
                c_navi_corto = c_navi_corto
            };

            int _resultado = 0;
            if(pValor == true)
                _resultado = Convert.ToInt32(UsuarioDAL.InsertarUsuario(_user));
            else
                _resultado = Convert.ToInt32(UsuarioDAL.ActUsuario(_user));


            if (_resultado > 0)
            {

                foreach (string item in pIdPerfiles)
                {
                    PerfilUsuario _perfil = new PerfilUsuario
                    {
                        IdPerfil = Convert.ToInt32(item),
                        IdUser = txtUser.Text
                    };

                    pUsers.Add(_perfil);
                }

               
                MenuDAL.AlmacenarPerfilUsuario(pUsers);
                

            }
        }

        protected void ddlCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                 List<UsuarioNaviera> pUsuNavi = UsuarioDAL.ObtenerUsuNavi(ddlCliente.SelectedValue);

                if (pUsuNavi == null)
                    pUsuNavi = new List<UsuarioNaviera>();

                if (pUsuNavi.Count == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(ddlCliente, typeof(DropDownList), "Popup", "javascript:Confirm(" + ddlCliente.Text + ");", true);
                }
                
            }
            catch (Exception ex)
            {
                Response.Write(string.Format("<script LANGUAGE='javascript'>alert('CEPA-Contenedores Error: {0}');</script>", ex.Message));
            }
        }

       

        

        
    }
}