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

namespace CEPA.CCO.UI.Web.CLIENTES
{
    public partial class Principal : System.Web.UI.MasterPage
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            //int currentNumberOfUsers = CEPA.CCO.UI.Web.CLIENTES.Global.CurrentNumberOfUsers;
            //int totalNumberOfUsers = CEPA.CCO.UI.Web.CLIENTES.Global.TotalNumberOfUsers;
            //lblCurrentNumberOfUsers.Text = currentNumberOfUsers.ToString();
            //lblTotalNumberOfUsers.Text = totalNumberOfUsers.ToString();
        }

        protected override void OnLoad(EventArgs e)
        {

            //llama al metodo base

            base.OnLoad(e);

            //vuelve a recrear la etiqueta head de la master que tiene el script del resolve url, por lo que recrea de nuevo la ruta para esta pagina de los archivos

            Page.Header.DataBind();

        }



       

                   
    }
}