  using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CEPA.CCO.BL;
using CEPA.CCO.DAL;
using CEPA.CCO.Entidades;
using CEPA.CCO.Linq;
using Microsoft.Office.Interop.Excel;
using System.Threading;
using System.Globalization;

namespace CEPA.CCO.UI.Web
{
    public partial class wfCargarBookings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {

                    EncaBuqueBL _encaBL = new EncaBuqueBL();
                    List<EncaBuque> _encaList = new List<EncaBuque>();
                    _encaList = EncaBuqueDAL.getNaviera(DBComun.Estado.verdadero, Session["c_naviera"].ToString());

                    if (_encaList.Count > 0)
                    {
                        foreach (EncaBuque item in _encaList)
                        {
                            c_naviera.Text = item.d_cliente;
                            break;
                        }
                    }
                }
                   
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ex.Message + "');", true);
            }
        }        

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("wfPrincipalNavi.aspx");
        }

        

    }
}