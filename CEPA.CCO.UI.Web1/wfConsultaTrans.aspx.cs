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
using System.Threading;
using System.Globalization;
using System.Drawing;

namespace CEPA.CCO.UI.Web
{
    public partial class wfConsultaTrans : System.Web.UI.Page
    {
        public string c_naviera;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    
                    if (Request.QueryString["IdReg"] == null)
                    {
                        throw new Exception("Falta código de cabecera");
                    }
                    else
                    {
                        List<DocBuque> _encaList = DocBuqueLINQ.ObtenerTransmi(Convert.ToInt32(Request.QueryString["IdReg"]));
                        //Sacar naviera
                        if (_encaList.Count > 0)
                        {
                            foreach (DocBuque item in _encaList)
                            {                                
                                c_imo.Text = item.c_imo;
                                d_buque.Text = item.d_buque;
                                f_llegada.Text = item.f_llegada.ToString("dd/MM/yyyy HH:mm:ss");
                                c_llegada.Text = item.c_llegada;
                                viaje.Text = item.c_voyage;
                                manif.Text = item.num_manif.ToString();
                                c_naviera = item.c_cliente;
                                tot_imp.Text = item.TotalImp.ToString();
                                tot_trans.Text = item.TotalTrans.ToString();
                            }

                            Cargar();
                        }
                        else
                        {
                           
                        }

                    }

                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
            }
        }

        private void Cargar()
        {
            GridView1.DataSource = DetaNavieraDAL.ObtenerDetaTrans(Convert.ToInt32(Request.QueryString["IdReg"])).ToList();
            GridView1.DataBind();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
             if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string trans = e.Row.Cells[4].Text;
  
                if (trans.TrimEnd().TrimStart() == "NO")
                {
                    e.Row.BackColor = Color.FromName("#ffeb9c");
                    
                }           
            }
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("wfBuquesTrans.aspx");
        }
    }
}