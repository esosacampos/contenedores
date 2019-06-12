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

namespace CEPA.CCO.UI.Web
{
    public partial class wfPrincipalNavi : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {          
            if (!IsPostBack)
            {
                try
                {
                    Cargar();
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
            }
        }

        private void Cargar()
        {
            EncaBuqueBL _encaBL = new EncaBuqueBL();
            GridView1.DataSource = DocBuqueLINQ.ObtenerBuqueDoc(Session["c_naviera"].ToString());
            GridView1.DataBind();             
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            Cargar();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            List<DocBuque> _lista = DocBuqueLINQ.ObtenerBuqueDoc(Session["c_naviera"].ToString());
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            int CantArch;

            string c_llegada;
            string c_imo;
            int b_noti;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //HyperLink link = e.Row.FindControl("Link") as HyperLink;
                //HyperLink link1 = e.Row.FindControl("Link1") as HyperLink;

                c_imo = e.Row.Cells[0].Text;
                c_llegada = e.Row.Cells[1].Text;

                //b_noti = Convert.ToInt32(EncaNavieraDAL.ObtenerNotiS(c_imo, c_llegada, Session["c_naviera"].ToString()));
                

                CantArch = Convert.ToInt32(e.Row.Cells[4].Text);

                //if (CantArch == 0)
                //{
                //    link1.Enabled = true;
                //    //link.Enabled = false;
                //}
                //else if (CantArch > 0 && b_noti == 1)
                //{
                //    //link.Enabled = true;
                //    link1.Enabled = true;
                //}
                //else if (CantArch > 0 && b_noti == 0)
                //{
                //    //link.Enabled = false;
                //    link1.Enabled = true;
                //}
                    
                


            }
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            Cargar();
        }
    }
}