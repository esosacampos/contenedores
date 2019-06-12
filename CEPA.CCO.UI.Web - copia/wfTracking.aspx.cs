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

                grvTracking.HeaderRow.Cells[0].Attributes["data-class"] = "expand";

                // GridView1.HeaderRow.Cells[1].Attributes["data-hide"] = "phone";
                grvTracking.HeaderRow.Cells[5].Attributes["data-hide"] = "phone";
                grvTracking.HeaderRow.Cells[6].Attributes["data-hide"] = "phone";
                grvTracking.HeaderRow.Cells[7].Attributes["data-hide"] = "phone";
                grvTracking.HeaderRow.Cells[8].Attributes["data-hide"] = "phone";
                grvTracking.HeaderRow.Cells[9].Attributes["data-hide"] = "phone";
                grvTracking.HeaderRow.Cells[10].Attributes["data-hide"] = "phone";
                grvTracking.HeaderRow.Cells[11].Attributes["data-hide"] = "phone";
                grvTracking.HeaderRow.Cells[12].Attributes["data-hide"] = "phone";
                

                //GridView1.HeaderRow.Cells[8].Attributes["data-hide"] = "phone";


                grvTracking.HeaderRow.TableSection = TableRowSection.TableHeader;
                
                grvTracking.FooterRow.Cells[0].Attributes["text-align"] = "center";
                grvTracking.FooterRow.TableSection = TableRowSection.TableFooter;

               
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ex.Message + "');", true);
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

           
                    GridView gvProvisionales = (GridView) gvDetails.Rows[16].Cells[1].FindControl("grvProv");

                    if (gvProvisionales != null)
                    {
                        List<ProvisionalesEnca> pList = new List<ProvisionalesEnca>();
                        pList = DetaNavieraDAL.GetProvisionales(order.IdDeta, order.c_llegada, order.n_contenedor, order.c_naviera);

                        if (pList.Count > 0)
                        {
                            gvProvisionales.DataSource = pList;
                            gvProvisionales.DataBind();
                            
                            gvProvisionales.HeaderRow.Cells[0].Attributes["data-class"] = "expand";

                            // GridView1.HeaderRow.Cells[1].Attributes["data-hide"] = "phone";
                            gvProvisionales.HeaderRow.Cells[2].Attributes["data-hide"] = "phone";


                            List<ProvisionalesDeta> pListD = new List<ProvisionalesDeta>();
                            foreach (var itemC in pList)
                            {
                                pListD = itemC.ProviList;
                                break;
                            }

                            if (pListD.Count > 0)
                            {
                                if (gvProvisionales.Rows.Count > 0)
                                {
                                    GridView gvDetaProvi = (GridView)gvProvisionales.Rows[0].Cells[3].FindControl("grvDetailProvi");

                                    if (gvDetaProvi != null)
                                    {
                                        gvDetaProvi.DataSource = pListD;
                                        gvDetaProvi.DataBind();

                                        gvDetaProvi.HeaderRow.Cells[0].Attributes["data-class"] = "expand";

                                        // GridView1.HeaderRow.Cells[1].Attributes["data-hide"] = "phone";
                                        gvDetaProvi.HeaderRow.Cells[4].Attributes["data-hide"] = "phone";
                                        gvDetaProvi.HeaderRow.Cells[5].Attributes["data-hide"] = "phone";
                                        gvDetaProvi.HeaderRow.Cells[6].Attributes["data-hide"] = "phone";
                                        gvDetaProvi.HeaderRow.Cells[7].Attributes["data-hide"] = "phone";

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
                            }
                        }
                        //}
                    }
                

            e.Row.Cells[13].Visible = false;
            e.Row.Cells[14].Visible = false;
            grvTracking.HeaderRow.Cells[13].Visible = false;
            grvTracking.HeaderRow.Cells[14].Visible = false;         
        }

        
        [System.Web.Services.WebMethod]
        public static string ValidacionTarja(string c_tarja, string n_contenedor)
        {
            List<Pago> pLista = new List<Pago>();
            pLista = PagoDAL.ConsultarPago(c_tarja, n_contenedor);
            var query = (dynamic)null;

            if (pLista.Count > 0)
            {
                query = from a in pLista
                            select new
                            {
                                tarifa = a.b_tarifa,
                                validacion = a.validacion == "Si" ? "Si, El contenedor esta libre de pagos con CEPA" : "No, El contenedor tiene pagos pendientes con CEPA",
                                ValTransfer = a.ValTransfer,
                                ValDespacho = a.ValDespacho,
                                ValManejo = a.ValManejo,
                                ValAlmacenaje = a.ValAlmacenaje,
                               
                                style_va = a.validacion == "Si" ? "color:#00FF00;" : "color:#FF00000;",
                                style_transfer = a.ValTransfer == "Si" ? "CSS/Img/icono_correcto.png" : "CSS/Img/ErrorIcon.png",
                                style_despac = a.ValDespacho == "Si" ? "CSS/Img/icono_correcto.png" : "CSS/Img/ErrorIcon.png",
                                style_manejo = a.ValManejo == "Si" ? "CSS/Img/icono_correcto.png" : "CSS/Img/ErrorIcon.png",
                                style_alma = a.ValAlmacenaje == "Si" ? "CSS/Img/icono_correcto.png" : "CSS/Img/ErrorIcon.png",
                            };
            }
            else
            {
                Pago _clPago = new Pago
                {
                    b_tarifa = "DESCONOCIDA",
                    validacion = "No",
                    ValTransfer = "No",
                    ValDespacho = "No",
                    ValManejo = "No",
                    ValAlmacenaje = "No"
                                   
                };

                pLista.Add(_clPago);

                query = from a in pLista
                        select new
                        {
                            tarifa = a.b_tarifa,
                            validacion = a.validacion == "Si" ? "Si, El contenedor esta libre de pagos con CEPA" : "No, El contenedor tiene pagos pendientes con CEPA",
                            ValTransfer = a.ValTransfer,
                            ValDespacho = a.ValDespacho,
                            ValManejo = a.ValManejo,
                            ValAlmacenaje = a.ValAlmacenaje,

                            style_va = a.validacion == "Si" ? "color:#00FF00;" : "color:#FF00000;",
                            style_transfer = a.ValTransfer == "Si" ? "CSS/Img/icono_correcto.png" : "CSS/Img/ErrorIcon.png",
                            style_despac = a.ValDespacho == "Si" ? "CSS/Img/icono_correcto.png" : "CSS/Img/ErrorIcon.png",
                            style_manejo = a.ValManejo == "Si" ? "CSS/Img/icono_correcto.png" : "CSS/Img/ErrorIcon.png",
                            style_alma = a.ValAlmacenaje == "Si" ? "CSS/Img/icono_correcto.png" : "CSS/Img/ErrorIcon.png",
                        };
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(query);
        }

        

    }
}