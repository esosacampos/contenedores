using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CEPA.CCO.Entidades;
using CEPA.CCO.BL;
using CEPA.CCO.DAL;
using CEPA.CCO.Linq;

namespace CEPA.CCO.UI.Web
{
    public partial class wfTracking : System.Web.UI.Page
    {
        private static readonly DateTime FIRST_GOOD_DATE = new DateTime(1900, 01, 01);

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            List<TrackingEnca> pLista = new List<TrackingEnca>();
            List<TrackingEnca> pLista1 = new List<TrackingEnca>();
            try
            {
                
                ScriptManager.RegisterClientScriptBlock(btnBuscar, typeof(Button), "testscript1", "alertError();", true);
                //System.Threading.Thread.Sleep(1000);
                
                if (txtBuscar.Text.Length == 11)
                {

                    
                    
                    pLista = DocBuqueLINQ.ObtenerTracking(txtBuscar.Text, Session["c_naviera"].ToString());
                    
                    

                    if (pLista.Count > 0)
                    {
                        grvTracking.DataSource = pLista;
                        grvTracking.DataBind();

                    }
                    else
                    {
                        grvTracking.DataSource = null;
                        grvTracking.DataBind();
                        Label lblEmptyMessage = grvTracking.Controls[0].Controls[0].FindControl("lblEmptyMessage") as Label;
                        lblEmptyMessage.Text = "No se poseen registros de este contenedor: " + txtBuscar.Text;
                    }

                    

                }
                else
                {
                    grvTracking.DataSource = null;
                    grvTracking.DataBind();
                    Label lblEmptyMessage = grvTracking.Controls[0].Controls[0].FindControl("lblEmptyMessage") as Label;
                    lblEmptyMessage.Text = "Este número no posee los 11 caracteres " + txtBuscar.Text;
                }

               
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }
               

        protected void grvTracking_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;
                        

            TrackingEnca order = (TrackingEnca)e.Row.DataItem;

            DetailsView gvDetails = (DetailsView)e.Row.FindControl("dtTracking");

            gvDetails.DataSource = order.TrackingList;
            gvDetails.DataBind();

            //ScriptManager.RegisterStartupScript(this, typeof(string), "", "almacenando();", true);
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "testscript1", "almacenando();", true);
             
            foreach (DetailsViewRow item in gvDetails.Rows)
            {

                if (ArchivoBookingDAL.isFecha(item.Cells[1].Text) == true)
                {
                    if (Convert.ToDateTime(item.Cells[1].Text) > FIRST_GOOD_DATE)
                    { }
                    else
                    {
                        item.Cells[1].Text = "";
                    }

                }               
            }

           
                    GridView gvProvisionales = (GridView) gvDetails.Rows[11].Cells[1].FindControl("grvProv");

                    if (gvProvisionales != null)
                    {
                        List<ProvisionalesEnca> pList = new List<ProvisionalesEnca>();
                        pList = DetaNavieraDAL.GetProvisionales(order.IdDeta, order.c_llegada, order.n_contenedor, order.c_naviera);
                        gvProvisionales.DataSource = pList;
                        gvProvisionales.DataBind();

                        List<ProvisionalesDeta> pListD = new List<ProvisionalesDeta>();
                        foreach (var itemC in pList)
	                    {
		                    pListD = itemC.ProviList;
                            break;
	                    }

                        //if (pListD.Count > 0)
                        //{
                        if (gvProvisionales.Rows.Count > 0)
                        {
                            GridView gvDetaProvi = (GridView)gvProvisionales.Rows[0].Cells[3].FindControl("grvDetailProvi");

                            if (gvDetaProvi != null)
                            {
                                gvDetaProvi.DataSource = pListD;
                                gvDetaProvi.DataBind();

                                for (int i = 0; i < gvDetaProvi.Rows.Count; i++)                                                     
                                {

                                    if (ArchivoBookingDAL.isFecha(gvDetaProvi.Rows[i].Cells[0].Text) == true)
                                    {
                                        if (Convert.ToDateTime(gvDetaProvi.Rows[i].Cells[0].Text) > FIRST_GOOD_DATE)
                                        { }
                                        else
                                        {
                                            gvDetaProvi.Rows[i].Cells[0].Text = "";
                                        }

                                    }

                                    if (ArchivoBookingDAL.isFecha(gvDetaProvi.Rows[i].Cells[6].Text) == true)
                                    {
                                        if (Convert.ToDateTime(gvDetaProvi.Rows[i].Cells[6].Text) > FIRST_GOOD_DATE)
                                        { }
                                        else
                                        {
                                            gvDetaProvi.Rows[i].Cells[6].Text = "";
                                        }

                                    }

                                    if (ArchivoBookingDAL.isFecha(gvDetaProvi.Rows[i].Cells[7].Text) == true)
                                    {
                                        if (Convert.ToDateTime(gvDetaProvi.Rows[i].Cells[7].Text) > FIRST_GOOD_DATE)
                                        { }
                                        else
                                        {
                                            gvDetaProvi.Rows[i].Cells[7].Text = "";
                                        }

                                    }
                                }

                            }
                        }
                        //}
                    }
                

            e.Row.Cells[7].Visible = false;
            e.Row.Cells[8].Visible = false;
            grvTracking.HeaderRow.Cells[7].Visible = false;
            grvTracking.HeaderRow.Cells[8].Visible = false;
            e.Row.Cells[9].Visible = false;
            grvTracking.HeaderRow.Cells[9].Visible = false;
        }

   

        

    }
}