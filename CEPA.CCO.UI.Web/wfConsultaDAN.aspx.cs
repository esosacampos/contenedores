using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CEPA.CCO.Entidades;
using CEPA.CCO.BL;
using CEPA.CCO.DAL;
using System.Drawing;

namespace CEPA.CCO.UI.Web
{
    public partial class wfConsultaDAN : System.Web.UI.Page
    {
        private static readonly DateTime FIRST_GOOD_DATE = new DateTime(1900, 01, 01);
        private static readonly DateTime SECOND_GOOD_DATE = new DateTime(2016, 01, 01);

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            List<DetaNaviera> pLista = new List<DetaNaviera>();
            List<DetaNaviera> pLista1 = new List<DetaNaviera>();
            try
            {
                string b_valido = null;
                string mensaje = null;
                string label = null;
                if (txtBuscar.Text.Length == 11)
                {
                    pLista = DetaNavieraDAL.ConsultarDAN(txtBuscar.Text, Session["c_naviera"].ToString()).OrderByDescending(a => a.n_folio).ToList();

                    if (pLista.Count > 0)
                    {
                        if (pLista.Count == 1)
                        {
                            foreach (var item in pLista)
                            {
                                b_valido = item.b_retenido;
                                if (item.b_estadoV == "1")
                                {
                                    if (b_valido == "0")
                                    {
                                        mensaje = "Este contenedor no se encuentra retenido: " + txtBuscar.Text;
                                        label = "# " + txtBuscar.Text.ToUpper() +  " ESTA" + " <strong><u><font color='green'>LIBERADO" + "</font></u></strong>";
                                    }
                                    else
                                    {
                                        mensaje = "Este contenedor se encuentra retenido: " + txtBuscar.Text;
                                        label = "# " + txtBuscar.Text.ToUpper() + " ESTA" + " <strong><u><font color='red'>RETENIDO" + "</font></u></strong>";
                                    }

                                    ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + mensaje + "');", true);
                                    GridView1.DataSource = pLista;
                                    GridView1.DataBind();
                                }
                                else
                                {
                                    if (b_valido == "0")
                                    {
                                        mensaje = "Este contenedor no se encuentra retenido: " + txtBuscar.Text;
                                        label = "# " + txtBuscar.Text.ToUpper() + " ESTA" + " <strong><u><font color='green'>LIBERADO" + "</font></u></strong>";
                                    }
                                    else
                                    {
                                        mensaje = "Este contenedor se encuentra retenido: " + txtBuscar.Text;
                                        label = "# " + txtBuscar.Text.ToUpper() + " ESTA" + " <strong><u><font color='red'>RETENIDO" + "</font></u></strong>";
                                    }

                                    ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + mensaje + "');", true);
                                    GridView1.DataSource = null;
                                    GridView1.DataBind();
                                   // Label lblEmptyMessage = GridView1.Controls[0].Controls[0].FindControl("lblEmptyMessage") as Label;
                                    //throw new Exception("Este contenedor no se encuentra retenido: " + txtBuscar.Text);
                                }
                            }
                        }
                        else if(pLista.Count > 1)
                        {

                            
                            for (int i = 0; i < pLista.Count; i++)
                            {
                                if(pLista[i].b_estadoV == "1" && pLista[i].b_estado != "CANCELADO")
                                {
                                    pLista1.Add(pLista[i]);
                                }
                            }

                            if (pLista1.Count > 0)
                            {
                                foreach (var item in pLista1)
                                {
                                    if (item.b_retenido == "0")
                                    {
                                        mensaje = "Este contenedor no se encuentra retenido: " + txtBuscar.Text;
                                        label = "# " + txtBuscar.Text.ToUpper() + " ESTA" + " <strong><u><font color='green'>LIBERADO" + "</font></u></strong>";
                                    }
                                    else
                                    {
                                        mensaje = "Este contenedor se encuentra retenido: " + txtBuscar.Text;
                                        label = "# " + txtBuscar.Text.ToUpper() + " ESTA" + " <strong><u><font color='red'>RETENIDO" + "</font></u></strong>";
                                    }
                                }
                                

                                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + mensaje + "');", true);
                                GridView1.DataSource = pLista.OrderByDescending(a=> a.f_retenido);
                                GridView1.DataBind();
                            }
                            else
                            {
                                foreach (var item in pLista)
                                {
                                    if (item.b_retenido == "0")
                                    {
                                        mensaje = "Este contenedor no se encuentra retenido: " + txtBuscar.Text;
                                        label = "# " + txtBuscar.Text.ToUpper() + " ESTA" + " <strong><u><font color='green'>LIBERADO" + "</font></u></strong>";
                                    }
                                    else
                                    {
                                        mensaje = "Este contenedor se encuentra retenido: " + txtBuscar.Text;
                                        label = "# " + txtBuscar.Text.ToUpper() + " ESTA" + " <strong><u><font color='red'>RETENIDO" + "</font></u></strong>";
                                    }
                                }

                                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + mensaje + "');", true);
                                GridView1.DataSource = null;
                                GridView1.DataBind();
                               // Label lblEmptyMessage = GridView1.Controls[0].Controls[0].FindControl("lblEmptyMessage") as Label;
                                //throw new Exception("Este contenedor no se encuentra retenido: " + txtBuscar.Text);
                            }
                        }
                    }
                    else
                    {
                        GridView1.DataSource = null;
                        GridView1.DataBind();
                       // Label lblEmptyMessage = GridView1.Controls[0].Controls[0].FindControl("lblEmptyMessage") as Label;
                        label = "# " + txtBuscar.Text.ToUpper() + " NO SE POSEEN REGISTROS";
                        throw new Exception("No se poseen registros de este contenedor: " + txtBuscar.Text);
                    }

                    b_retenido.Text = label;
                }
                else
                {
                    GridView1.DataSource = null;
                    GridView1.DataBind();
                  //  Label lblEmptyMessage = GridView1.Controls[0].Controls[0].FindControl("lblEmptyMessage") as Label;
                    label = "# " + txtBuscar.Text.ToUpper() + " NO POSEE 11 CARACTERES";
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
                DateTime f_recepcion;
                DateTime f_ini_dan;
                DateTime f_retenido;
                f_recepcion = Convert.ToDateTime(e.Row.Cells[4].Text);
                f_rpatio = Convert.ToDateTime(e.Row.Cells[5].Text);
                f_retenido = Convert.ToDateTime(e.Row.Cells[6].Text);
                f_tramite = Convert.ToDateTime(e.Row.Cells[7].Text);
                f_ini_dan = Convert.ToDateTime(e.Row.Cells[8].Text);
                f_liberados = Convert.ToDateTime(e.Row.Cells[10].Text);

                HiddenField hEstado = (HiddenField)e.Row.FindControl("hEstado") as HiddenField;

                if(hEstado.Value == "CANCELADO")
                {
                    e.Row.BackColor = Color.FromName("#EB7A7A");
                    e.Row.ForeColor = Color.White;  
                }

                if (f_liberados > FIRST_GOOD_DATE)
                { }
                else
                {
                    e.Row.Cells[10].Text = "";
                }

                if (f_tramite > FIRST_GOOD_DATE)
                { }
                else
                {
                    e.Row.Cells[7].Text = "";
                }
                if (f_rpatio > FIRST_GOOD_DATE)
                { }
                else
                {
                    e.Row.Cells[5].Text = "";
                }
                if (f_recepcion > FIRST_GOOD_DATE)
                { }
                else
                {
                    e.Row.Cells[4].Text = "";
                }
                if (f_ini_dan > FIRST_GOOD_DATE)
                { }
                else
                {
                    e.Row.Cells[8].Text = "";
                }
                if (f_retenido > FIRST_GOOD_DATE)
                { }
                else
                {
                    e.Row.Cells[6].Text = "";
                }
                if (f_recepcion > SECOND_GOOD_DATE)
                { }
                else
                {
                    e.Row.Cells[4].Text = "";
                }
                if (f_rpatio > SECOND_GOOD_DATE)
                { }
                else
                {
                    e.Row.Cells[5].Text = "";
                }

            }
        }
    }
}