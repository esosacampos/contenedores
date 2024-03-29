﻿using System;
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
            if (!IsPostBack)
            {
                ObtenerMenu();
            }
        }

        protected override void OnLoad(EventArgs e)
        {

            //llama al metodo base

            base.OnLoad(e);

            //vuelve a recrear la etiqueta head de la master que tiene el script del resolve url, por lo que recrea de nuevo la ruta para esta pagina de los archivos

            Page.Header.DataBind();

        }

        public void ObtenerMenu()
        {
            List<MenuItem> menuItem = new List<MenuItem>();

            List<CEPA.CCO.Entidades.Menu> menuLista = MenuDAL.ObtenerMenu(HttpContext.Current.User.Identity.Name);


            foreach (CEPA.CCO.Entidades.Menu item in menuLista)
            {
                if (item.PadreId == 0)
                {
                    mnuMenuItem = new MenuItem();
                    mnuMenuItem.Value = item.MenuId.ToString();
                    mnuMenuItem.Text = item.NombreMenu;
                    mnuMenuItem.ImageUrl = item.Icono;
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
                    mnuNewMenuItem.Text = menuSubItem.NombreMenu;
                    mnuNewMenuItem.ImageUrl = menuSubItem.Icono;
                    mnuNewMenuItem.NavigateUrl = menuSubItem.Url;

                    mnuMenuItem.ChildItems.Add(mnuNewMenuItem);

                    //llamada recursiva para ver si el nuevo menu item aun tiene elementos hijos.
                    AddMenuItem(mnuNewMenuItem, lista);
                }
            }
        }            
    }
}