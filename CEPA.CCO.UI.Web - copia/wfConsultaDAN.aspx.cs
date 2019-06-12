using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CEPA.CCO.Entidades;
using CEPA.CCO.BL;
using CEPA.CCO.DAL;

namespace CEPA.CCO.UI.Web
{
    public partial class wfConsultaDAN : System.Web.UI.Page
    {
        private static readonly DateTime FIRST_GOOD_DATE = new DateTime(1900, 01, 01);

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            List<DetaNaviera> pLista = new List<DetaNaviera>();
            List<DetaNaviera> pLista1 = new List<DetaNaviera>();
            try
            {
                if (txtBuscar.Text.Length == 11)
                {
                    pLista = DetaNavieraDAL.ConsultarDAN(txtBuscar.Text, Session["c_naviera"].ToString());

                    if (pLista.Count > 0)
                    {
                        if (pLista.Count == 1)
                        {
                            foreach (var item in pLista)
                            {
                                if (item.b_estadoV == "1")
                                {
                                    GridView1.DataSource = pLista;
                                    GridView1.DataBind();
                                }
                                else
                                {
                                    GridView1.DataSource = null;
                                    GridView1.DataBind();
                                   // Label lblEmptyMessage = GridView1.Controls[0].Controls[0].FindControl("lblEmptyMessage") as Label;
                                    throw new Exception("Este contenedor no se encuentra retenido: " + txtBuscar.Text);
                                }
                            }
                        }
                        else if(pLista.Count > 1)
                        {
                            for (int i = 0; i < pLista.Count; i++)
                            {
                                if (pLista[i].b_estadoV == "1")
                                {
                                    pLista1.Add(pLista[i]);
                                }
                            }

                            if (pLista1.Count > 0)
                            {
                                GridView1.DataSource = pLista1;
                                GridView1.DataBind();
                            }
                            else
                            {
                                GridView1.DataSource = null;
                                GridView1.DataBind();
                               // Label lblEmptyMessage = GridView1.Controls[0].Controls[0].FindControl("lblEmptyMessage") as Label;
                                throw new Exception("Este contenedor no se encuentra retenido: " + txtBuscar.Text);
                            }
                        }
                    }
                    else
                    {
                        GridView1.DataSource = null;
                        GridView1.DataBind();
                       // Label lblEmptyMessage = GridView1.Controls[0].Controls[0].FindControl("lblEmptyMessage") as Label;
                        throw new Exception("No se poseen registros de este contenedor: " + txtBuscar.Text);
                    }

                    
                }
                else
                {
                    GridView1.DataSource = null;
                    GridView1.DataBind();
                  //  Label lblEmptyMessage = GridView1.Controls[0].Controls[0].FindControl("lblEmptyMessage") as Label;
                    throw new Exception("Este número no posee los 11 caracteres " + txtBuscar.Text);
                }

                GridView1.HeaderRow.Cells[0].Attributes["data-class"] = "expand";

                // GridView1.HeaderRow.Cells[1].Attributes["data-hide"] = "phone";
                GridView1.HeaderRow.Cells[3].Attributes["data-hide"] = "phone";
                GridView1.HeaderRow.Cells[4].Attributes["data-hide"] = "phone";
                GridView1.HeaderRow.Cells[5].Attributes["data-hide"] = "phone";
                GridView1.HeaderRow.Cells[6].Attributes["data-hide"] = "phone";
                GridView1.HeaderRow.Cells[7].Attributes["data-hide"] = "phone";
         
                //GridView1.HeaderRow.Cells[8].Attributes["data-hide"] = "phone";

                GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;

                GridView1.FooterRow.Cells[0].Attributes["text-align"] = "center";
                GridView1.FooterRow.TableSection = TableRowSection.TableFooter;
                //  ViewState["EmployeeList"] = GridView1.DataSource;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ex.Message + "');", true);
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DateTime f_liberados;
                DateTime f_tramite;
                DateTime f_rpatio;
                f_liberados = Convert.ToDateTime(e.Row.Cells[5].Text);
                f_tramite = Convert.ToDateTime(e.Row.Cells[6].Text);
                f_rpatio = Convert.ToDateTime(e.Row.Cells[3].Text);

                if (f_liberados > FIRST_GOOD_DATE)
                { }
                else
                {
                    e.Row.Cells[5].Text = "";
                }

                if (f_tramite > FIRST_GOOD_DATE)
                { }
                else
                {
                    e.Row.Cells[6].Text = "";
                }
                if (f_rpatio > FIRST_GOOD_DATE)
                { }
                else
                {
                    e.Row.Cells[3].Text = "";
                }

            }
        }
    }
}