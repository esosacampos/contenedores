using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CEPA.CCO.Linq;
using System.Globalization;

namespace CEPA.CCO.UI.Web.Navieras
{
    public partial class wfRegistrarSalida : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblFecha.Text = DetaNavieraLINQ.FechaBD().ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("es-SV"));
            }
        }        
    }
}