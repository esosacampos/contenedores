using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CEPA.CCO.Entidades;
using CEPA.CCO.DAL;
using CEPA.CCO.Linq;

namespace CEPA.CCO.WEB.Operaciones
{
    public partial class wfRecepcion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GridView1.DataSource = DetaNavieraDAL.ObtenerRecibidos(Convert.ToInt32(Session["IdRegistro"]), Session["grupo"].ToString(), Session["grua"].ToString());
            GridView1.DataBind();
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("wfDetalle.aspx");
        }
    }
}