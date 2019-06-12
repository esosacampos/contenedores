using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CEPA.CCO.UI.Web
{
    public partial class Principal : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnLoad(EventArgs e)
        {

            //llama al metodo base

            base.OnLoad(e);

            //vuelve a recrear la etiqueta head de la master que tiene el script del resolve url, por lo que recrea de nuevo la ruta para esta pagina de los archivos

            Page.Header.DataBind();

        }

        protected void LoginStatus1_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Session.RemoveAll();
            Session.Abandon();
            Response.Redirect("~/Inicio.aspx");
        }
    }
}