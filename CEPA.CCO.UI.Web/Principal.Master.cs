using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CEPA.CCO.Entidades;
using CEPA.CCO.DAL;
using CEPA.CCO.Linq;
using System.Data;
using System.Drawing;
using System.Configuration;
using System.Text;

namespace CEPA.CCO.UI.Web
{
    public partial class Principal : System.Web.UI.MasterPage
    {
        private MenuItem mnuMenuItem;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Page.ClientScript.GetPostBackEventReference(this, string.Empty);

                string userUrl = System.IO.Path.GetFileName(Request.PhysicalPath);
                string userName = Page.User.Identity.Name.ToString();

                List<CEPA.CCO.Entidades.Menu> menuLista = MenuDAL.ObtenerMenu(userName);


                if (menuLista == null)
                {
                    menuLista = new List<Entidades.Menu>();
                }

                bool valida = false;
                if(menuLista.Count > 0)
                    valida = VerificarAutorizacion(userUrl, menuLista);

                

                

                string targetCtrl = Page.Request.Params.Get("__EVENTTARGET");
                if (targetCtrl != null && targetCtrl != string.Empty)
                {
                    //lblMsg.Text = targetCtrl + " button make Postback";
                }

                if (Request.Form["__EVENTTARGET"] == "btnLogOut")
                {
                    btnLogOut_Click(this, new EventArgs());
                }

                if (valida == false)
                {
                    sessionInput.InnerText = Session["d_usuario"].ToString().ToUpper();
                    Response.Redirect("~/wfDenegado.aspx", false);
                }

                if (!IsPostBack)
                {
                    ObtenerMenu(menuLista);
                    sessionInput.InnerText = Session["d_usuario"].ToString().ToUpper();

                    //string path = HttpContext.Current.Request.Url.AbsolutePath;
                    
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ex.Message + "');", true); 
            }
        }

        protected override void OnLoad(EventArgs e)
        {

            //llama al metodo base

            base.OnLoad(e);

            //vuelve a recrear la etiqueta head de la master que tiene el script del resolve url, por lo que recrea de nuevo la ruta para esta pagina de los archivos

            Page.Header.DataBind();

        }

        public bool VerificarAutorizacion(string urlUser, List<Entidades.Menu> menuLista)
        {
            bool value = false;

            List<Entidades.Menu> pFiltro = new List<Entidades.Menu>();

            pFiltro = menuLista.Where(a => a.Url.Contains(urlUser)).ToList();

            if (pFiltro.Count > 0)
                value = true;


            return value;
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            Session["c_naviera"] = null;
            Session["c_usuario"] = null;
            Session["d_usuario"] = null;
            //Session["s_email_reg"] = item.c_mail;
            Session["c_iso_navi"] = null;
            Session["c_navi_corto"] = null;
            Session["b_sidunea"] = null;
            Session["b_ibooking"] = null;
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("~/Inicio.aspx", false);
        }

        public void ObtenerMenu(List<CEPA.CCO.Entidades.Menu> menuLista)
        {
            List<MenuItem> menuItem = new List<MenuItem>();
                     


            foreach (CEPA.CCO.Entidades.Menu item in menuLista)
            {
                if (item.PadreId == 0)
                {
                    mnuMenuItem = new MenuItem();
                    mnuMenuItem.Value = item.MenuId.ToString();
                    mnuMenuItem.Text = "<span class='" + item.Icono + "' style='margin-right:5px;'></span>" + item.NombreMenu;
                    //mnuMenuItem.ImageUrl = item.Icono;
                    mnuMenuItem.NavigateUrl = item.Url;

                    //agregar al menu principal de la pagina html
                    MenuPrincipal.Items.Add(mnuMenuItem);

                    //hacemos un llamado al metodo recursivo encargado de generar el arbol del menu.
                    AddMenuItem(mnuMenuItem, menuLista);
                }
            }

        }

        private void AddMenuItem(MenuItem mnuMenuItem, List<CEPA.CCO.Entidades.Menu> lista)
        {
            MenuItem mnuNewMenuItem;

            foreach (CEPA.CCO.Entidades.Menu menuSubItem in lista)
            {
                if (menuSubItem.PadreId.ToString().Equals(mnuMenuItem.Value)
                    && !menuSubItem.MenuId.ToString().Equals(menuSubItem.PadreId.ToString()))
                {
                    mnuNewMenuItem = new MenuItem();

                    mnuNewMenuItem.Value = menuSubItem.MenuId.ToString();
                    mnuNewMenuItem.Text = "<span class='" + menuSubItem.Icono + "' style='margin-right:5px;'></span>" + menuSubItem.NombreMenu;                    
                    //mnuNewMenuItem.ImageUrl = menuSubItem.Icono;
                    mnuNewMenuItem.NavigateUrl = menuSubItem.Url;

                    mnuMenuItem.ChildItems.Add(mnuNewMenuItem);

                    //llamada recursiva para ver si el nuevo menu item aun tiene elementos hijos.
                    AddMenuItem(mnuNewMenuItem, lista);
                }
            }
        }            
    }
}